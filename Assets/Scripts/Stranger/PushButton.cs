using UnityEngine;
using System.Collections;

public class PushButton : MonoBehaviour 
{
    public Animator anim;
    public Animator triggered;

    private bool pushed = false;
    private bool flag = true;
    private int num = 0;

	// Use this for initialization
	void Start () 
    {
        //anim = GameObject.FindGameObjectWithTag("PushButton").GetComponent<Animator>();
        //anim.Play("push_button_iddle");

	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Crate" || col.tag == "PlayerTrigger")
            num++;

        if (num > 0)
        {
            anim.SetBool("Pushed", true);
            triggered.SetBool("Triggered", true);
        }
        //pushed = true;
       // flag = true;
        Debug.Log("Enter: "+num.ToString());
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Crate" || col.tag == "PlayerTrigger")
            num--;

        if (num == 0)
        {
            anim.SetBool("Pushed", false);
            triggered.SetBool("Triggered", false);
        }
            //anim.Play("push_button_up");
        //pushed = true;
        // flag = true;
        Debug.Log("Left: " + num.ToString());
    }
}
