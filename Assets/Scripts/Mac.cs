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
            enabled = false;
        }

        if (hitTime > 0)
        {
            hitTime -= Time.deltaTime;

            if (hitTime <= 0)
            {
                rb.centerOfMass = centerOfMass.localPosition;
            }
        }
    }

    float hitTime = 0f;



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")) {
            rb.centerOfMass = Vector3.zero;
            hitTime = 1f;
        }
    }
}
