using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mac : MonoBehaviour
{
    public Transform centerOfMass;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
    }
}
