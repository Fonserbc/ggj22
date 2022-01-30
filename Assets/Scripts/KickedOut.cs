using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickedOut : MonoBehaviour
{
    public string what = "something";

    private void Start()
    {
        Biteable b = GetComponent<Biteable>();
        if (b) {
            what = b.what;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -10f)
        {
            AchievementText.instance.ReportOutOfTheWindow(what);
            enabled = false;
        }
    }
}
