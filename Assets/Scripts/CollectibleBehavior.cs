using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]
[ExecuteInEditMode]
public class CollectibleBehavior : MonoBehaviour
{
    public Collider2D   m_Collider;
    public AudioSource  m_audioSource;
    private ComposerBehaviour jukebox;
    public float        m_playerSpeed;
    private Vector3     m_DebugRay;

    public float lengthInBeats  = 1.0f;
    private bool m_isCollected   = false;
	void Start ()
    {
        m_audioSource   = GetComponent<AudioSource>();
        m_Collider      = GetComponent<Collider2D>();
        jukebox         = FindObjectOfType<ComposerBehaviour>();
        m_playerSpeed   = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RunSpeed;
        m_DebugRay      = (transform.right * m_playerSpeed * m_audioSource.clip.length);
    }

    void OnDrawGizmos()
    {
        Vector3 p = transform.position + m_DebugRay;
        Debug.DrawLine(transform.position, p);

        Handles.Label(p, "Data: x:"+ p.x + "y:" + p.y);
    }

    public void PlayAudio(Collider2D player)
    {
        if (!m_isCollected)
        {
            Hide();

            //Adding both regardlessly for now, may revisit later
            jukebox.AddSingleSound(this);

            m_audioSource.Play();
        }
    }

    private void Hide()
    {
        // Comment out to play more than once.
        m_isCollected = true;
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(InformCollected());
    }

    IEnumerator InformCollected()
    {
        yield return null;
        SendMessage("Collected", SendMessageOptions.DontRequireReceiver);
    }
}
