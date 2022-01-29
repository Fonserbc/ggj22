using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{
    List<Biteable> inRange = new List<Biteable>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        }
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
