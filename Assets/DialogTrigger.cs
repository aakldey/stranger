using UnityEngine;
using System.Collections;

public class DialogTrigger : MonoBehaviour {

    public GameObject ghostText;
    int num;
    Animator anim;
	// Use this for initialization
	void Start () {

        anim = ghostText.GetComponent<Animator>();
       // anim.SetBool("Trigger", true);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerTrigger")
            num++;

        if (num > 0)
        {
            anim.SetBool("Trigger", true);
         //   triggered.SetBool("Triggered", true);
        }
        //pushed = true;
        // flag = true;
        Debug.Log("Enter: " + num.ToString());
    }
}
