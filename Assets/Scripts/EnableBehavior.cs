using UnityEngine;
using System.Collections;
using Prime31;

public class EnableBehavior : MonoBehaviour {

    public GameObject ObjectToEnable;

	// Use this for initialization
	void Start () {
        ObjectToEnable.SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController2D>().onTriggerExitEvent += TriggerExit;
    }
	
	// Update is called once per frame
	public void EnableObject()
    {
        ObjectToEnable.SetActive(true);
    }

    void TriggerExit(Collider2D other)
    {
        if (other.gameObject.transform == transform)
        {
            EnableObject();
        }
    }
}
