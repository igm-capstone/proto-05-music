using UnityEngine;
using System.Collections;

public class AttackEvent : MonoBehaviour 
{
    private bool attack = false;
    private Transform enemy;
    private string key = null;
    private float damage = 1f;
    private GameObject star;
    private Vector3 playerPosBeforeAttack;
    public float hitDist, AttackSpeed;
    public AudioSource a1, a2, a3, a4;
	
    void Start()
    {
        star = GameObject.Find("Star");
    }
    void Update () 
    {
        AttackActive();
        if (attack == true && key != null)
            AttackMovePlayer(key);
	}

    private void AttackActive()
    {
        if (attack == true)
        {
            if (Input.GetButtonDown("A"))
            {
                a1.Play();
                HitEffects("A",Color.red);
            }
            if (Input.GetButtonDown("B"))
            {
                a2.Play();
                HitEffects("B", Color.yellow);
            }
            if (Input.GetButtonDown("X"))
            {
                a3.Play();
                HitEffects("X", Color.white);
            }
            if (Input.GetButtonDown("Y"))
            {
                a4.Play();
                HitEffects("Y", Color.blue);
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

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemy = other.gameObject.transform;
            attack = true;
            playerPosBeforeAttack = gameObject.transform.position;
        }
    }

    public void StopAttack()
    {
        attack = false;
        gameObject.transform.position = playerPosBeforeAttack;
    }
}
