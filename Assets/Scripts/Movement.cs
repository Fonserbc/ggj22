using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform neck;
    public Transform head;

    public Transform back;

    public float headTurningSpeed = 1f;

    Rigidbody rb;

    float headYaw = 0;
    float headPitch = 0;

    float neckYaw = 0;
    float neckPitch = 0;

    public Vector2 minMaxYawDifference = new Vector2(-25f, 25f);
    public Vector2 minMaxNeckYaw = new Vector2(-30f, 30f);


    public Vector2 minMaxPitchDifference = new Vector2(-25f, 25f);
    public Vector2 minMaxNeckPitch = new Vector2(-30f, 30f);

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        neckPitch = Vector3.SignedAngle(Vector3.forward, neck.forward, neck.right);
        Debug.Log(neckPitch);
    }

    // Update is called once per frame
    void Update()
    {
        MoveHead(out float wantedExtraMouseYaw);


        // TODO do something with yawdelta to tilt the body on that direction
    }

    void MoveHead(out float wantToRotateMore) {
        float hAxis = Input.GetAxis("Mouse X");
        float vAxis = Input.GetAxis("Mouse Y");

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
                    yawDelta -= minMaxYawDifference.x - neckYaw;
                    neckYaw = minMaxNeckYaw.x;
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
                Debug.Log("Positive, head at max");

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



        neck.localRotation = Quaternion.Euler(neckPitch, neckYaw, 0);
        head.localRotation = Quaternion.Euler(headPitch, headYaw, 0);

        wantToRotateMore = yawDelta;
    }

    private void OnDrawGizmosSelected()
    {
        float d = Vector3.Distance(neck.position, head.position);


    }
}
