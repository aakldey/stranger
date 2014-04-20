using UnityEngine;
using System.Collections;

public class WhitePoint : MonoBehaviour {
    int num = 0;
    bool done = false;

    public GameObject text1;
    public GameObject text2;

    public GameObject playerSprite;
    public GameObject player;

    public int innactiveTime = 2;

    bool flag = false;
    float time = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!done)
        {
            if (time < innactiveTime)
            {
                time += Time.deltaTime;
            }
            else
            {
                flag = true;
            }

            if (Input.anyKeyDown && flag)
            {
                if (num == 0)
                {
                    GetComponent<Animator>().SetBool("Trigger", true);
                    num++;
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num - 1].Stop();
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num].volume = 1;
                    text1.GetComponent<Animator>().SetBool("Trigger", false);
                    flag = false;
                    time = 0;

                }
                else if (num == 1)
                {
                    text2.GetComponent<Animator>().SetBool("Trigger", true);
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num - 1].volume = 0;
                    GameObject.FindGameObjectWithTag("Sounds").GetComponent<MasterBus>().list[num].volume = 1;
                   // done = true;

                    playerSprite.GetComponent<Animator>().SetBool("Trigger", true);

                    flag = false;
                    time = 0;

                    Camera.main.GetComponent<Animator>().SetBool("Trigger", true);
                    num++;

                }
                else if (num == 2)
                {
                    player.GetComponent<CharacterController>().enableControl = true;
                    player.GetComponent<Rigidbody2D>().gravityScale = 1;
                    Camera.main.GetComponent<Animator>().SetBool("Trigger", false);
                    flag = false;
                    num++;
                }
            }
        }

	
	}
}
