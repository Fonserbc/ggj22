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

    public bool playOnAllCollision = false;
    public bool velocityAffectsPitch = false;
    public float otherCollisionVolume = 0.5f;


    float startVolume = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if (!source) source = GetComponent<AudioSource>();
        startVolume = source.volume;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float magFactor = Random.Range(0, 1);
        if (velocityAffectsPitch)
        {
            float mag = collision.relativeVelocity.magnitude;
            magFactor = Mathf.Clamp01(mag / 5f);
        }

        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Respawn"))
        {
            PlaySound(startVolume, magFactor);
        }
        else if (playOnAllCollision)
        {
            PlaySound(startVolume * otherCollisionVolume * magFactor, magFactor);
        }
    }

    void PlaySound(float volume, float pitchFactor)
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
            source.pitch = Mathf.Lerp(pitchMinMax.x, pitchMinMax.y, pitchFactor);
        }

        source.volume = volume;
        lastPlayedClip = chosenClip;
        source.Play();

        lastPlayedTime = Time.time;
    }
}
