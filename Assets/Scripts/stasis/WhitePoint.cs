using UnityEngine;
using System.Collections;

public class WhitePoint : MonoBehaviour {
    int num = 0;
    bool done = false;

    public GameObject text1;
    public GameObject text2;

    public GameObject playerSprite;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!done)
        {
            
            if (Input.anyKeyDown)
            {
                if (num == 0)
                {
                    GetComponent<Animator>().SetBool("Trigger", true);
                    num++;
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num - 1].Stop();
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num].volume = 1;
                    text1.GetComponent<Animator>().SetBool("Trigger", false);

                }
                else if (num == 1)
                {
                    text2.GetComponent<Animator>().SetBool("Trigger", true);
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num - 1].volume = 0;
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num].volume = 1;
                    done = true;

                    playerSprite.GetComponent<Animator>().SetBool("Trigger", true);
                    
                }
            }
        }

	
	}
}
