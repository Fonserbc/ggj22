using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biteable : MonoBehaviour
{
    public Vector3 relPos;
    public Quaternion relRot;

    Rigidbody rb;
    Transform oldParent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        oldParent = transform.parent;
    }

    Mouth closeMouth;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            closeMouth = other.GetComponent<Mouth>();
            closeMouth.OnMouthRange(this);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (closeMouth)
                closeMouth.FarFromMouth(this);
            closeMouth = null;
        }
    }

    public void Grab() {

    }

    public void Release() {

    }
}
