using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barks : MonoBehaviour
{
    public Mouth mouth;

    public AudioClip[] barks;
    public AudioSource source;

    public Transform tongue;
    public float maxTongueRotation = 75f;
    Quaternion originalTongueRotation = Quaternion.identity;

    Quaternion fromRotation, toRotation;

    int lastPlayedBark = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (source == null)
            source = GetComponent<AudioSource>();

        originalTongueRotation = tongue.localRotation;
    }

    bool mouthBusy = false;

    // Update is called once per frame
    void Update()
    {
        bool mouthBusyNow = mouth.IsBittingSomething();

        if (mouthBusy != mouthBusyNow)
        {
            // TODO muffle barks?
        }


        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Bark();
        }

        if (source.isPlaying)
        {
            float timeFactor = source.time / barks[lastPlayedBark].length;
            tongue.localRotation = Quaternion.Lerp(fromRotation, toRotation, Easing.Back.Out(timeFactor));
        }
    }

    public void Bark()
    {
        if (source.isPlaying)
            return;

        List<int> possibleBarks = new List<int>();
        for (int i = 0; i < barks.Length; ++i)
        {
            if (i != lastPlayedBark)
            {
                possibleBarks.Add(i);
            }
        }

        int chosenBark = possibleBarks[Random.Range(0, possibleBarks.Count)];

        source.clip = barks[chosenBark];

        lastPlayedBark = chosenBark;
        source.Play();

        fromRotation = tongue.localRotation;
        toRotation = originalTongueRotation * Quaternion.AngleAxis(Random.Range(-maxTongueRotation, maxTongueRotation), Vector3.forward);
    }

}
