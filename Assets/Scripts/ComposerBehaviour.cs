using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;


public struct Sound
{
    public float timeStamp;
    public AudioClip audioClip;
    public float lengthInBeats;
}

public class SoundAction
{
    public int type; //0: single sound   1: solo (list of Sound)
    public Sound singleSound;
    public float startTime;
    public float endTime;
    public List<Sound> solo;
}

public class ComposerBehaviour : MonoBehaviour
{
    private List<SoundAction> composition;
    private AudioSource[] soloSpeakers;
    private AudioSource[] normalSpeakers;
    public int playbackBPM = 60;

    private int maxNormalSpeakers = 10;
    private int maxSoloSpeakers = 5;

    private int _currentNormalSpeaker;
    private int currentNormalSpeaker
    {
        get
        {
            return _currentNormalSpeaker;
        }
        set
        {
            if (value >= maxNormalSpeakers)
                _currentNormalSpeaker = 0;
            else if (value < 0)
                _currentNormalSpeaker = maxNormalSpeakers;
            else
                _currentNormalSpeaker = value;
        }
    }

    private int _currentSoloSpeaker;
    private int currentSoloSpeaker
    {
        get
        {
            return _currentSoloSpeaker;
        }
        set
        {
            if (value >= maxSoloSpeakers)
                _currentSoloSpeaker = 0;
            else if (value < 0)
                _currentSoloSpeaker = maxSoloSpeakers;
            else
                _currentSoloSpeaker = value;
        }
    }


    SoundAction currentSolo;
    bool isRecordingSolo = false;
    bool isPlaying = false;

    // Use this for initialization
    void Awake()
    {
        composition = new List<SoundAction>();
        soloSpeakers = new AudioSource[maxSoloSpeakers];
        normalSpeakers = new AudioSource[maxNormalSpeakers];
        for (int i = 0; i < maxNormalSpeakers; i++)
        {
            normalSpeakers[i] = gameObject.AddComponent<AudioSource>();
        }

        for (int i = 0; i < maxSoloSpeakers; i++)
        {
            soloSpeakers[i] = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame


    public void AddSingleSound(CollectibleBehavior src)
    {
        if (isPlaying) return;
        if (isRecordingSolo) return;

        Sound s;
        s.timeStamp = 0;
        s.audioClip = src.m_audioSource.clip;
        s.lengthInBeats = src.lengthInBeats;

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
        s.lengthInBeats = 0;

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
                PlayInAvailableSpeaker(a.singleSound.audioClip, false);
                yield return new WaitForSeconds(60.0f / (float)playbackBPM * a.singleSound.lengthInBeats);
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
            PlayInAvailableSpeaker(a.solo[i].audioClip, true);
            if (i != count - 1)
                yield return new WaitForSeconds(a.solo[i+1].timeStamp - a.solo[i].timeStamp);
        }
    }

    private void PlayInAvailableSpeaker(AudioClip c, bool isSolo)
    {
        if (isSolo)
        {
            soloSpeakers[currentSoloSpeaker].clip = c;
            soloSpeakers[currentSoloSpeaker].Play();
            currentSoloSpeaker++;
        }
        else
        {
            normalSpeakers[currentNormalSpeaker].clip = c;
            normalSpeakers[currentNormalSpeaker].Play();
            currentNormalSpeaker++;
        }
        
    }
}