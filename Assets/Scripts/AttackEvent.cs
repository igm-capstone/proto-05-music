using UnityEngine;
using System.Collections;

public class AttackEvent : MonoBehaviour 
{
    private bool attack = false;
    private string key = null;
    private float origGravity;
    private Vector3 playerPosBeforeAttack;
    
    private Transform enemy;
    private GameObject star;

    public float hitDist, AttackSpeed, damage;
    public AudioSource a1, a2, a3, a4;

    private ComposerBehaviour jukebox;
    private GameObject Arch, Audio;
    private GameObject Cam;

    private Animator MyAnim;

    void Start() {
        jukebox = FindObjectOfType<ComposerBehaviour>();
        Arch = GameObject.Find("Architecture");
        Audio = GameObject.Find("Audio");
        Cam = GameObject.Find("Main Camera");
        MyAnim = GetComponentInChildren<Animator>();
    }

    void Update () 
    {
        AttackActive();
        if (attack == true && key != null)
        {
            AttackMovePlayer(key);
        }
	}

    private void AttackActive()
    {
        if (attack == true)
        {
            if (Input.GetButtonDown("A") || Input.GetKeyDown(KeyCode.UpArrow))
            {
                a1.Play();
                jukebox.AddToSoloRecording(a1);
                HitEffects("A",Color.red);
                MyAnim.SetTrigger("HeavyAttack");
            }
            if (Input.GetButtonDown("B") || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                a2.Play();
                jukebox.AddToSoloRecording(a2);
                HitEffects("B", Color.yellow);
                MyAnim.SetTrigger("LightAttack");
            }
            if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.DownArrow))
            {
                a3.Play();
                jukebox.AddToSoloRecording(a3);
                HitEffects("X", Color.white);
                MyAnim.SetTrigger("HeavyAttack");
            }
            if (Input.GetButtonDown("Y") || Input.GetKeyDown(KeyCode.RightArrow))
            {
                a4.Play();
                jukebox.AddToSoloRecording(a4);
                HitEffects("Y", Color.blue);
                MyAnim.SetTrigger("LightAttack");
            }
        }
        else
            key = null;
    }

    private void HitEffects(string button, Color starColor)
    {
        key = button;
        star.GetComponent<SpriteRenderer>().color = starColor;
        GameObject.FindWithTag("Enemy").GetComponent<Enemy>().HitEnemy(damage);
        star.GetComponent<Animator>().SetTrigger("Start");
        star.transform.localScale = new Vector3(Mathf.Lerp(1, 90, 1), Mathf.Lerp(1, 90, 1), Mathf.Lerp(1, 90, 1));
        StartCoroutine("StarWait", 0.1f);
    }

    IEnumerator StarWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        star.transform.localScale = new Vector3(Mathf.Lerp(90, 1, 1), Mathf.Lerp(90, 1, 1), Mathf.Lerp(90, 1, 1));
    }

    private void AttackMovePlayer(string button)
    {
        Vector3 destination = gameObject.transform.position;
        if (button == "A")
            destination = new Vector3(enemy.position.x, enemy.position.y - hitDist, transform.position.z);
        if (button == "B")
            destination = new Vector3(enemy.position.x + hitDist, enemy.position.y, transform.position.z);
        if (button == "X")
            destination = new Vector3(enemy.position.x - hitDist, enemy.position.y, transform.position.z);
        if (button == "Y")
            destination = new Vector3(enemy.position.x, enemy.position.y + hitDist, transform.position.z);

        transform.position = Vector3.Lerp(gameObject.transform.position,destination, AttackSpeed);
    }

    public void TriggerEvent(Collider2D other)
    {
        ChangeEnvironment(true);
        GetComponent<PlayerController>().eventCheck = true;
        jukebox.StartSoloRecording();
        other.GetComponent<Collider2D>().enabled = false;
        origGravity = GetComponent<PlayerController>().Gravity;
        GetComponent<PlayerController>().Gravity = 0;
        enemy = other.gameObject.transform;
        star = enemy.FindChild("Star").gameObject;
        attack = true;
        playerPosBeforeAttack = gameObject.transform.position;
    }

    public void StopAttack()
    {
        ChangeEnvironment(false);
        attack = false;
        GetComponent<PlayerController>().eventCheck = false;
        jukebox.StopSoloRecording();
        gameObject.transform.position = playerPosBeforeAttack;
        GetComponent<PlayerController>().Gravity = origGravity;
    }

    private void ChangeEnvironment(bool eventCheck)
    {
        if (eventCheck)
        {
            Arch.SetActive(false);
            Audio.SetActive(false);
            Cam.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        }
        else
        {
            Arch.SetActive(true);
            Audio.SetActive(true);
            Cam.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        }
    }
}
