using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerScreen : MonoBehaviour
{
    public Texture2D[] animation;
    public int imagesPerSecond = 24;
    float secondsPerImage {
        get { return 1f / (float)imagesPerSecond;  } 
    }
    float accTime = 0;
    int it = 0;

    public MeshRenderer[] renderers;

    MaterialPropertyBlock block;

    // Start is called before the first frame update
    void Start()
    {
        block = new MaterialPropertyBlock();
        renderers[0].GetPropertyBlock(block);

        SetTexture(animation[it]);
    }

    // Update is called once per frame
    void Update()
    {
        accTime += Time.deltaTime;

        if (accTime > secondsPerImage) {
            accTime -= secondsPerImage;

            it = (it + 1) % animation.Length;
            SetTexture(animation[it]);
        }
    }

    void SetTexture(Texture2D tex)
    {
        block.SetTexture("_MainTex", tex);
        foreach (Renderer rend in renderers)
        {
            rend.SetPropertyBlock(block);
        }
    }
}
