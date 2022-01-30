using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    List<Biteable> inRange = new List<Biteable>();


    public float grabSpeed = 1f;
    public float rotationGrabSpeedMult = 120f;
    float grabbingTime = 0;
    float grabbingTimeLeft = 0;

    Transform from;
    Transform to;

    Biteable currentlyBiting = null;

    // Start is called before the first frame update
    void Start()
    {
        from = new GameObject("Mouth help from").transform;
        to = new GameObject("Mouth help to").transform;
        to.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyBiting != null)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                Release();
            }
        }

        if (inRange.Count > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            int closest = 0;
            float closestDistance = Vector3.Distance(inRange[0].transform.position, transform.position);

            for (int i = 1; i < inRange.Count; ++i) {
                float d = Vector3.Distance(inRange[i].transform.position, transform.position);

                if (d < closestDistance)
                {
                    closest = i;
                    closestDistance = d;

                }
            }

            Bite(inRange[closest]);
        }
    }

    private void FixedUpdate()
    {
        if (currentlyBiting != null)
        {
            if (grabbingTimeLeft > 0)
            {
                grabbingTimeLeft = Mathf.Max(0, grabbingTimeLeft - Time.fixedDeltaTime);

                float grabbinghFactor = 1f - (grabbingTimeLeft / grabbingTime);
                grabbinghFactor = Easing.Cubic.Out(grabbinghFactor);

                currentlyBiting.rb.MovePosition(Vector3.Lerp(from.position, to.position, grabbinghFactor));
                currentlyBiting.rb.MoveRotation(Quaternion.Lerp(from.rotation, to.rotation, grabbinghFactor));

                if (grabbinghFactor >= 1f)
                {
                    currentlyBiting.FixTo(transform);
                }
            }
        }
    }

    void Bite(Biteable b)
    {
        currentlyBiting = b;
        from.transform.position = currentlyBiting.transform.position;
        from.transform.rotation = currentlyBiting.transform.rotation;

        to.transform.localPosition = currentlyBiting.relPos;
        to.transform.localRotation = currentlyBiting.relRot;


        float time = Vector3.Distance(from.position, to.position) / grabSpeed;
        float timeRotation = Quaternion.Angle(from.rotation, to.rotation) / (grabSpeed * rotationGrabSpeedMult);
        grabbingTimeLeft = Mathf.Max(timeRotation, time);
        grabbingTime = grabbingTimeLeft;
        b.Grab();
    }

    void Release()
    {
        currentlyBiting.Release();
        currentlyBiting = null;
    }

    public void OnMouthRange(Biteable biteable) {
        if (!inRange.Contains(biteable))
            inRange.Add(biteable);
    }

    public void FarFromMouth(Biteable biteable)
    {
        if (inRange.Contains(biteable))
            inRange.Remove(biteable);
    }
}
