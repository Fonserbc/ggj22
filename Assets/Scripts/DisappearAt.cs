using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAt : MonoBehaviour
{
    Transform to;
    float totalTime;
    float currentTime;

    Vector3 startScale = Vector3.one;

    public void Goodbye(Transform towards, float time) {
        currentTime = 0;
        to = towards;
        totalTime = time;
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        float factor = Mathf.Clamp01(currentTime / totalTime);

        transform.position = Vector3.Lerp(transform.position, to.position, factor);

        transform.localScale = startScale * (1f - factor);

        if (factor >= 1)
            Destroy(gameObject);
    }
}
