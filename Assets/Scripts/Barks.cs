using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barks : MonoBehaviour
{
    public Mouth mouth;

    public AudioClip[] barks;
    [UnityEngine.Serialization.FormerlySerializedAs("source")]
    public AudioSource barkingSource;
    int lastPlayedBark = -1;

    public AudioSource pantingSource;
    public AudioClip[] pants;
    int lastPlayedPant = -1;

    public Transform tongue;
    public float maxTongueRotation = 75f;
    Quaternion originalTongueRotation = Quaternion.identity;

    Quaternion fromRotation, toRotation;


    // Start is called before the first frame update
    void Start()
    {
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

        if (barkingSource.isPlaying)
        {
            float timeFactor = barkingSource.time / barks[lastPlayedBark].length;
            tongue.localRotation = Quaternion.Lerp(fromRotation, toRotation, Easing.Back.Out(timeFactor));
        }

        if (!pantingSource.isPlaying)
        {
            Pant();
        }
    }

    public void Bark()
    {
        if (barkingSource.isPlaying)
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

        barkingSource.clip = barks[chosenBark];

        lastPlayedBark = chosenBark;
        barkingSource.Play();

        fromRotation = tongue.localRotation;
        toRotation = originalTongueRotation * Quaternion.AngleAxis(Random.Range(-maxTongueRotation, maxTongueRotation), Vector3.forward);
    }

    public void Pant() {
        if (pantingSource.isPlaying)
            return;

        List<int> possiblePants = new List<int>();
        for (int i = 0; i < pants.Length; ++i)
        {
            if (i != lastPlayedPant)
            {
                possiblePants.Add(i);
            }
        }

        int chosenPant = possiblePants[Random.Range(0, possiblePants.Count)];

        pantingSource.clip = pants[chosenPant];

        lastPlayedPant = chosenPant;
        pantingSource.Play();
    }

}
