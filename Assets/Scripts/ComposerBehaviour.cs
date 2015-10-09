using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public struct Sound
{
    public float timeStamp;
    public AudioClip audioClip;
}

public class SoundAction
{
    public int type; //0: single sound   1: solo (list of Sound)
    public Sound singleSound;
    public float startTime;
    public float endTime;
    public List<Sound> solo;
}

[RequireComponent(typeof(AudioSource))]
public class ComposerBehaviour : MonoBehaviour
{
    private List<SoundAction> composition;
    private AudioSource myAudio;


    SoundAction currentSolo;
    bool isRecordingSolo = false;
    bool isPlaying = false;

    // Use this for initialization
    void Awake()
    {
        composition = new List<SoundAction>();
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame


    public void AddSingleSound(AudioSource src)
    {
        if (isPlaying) return;
        if (isRecordingSolo) return;

        Sound s;
        s.timeStamp = 0;
        s.audioClip = src.clip;

        SoundAction a = new SoundAction();
        a.type = 0;
        a.singleSound = s;
        
        composition.Add(a);
        Debug.Log("Stored a single sound");
    }


    public void StartSoloRecording()
    {
        if (isRecordingSolo) return;
        isRecordingSolo = true;

        SoundAction a = new SoundAction();
        a.type = 1;
        a.solo = new List<Sound>();
        a.startTime = Time.time;
        currentSolo = a;

        Debug.Log("Started a solo");
    }

    public void AddToSoloRecording(AudioSource src)
    {
        if (!isRecordingSolo) return;

        Sound s;
        s.timeStamp = Time.time;
        s.audioClip = src.clip;

        currentSolo.solo.Add(s);

        Debug.Log("Add to solo");
    }

    public void StopSoloRecording()
    {
        if (!isRecordingSolo) return;
        isRecordingSolo = false;

        currentSolo.endTime = Time.time;

        composition.Add(currentSolo);

        Debug.Log("Finished the solo");
    }


    public void PlayComposition()
    {
        isPlaying = true;
        StartCoroutine(PlayCompositionCoroutine());
        Debug.Log("Playing Composition");
    }

    IEnumerator PlayCompositionCoroutine()
    {
        foreach (SoundAction a in composition)
        {
            if (a.type == 0)
            {
                myAudio.clip = a.singleSound.audioClip;
                myAudio.Play();
                yield return new WaitForSeconds(a.singleSound.audioClip.length);
            }
            else
            {
                StartCoroutine(PlaySolo(a));
                yield return new WaitForSeconds(a.endTime - a.startTime);
            }
        }
        isPlaying = false;
    }

    IEnumerator PlaySolo(SoundAction a)
    {
        Debug.Log("Playing Solo"); 
        float offset = a.startTime;
        int count = a.solo.Count;
        for (int i = 0; i < count; i++)
        {
            myAudio.clip = a.solo[i].audioClip;
            myAudio.Play();
            if (i != count - 1)
                yield return new WaitForSeconds(a.solo[i+1].timeStamp - a.solo[i].timeStamp);
        }
    }
}