using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]

public class CollectibleBehavior : MonoBehaviour
{
    public Collider2D   m_Collider;
    public AudioSource  m_audioSource;
    public bool         m_AnimatesPlayer;
    private ComposerBehaviour jukebox;

    public float lengthInBeats  = 1.0f;
    private bool m_isCollected   = false;
	void Start ()
    {
        m_audioSource   = GetComponent<AudioSource>();
        m_Collider      = GetComponent<Collider2D>();
        jukebox         = FindObjectOfType<ComposerBehaviour>();
	}

    public void PlayAudio(Collider2D player)
    {
        if (!m_isCollected)
        {
            Hide();

            Vector3 direction = Quaternion.AngleAxis(transform.rotation.z, Vector3.forward) * Vector3.right;

            Vector3 p0 = player.gameObject.transform.position;
            Vector3 p1 = p0 + direction.normalized * Vector3.Dot(m_Collider.bounds.extents, direction.normalized) * 2.0f;

            //Adding both regardlessly for now, may revisit later
            jukebox.AddSingleSound(this);
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
    }

    IEnumerator InterpolatePlayerPosition(GameObject player, Vector3 p0, Vector3 p1)
    {

        // Start Dash Animation
        Animator plyrAnim = player.GetComponentInChildren<Animator>();
        plyrAnim.SetTrigger("Dash");
        plyrAnim.SetBool("isDashing", true);

        m_audioSource.Play();
        while (m_audioSource.isPlaying)
        {
            float t = ((float)m_audioSource.timeSamples / (float)m_audioSource.clip.samples);
            //player.transform.position = Vector3.Lerp(p0, p1, (1 - Mathf.Exp(-10*t / 2)));
            player.transform.position = Vector3.Lerp(p0, p1, (1 - Mathf.Exp(-5 * t / 2)));
            yield return null;
        }
        // Ends Dashing Animation
        plyrAnim.SetBool("isDashing", false);
    }

    private void Hide()
    {
        // Comment out to play more than once.
        m_isCollected = true;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
