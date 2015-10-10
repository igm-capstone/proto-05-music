using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]

public class CollectibleBehavior : MonoBehaviour
{
    public Collider2D m_Collider;
    public AudioSource  m_audioSource;
    public bool         m_AnimatesPlayer;
    private ComposerBehaviour jukebox;

	void Start ()
    {
        m_audioSource   = GetComponent<AudioSource>();
        m_Collider      = GetComponent<Collider2D>();
        jukebox         = FindObjectOfType<ComposerBehaviour>();
	}

    public void PlayAudio(Collider2D player)
    {        
        Vector3 direction = Quaternion.AngleAxis(transform.rotation.z, Vector3.forward) * Vector3.right;
        Vector3 p0 = player.gameObject.transform.position;
        Vector3 p1 = p0 + direction.normalized * Vector3.Dot(m_Collider.bounds.extents, direction.normalized) * 2.0f;

        //Adding both regardlessly for now, may revisit later
        jukebox.AddSingleSound(m_audioSource);
        jukebox.AddToSoloRecording(m_audioSource);

        if (m_AnimatesPlayer)
        {
            StartCoroutine(InterpolatePlayerPosition(player.gameObject, p0, p1));
        }
        else
        {
            m_audioSource.Play();
        }
    }

    IEnumerator InterpolatePlayerPosition(GameObject player, Vector3 p0, Vector3 p1)
    {
        m_audioSource.Play();
        while (m_audioSource.isPlaying)
        {
            float t = ((float)m_audioSource.timeSamples / (float)m_audioSource.clip.samples);
            player.transform.position = Vector3.Lerp(p0, p1, (1 - Mathf.Exp(-10*t / 2)));
            yield return null;
        }
    }
}
