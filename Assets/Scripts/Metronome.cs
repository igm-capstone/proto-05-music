using UnityEngine;
using System.Collections;

public class Metronome : MonoBehaviour
{

    public float BPS;
    public bool isPlaying;

    AudioSource sndPlyr;

    // Use this for initialization
    void Start()
    {

        isPlaying = false;

        sndPlyr = GetComponent<AudioSource>();

    }

    // Use this for initialization
    void Update ()
    {
        // Check for sound player
        if (sndPlyr == null)
        {
            Debug.Log("No Audio source Component!");
        }

        // Check for spaceBar
        if(Input.GetKeyDown(KeyCode.Space))
        {
            // Check if it was playing
            if(isPlaying)
            {
                // Stop playing
                isPlaying = false;
            }
            else 
            {
                // Start playing
                isPlaying = true;
                StartCoroutine(PlayMetroBeat(BPS));
            }
        }
	}

    IEnumerator PlayMetroBeat(float BPS)
    {
        //  converts BPs into the wait time(seconds per beat)
        float waitTime = (1 / BPS);
        while(isPlaying)
        {
            yield return new WaitForSeconds(waitTime);
            sndPlyr.PlayOneShot(sndPlyr.clip);
        }
    }
	
}

