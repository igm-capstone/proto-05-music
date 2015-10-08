using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
    [SerializeField]
    private float rotateSpeed, moveSpeed;
    public Transform startPos, endPos;
    public float health;
	
    
    void FixedUpdate () 
    {
        transform.Rotate(new Vector3(0, 0, 1) * rotateSpeed);
        //gameObject.GetComponent<Rigidbody>().MovePosition(new Vector3(Mathf.Lerp(startPos.position.x, endPos.position.x, moveSpeed), transform.position.y, transform.position.z));        
	}

    void Update()
    {
        if (health <= 0)
        {
            GameObject.FindWithTag("Player").GetComponent<AttackEvent>().StopAttack();
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        Debug.Log(health);
    }

    public void HitEnemy(float damage)
    {
        if(health > 0)
            health -= damage;        
    }
}
