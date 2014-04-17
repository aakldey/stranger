using UnityEngine;
using System.Collections;

public class HeroTrigger : MonoBehaviour {


    private int num1 = 0;
    private int num2 = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.GetType());
        if (col.gameObject.tag == "ToFirst")
        {
            Debug.Log("Enter");
            
            if (num1 == 0)
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().AllowToFirst = true;

            num1++;
        }

        if (col.gameObject.tag == "ToSecond")
        {
            
            Debug.Log("Enter");
            if (num2 == 0)
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().AllowToSecond = true;
            num2++;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "ToFirst")
        {
            num1--;
            Debug.Log("Enter");
            if (num1 == 0)
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().AllowToFirst = false;
        }

        if (col.gameObject.tag == "ToSecond")
        {
            num2--;
            Debug.Log("Enter");
            if (num2 == 0) 
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerState>().AllowToSecond = false;
        }
    }
}
