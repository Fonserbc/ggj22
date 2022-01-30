using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundsOnKick : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioSource source;
    int lastPlayedClip = -1;
    float lastPlayedTime = 0;
    public bool cutPlayingClip = true;
    public float minPlayingDelay = 0.7f;

    public bool changePitch = false;
    public Vector2 pitchMinMax = Vector2.one;

    // Start is called before the first frame update
    void Start()
    {
        if (!source) source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Respawn"))
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        if (!cutPlayingClip && source.isPlaying)
            return;

        if (Time.time - lastPlayedTime < minPlayingDelay)
            return;


        if (source.isPlaying)
            source.Stop();

        int chosenClip = 0;

        if (clips.Length > 1)
        {
            List<int> possibleClips = new List<int>();
            for (int i = 0; i < clips.Length; ++i)
            {
                if (i != lastPlayedClip)
                {
                    possibleClips.Add(i);
                }
            }

            chosenClip = possibleClips[Random.Range(0, possibleClips.Count)];
        }

        source.clip = clips[chosenClip];

        if (changePitch)
        {
            source.pitch = Random.Range(pitchMinMax.x, pitchMinMax.y);
        }

        lastPlayedClip = chosenClip;
        source.Play();

        lastPlayedTime = Time.time;
    }
}
