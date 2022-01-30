using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mac : MonoBehaviour
{
    public Transform centerOfMass;
    Rigidbody rb;

    public GameObject onWin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.localPosition;
        onWin.SetActive(false);
    }

    private void Update()
    {
        if (transform.position.y < -10f) {
            onWin.SetActive(true);
            Debug.Log("Win");
        }
    }
}
