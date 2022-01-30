using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biteable : MonoBehaviour
{
    public Vector3 relPos;
    public Quaternion relRot;

    [HideInInspector]
    public Rigidbody rb;
    Transform oldParent;

    float mass;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();
        mass = rb.mass;
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
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        SetLayerRecursive(transform, 7);
    }

    public void FixTo(Transform p)
    {
        transform.SetParent(p);
        transform.localPosition = relPos;
        transform.localRotation = relRot;
        DestroyImmediate(rb);
    }

    public void Release()
    {
        if (rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = mass;
        }
        rb.isKinematic = false;
        transform.SetParent(oldParent);
        SetLayerRecursive(transform, 0);
    }

    void SetLayerRecursive(Transform t, int layer)
    {
        t.gameObject.layer = layer;

        foreach (Transform c in t)
        {
            SetLayerRecursive(c, layer);
        }
    }
}
