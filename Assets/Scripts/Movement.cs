using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Interaction")]
    public Transform rightTouch;
    public Transform leftTouch;
    bool touchingLeft = false;
    bool touchingRight = false;

    [Header("Body")]
    public Transform back;
    public Transform front;
    public float forwardSpeed = 5f;
    public float turningTorque = 50f;
    public float maxSpeed = 1f;

    Rigidbody rb;
    Rigidbody backRb;

    [Header("Front Legs")]
    public Rigidbody rightLeg;
    public Rigidbody leftLeg;
    public Transform rightRest, leftRest;
    public Transform rightWalk, leftWalk;
    public float distanceBetweenStep = 0.2f;
    public float minStepSpeed = 0.05f;
    public float feetSpeed = 1f;
    public float feetRotationSpeed = 90f;
    float accumulatedDistance = 0;

    Transform targetRight;
    Transform prevRight;
    Transform targetLeft;
    Transform prevLeft;
    float movingTimeLeft, movingTimeRight;
    float moveTimeL, moveTimeR;

    [Header("Head and camera")]
    public float timeUntilCameraFix = 0.5f;
    float lastCameraMovement = 0;
    float timeBodyMoving = 0;

    public Transform neck;
    public Transform head;

    public float headTurningSpeed = 1f;

    float headYaw = 0;
    float headPitch = 0;

    float neckYaw = 0;
    float neckPitch = 0;
    float neutralHeadPitch, neutralNeckPitch;

    public Vector2 minMaxYawDifference = new Vector2(-25f, 25f);
    public Vector2 minMaxNeckYaw = new Vector2(-30f, 30f);

    public Vector2 minMaxPitchDifference = new Vector2(-25f, 25f);
    public Vector2 minMaxNeckPitch = new Vector2(-30f, 30f);

    Vector3 startPosition;
    Quaternion startRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        backRb = back.GetComponent<Rigidbody>();

        neutralNeckPitch = Vector3.SignedAngle(transform.forward, neck.forward, neck.right);
        neutralHeadPitch = Vector3.SignedAngle(neck.forward, head.forward, neck.right);
        lastFrontPos = front.position;

        startPosition = backRb.transform.position;
        startRotation = backRb.transform.rotation;

        targetLeft = leftRest;
        targetRight = rightRest;
    }

    Vector3 lastFrontPos = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (Cursor.lockState == CursorLockMode.Locked && Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        if (Cursor.lockState == CursorLockMode.Locked)
            MoveHead(out float wantedExtraMouseYaw);

        // TODO do something with yawdelta to tilt the body on that direction

        if (Input.GetKeyDown(KeyCode.R))
        {
            backRb.position = startPosition;
            backRb.rotation = startRotation;
        }

        if (Input.GetMouseButtonDown(0))
        {
            touchingLeft = true;
            touchingRight = false;
        }
        else if (touchingLeft && Input.GetMouseButtonUp(0))
            touchingLeft = false;

        if (Input.GetMouseButtonDown(1))
        {
            touchingLeft = false;
            touchingRight = true;
        }
        else if (touchingRight && Input.GetMouseButtonUp(1))
            touchingRight = false;

        //
        float movingFactor = Mathf.Clamp01(Vector3.Dot(front.forward, backRb.velocity) / maxSpeed);
        if (!Mathf.Approximately(movingFactor, 0) && timeBodyMoving > timeUntilCameraFix && Time.time - lastCameraMovement > timeUntilCameraFix)
        {
            if (headPitch < 0) headPitch = Mathf.MoveTowardsAngle(headPitch, 0, Time.deltaTime * movingFactor * headTurningSpeed * 0.5f);
            headYaw = Mathf.MoveTowardsAngle(headYaw, 0, Time.deltaTime * movingFactor * headTurningSpeed * 0.5f);
            if (neckPitch < 0) neckPitch = Mathf.MoveTowardsAngle(neckPitch, 0, Time.deltaTime * movingFactor * headTurningSpeed * 0.5f);
            neckYaw = Mathf.MoveTowardsAngle(neckYaw, 0, Time.deltaTime * movingFactor * headTurningSpeed * 0.5f);
        }
    }

    private void FixedUpdate()
    {
        MoveBody();

        Vector3 deltaPosFront = front.position - lastFrontPos;
        float frameMovement = deltaPosFront.magnitude;

        float frontSpeed = frameMovement / Time.fixedDeltaTime;

        if (frontSpeed > minStepSpeed)
        {
            accumulatedDistance += frameMovement;

            bool left = (accumulatedDistance / distanceBetweenStep) % 2f >= 1f;

            MoveFeet(true, left ? leftRest : leftWalk, feetSpeed);
            MoveFeet(false, left ? rightWalk : rightRest, feetSpeed);
        }
        else
        {
            if (touchingLeft) MoveFeet(true, leftTouch, feetSpeed);
            else MoveFeet(true, leftRest, feetSpeed);

            if (touchingRight) MoveFeet(false, rightTouch, feetSpeed);
            else MoveFeet(false, rightRest, feetSpeed);
        }

        lastFrontPos = front.position;

        //
        if (movingTimeLeft > 0)
        {
            movingTimeLeft = Mathf.Max(0, movingTimeLeft - Time.fixedDeltaTime);
            float leftFactor = 1f - (movingTimeLeft / moveTimeL);
            leftFactor = Easing.Cubic.Out(leftFactor);

            leftLeg.MovePosition(Vector3.Lerp(prevLeft.position, targetLeft.position, leftFactor));
            leftLeg.MoveRotation(Quaternion.Slerp(prevLeft.rotation, targetLeft.rotation, leftFactor));
        }
        else {
            leftLeg.MovePosition(targetLeft.position);
            leftLeg.MoveRotation(targetLeft.rotation);
        }

        if (movingTimeRight > 0)
        {
            movingTimeRight = Mathf.Max(0, movingTimeRight - Time.fixedDeltaTime);
            float rightFactor = 1f - (movingTimeRight / moveTimeR);
            rightFactor = Easing.Cubic.Out(rightFactor);

            rightLeg.MovePosition(Vector3.Lerp(prevRight.position, targetRight.position, rightFactor));
            rightLeg.MoveRotation(Quaternion.Slerp(prevRight.rotation, targetRight.rotation, rightFactor));
        }
        else
        {
            rightLeg.MovePosition(targetRight.position);
            rightLeg.MoveRotation(targetRight.rotation);
        }
    }

    void MoveFeet(bool left, Transform where, float speed) {
        if (left && where == targetLeft)
            return;
        if (!left && where == targetRight)
            return;

        if (left)
        {
            prevLeft = targetLeft;
            targetLeft = where;
            float time = Vector3.Distance(targetLeft.position, leftLeg.position) / speed;
            float timeRotation = Quaternion.Angle(targetLeft.rotation, leftLeg.rotation) / (speed * feetRotationSpeed);
            movingTimeLeft = Mathf.Max(timeRotation, time);
            moveTimeL = movingTimeLeft;
        }
        else {
            prevRight = targetRight;
            targetRight = where;
            float time = Vector3.Distance(targetRight.position, rightLeg.position) / speed;
            float timeRotation = Quaternion.Angle(targetRight.rotation, rightLeg.rotation) / (speed * feetRotationSpeed);
            movingTimeRight = Mathf.Max(timeRotation, time);
            moveTimeR = movingTimeRight;
        }
    }

    void MoveHead(out float wantToRotateMore) {
        float hAxis = Input.GetAxis("Mouse X");
        float vAxis = Input.GetAxis("Mouse Y");

        if (!Mathf.Approximately(Mathf.Abs(hAxis) + Mathf.Abs(vAxis), 0f)) {
            lastCameraMovement = Time.time;
        }

        float dt = Mathf.Min(1 / 24f, Time.deltaTime);

        float yawDelta = hAxis * headTurningSpeed * dt;
        float pitchDelta = -vAxis * headTurningSpeed * dt;


        // Yaw
        if (yawDelta > 0)
        {
            if (headYaw + yawDelta < minMaxYawDifference.y)
            {
                if (neckYaw < 0) {
                    neckYaw += yawDelta * 0.5f;
                    headYaw += yawDelta * 0.5f;
                }
                else headYaw += yawDelta;
                yawDelta = 0;
            }
            else
            {
                yawDelta -= minMaxYawDifference.y - headYaw;
                headYaw = minMaxYawDifference.y;

                if (neckYaw + yawDelta < minMaxNeckYaw.y)
                {
                    neckYaw += yawDelta;
                    yawDelta = 0;
                }
                else
                {
                    yawDelta -= minMaxNeckYaw.y - neckYaw;
                    neckYaw = minMaxNeckYaw.y;

                    float pitchFactor = Mathf.Clamp01((neckPitch + headPitch) / minMaxNeckPitch.x);
                    pitchDelta += (neckPitch + headPitch < 0) ? Mathf.Abs(yawDelta * pitchFactor) : -Mathf.Abs(yawDelta * pitchFactor);
                }
            }
        }
        else
        {
            if (headYaw + yawDelta > minMaxYawDifference.x)
            {
                if (neckYaw > 0)
                {
                    neckYaw += yawDelta * 0.5f;
                    headYaw += yawDelta * 0.5f;
                }
                else headYaw += yawDelta;
                yawDelta = 0;
            }
            else
            {
                yawDelta -= minMaxYawDifference.x - headYaw;
                headYaw = minMaxYawDifference.x;

                if (neckYaw + yawDelta > minMaxNeckYaw.x)
                {
                    neckYaw += yawDelta;
                    yawDelta = 0;
                }
                else
                {
                    yawDelta -= minMaxNeckYaw.x - neckYaw;
                    neckYaw = minMaxNeckYaw.x;

                    float pitchFactor = Mathf.Clamp01((neckPitch + headPitch) / minMaxNeckPitch.y);
                    pitchDelta += (neckPitch + headPitch < 0) ? Mathf.Abs(yawDelta * pitchFactor) : -Mathf.Abs(yawDelta * pitchFactor);
                }
            }
        }
        // Pitch
        if (pitchDelta > 0)
        {
            if (headPitch + pitchDelta < minMaxPitchDifference.y)
            {
                if (neckPitch < 0)
                {
                    neckPitch += pitchDelta * 0.5f;
                    headPitch += pitchDelta * 0.5f;
                }
                else headPitch += pitchDelta;

                pitchDelta = 0;
            }
            else
            {
                pitchDelta -= minMaxPitchDifference.y - headPitch;
                headPitch = minMaxPitchDifference.y;

                if (neckPitch + pitchDelta < minMaxNeckPitch.y)
                {
                    neckPitch += pitchDelta;
                    pitchDelta = 0;
                }
                else
                {
                    pitchDelta -= minMaxNeckPitch.y - neckPitch;
                    neckPitch = minMaxNeckPitch.y;
                }
            }
        }
        else
        {
            if (headPitch + pitchDelta > minMaxPitchDifference.x)
            {
                if (neckPitch > 0)
                {
                    neckPitch += pitchDelta * 0.5f;
                    headPitch += pitchDelta * 0.5f;
                }
                else headPitch += pitchDelta;

                pitchDelta = 0;
            }
            else
            {
                pitchDelta -= minMaxPitchDifference.x - headPitch;
                headPitch = minMaxPitchDifference.x;

                if (neckPitch + pitchDelta > minMaxNeckPitch.x)
                {
                    neckPitch += pitchDelta;
                    pitchDelta = 0;
                }
                else
                {
                    pitchDelta -= minMaxNeckPitch.x - neckPitch;
                    neckPitch = minMaxNeckPitch.x;
                }
            }
        }

        //



        neck.localRotation = Quaternion.Euler(neckPitch + neutralNeckPitch, neckYaw, 0);
        //head.localRotation = Quaternion.Euler(headPitch + neutralHeadPitch, headYaw, 0);

        //neck.localRotation = Quaternion.AngleAxis(neckPitch + neutralNeckPitch, Vector3.right) * Quaternion.AngleAxis(neckYaw, Vector3.up);
        head.localRotation = Quaternion.AngleAxis(headPitch + neutralHeadPitch, Vector3.right) * Quaternion.AngleAxis(headYaw, Vector3.up);

        wantToRotateMore = yawDelta;
    }

    void MoveBody() {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");

        backRb.AddRelativeTorque(Vector3.up * horizontal * turningTorque * backRb.mass, ForceMode.Force);
        //backRb.AddForceAtPosition(front.right * horizontal * speed, front.position + Vector3.up * 0f, ForceMode.Acceleration);
        backRb.AddForceAtPosition(front.forward * vertical * (vertical > 0? forwardSpeed : forwardSpeed * 0.5f) * backRb.mass, front.position, ForceMode.Force);
        //rb.AddForceAtPosition(transform.right * horizontal * speed, transform.position + Vector3.up * 0.35f, ForceMode.Acceleration);

        float velMagnitude = backRb.velocity.magnitude;
        if (velMagnitude > maxSpeed)
        {
            backRb.velocity = backRb.velocity.normalized * maxSpeed;
            velMagnitude = maxSpeed;
        }
        float angularMagnitude = backRb.angularVelocity.magnitude;
        if (angularMagnitude > maxSpeed)
        {
            backRb.angularVelocity = backRb.angularVelocity.normalized * maxSpeed;
            angularMagnitude = maxSpeed;
        }

        if (!Mathf.Approximately(Vector3.Dot(front.forward, backRb.velocity), 0f))
        {
            timeBodyMoving += Time.fixedDeltaTime;
        }
        else timeBodyMoving = 0f;

        Quaternion wantedRotation = Quaternion.FromToRotation(back.up, Vector3.up);

        Vector3 x = Vector3.Cross(back.up, Vector3.up);
        float theta = Mathf.Asin(x.magnitude);
        Vector3 w = x.normalized * theta / Time.fixedDeltaTime;
        Quaternion q = transform.rotation * backRb.inertiaTensorRotation;
        Vector3 T = q * Vector3.Scale(backRb.inertiaTensor, (Quaternion.Inverse(q) * w));
        backRb.AddTorque(T, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        float d = Vector3.Distance(neck.position, head.position);


    }
}
