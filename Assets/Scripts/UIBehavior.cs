using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class UIBehavior : MonoBehaviour {

    public Text mTimerText;
    public float timeInFloat;
    private AudioSource mAudioSource;
	void Start ()
    {
        mAudioSource = null;
	}
	
	void Update ()
    {

	    if (mAudioSource)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(mAudioSource.clip.length - mAudioSource.time);
            string formatTime = string.Format("{0:D2}:{1:D2}", timeSpan.Seconds, timeSpan.Milliseconds);
            float.TryParse(formatTime, out timeInFloat);
            mTimerText.text = formatTime;
            if (timeSpan.Milliseconds == 0.0f)
            {
                mAudioSource = null;
            }
        }    
	}

    public void SetAudioSource(AudioSource audioSource)
    {
        mAudioSource = audioSource;
    }
}
