using UnityEngine;
using System.Collections;

public class ToSecondTrigger : MonoBehaviour {

    BoxCollider2D collider;

	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.GetType());
        if (col.gameObject.tag == "PlayerTrigger")
        {
            Debug.Log("Enter");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().AllowToSecond = true;
           // Debug.Log(col.GetType());
           // col.gameObject.GetComponent<Rigidbody2D>().gravityScale = -1;
        }
    }

    void OnTriggerExit2D(Collider2D col )
    {
        if (col.gameObject.tag == "PlayerTrigger")
        {
            Debug.Log("Left");
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().AllowToSecond = false;
          //  Debug.Log(col.GetType());
         //   col.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
