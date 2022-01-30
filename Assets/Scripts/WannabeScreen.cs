using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WannabeScreen : MonoBehaviour
{
    public static List<WannabeScreen> screens = new List<WannabeScreen>();

    [HideInInspector]
    public Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        screens.Add(this);
    }

    private void OnDestroy()
    {
        screens.Remove(this);
    }
}
