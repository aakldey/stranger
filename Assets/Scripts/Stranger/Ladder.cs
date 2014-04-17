using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerTrigger")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().ladder = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "PlayerTrigger")
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>().ladder = false;
        }
    }
}
