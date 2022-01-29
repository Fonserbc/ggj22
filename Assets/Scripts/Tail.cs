using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    public float relaxedAngle = 30f;
    public float happyAngle = 0f;
    public float upRightSpeed = 1f;

    public float leftRightAngle = 35f;
    public float leftRightSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(180f - Mathf.Lerp(relaxedAngle, happyAngle, Mathf.Clamp01(Mathf.Sin(Time.time * upRightSpeed))),
            Mathf.Sin(Time.time * leftRightSpeed) * leftRightAngle, 0f);
    }
}
