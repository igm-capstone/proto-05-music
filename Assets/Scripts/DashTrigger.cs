﻿using UnityEngine;
using System.Collections;



public class DashTrigger : MonoBehaviour {
    private AudioSource m_audioSource;
    private Transform start;
    private Transform end;

    public bool isActive { get; private set; }

    public AnimationCurve curve;

	void Awake ()
	{
	    m_audioSource = GetComponent<AudioSource>();
	    start = transform.Find("Start");
	    end = transform.Find("End");
	    isActive = true;
	}
	
	// Update is called once per frame
    public void StartDash(Collider2D player)
    {
        if (!isActive)
            return;

        //Vector3 direction = Quaternion.AngleAxis(transform.rotation.z, Vector3.forward) * Vector3.right;

        Vector3 direction = Quaternion.AngleAxis(transform.rotation.z, Vector3.forward) * Vector3.right;

        //Adding both regardlessly for now, may revisit later
        //StartCoroutine(InterpolatePlayerPosition(player.GetComponent<PlayerController>(), start.position, end.position));

        player.GetComponent<PlayerController>().DashTo(end.position, m_audioSource.clip.length, curve);

    }

    void Collected()
    {
        isActive = false;
    }

    IEnumerator InterpolatePlayerPosition(PlayerController player, Vector3 p0, Vector3 p1)
    {

        // start animation next frame..
        yield return null;

        Debug.Log("dash");
        while (m_audioSource.isPlaying)
        {
            float t = ((float)m_audioSource.timeSamples / (float)m_audioSource.clip.samples);
            //player.transform.position = Vector3.Lerp(p0, p1, (1 - Mathf.Exp(-10*t / 2)));
            //player.transform.position = Vector3.Lerp(p0, p1, (1 - Mathf.Exp(-5 * t / 2)));
            player.transform.position = Vector3.Lerp(p0, p1, curve.Evaluate(t));


            yield return null;
        }

        // Ends Dashing Animation
        
    }
}
