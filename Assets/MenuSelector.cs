using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSelector : MonoBehaviour {
    public int selectedItem = 0;
    private List<GUIText> items = new List<GUIText>();
    public int standartSize = 16;
    public int selectedSize = 20;
    bool flag = false;
	// Use this for initialization
	void Start () {

        items.Add(GameObject.FindGameObjectWithTag("NewGame").GetComponent<GUIText>());
        items.Add(GameObject.FindGameObjectWithTag("Continue").GetComponent<GUIText>());
        items.Add(GameObject.FindGameObjectWithTag("About").GetComponent<GUIText>());
        items.Add(GameObject.FindGameObjectWithTag("Exit").GetComponent<GUIText>());
        items[selectedItem%4].fontSize = selectedSize;
	}
	
	// Update is called once per frame
	void Update () {

        if ((Input.GetAxis("Vertical2") < 0) && !flag)
        {
            flag = true;
            items[selectedItem % 4].fontSize = standartSize;
            selectedItem++;
            items[selectedItem % 4].fontSize = selectedSize;
        }

        if (Input.GetAxis("Vertical2") > 0  && !flag)
        {
            flag = true;
            items[selectedItem % 4].fontSize = standartSize;
            selectedItem--;
            if (selectedItem == -1)
                selectedItem = 3;
            items[selectedItem % 4].fontSize = selectedSize;
        }

        if (Input.GetAxis("Vertical2") == 0)
        {
            flag = false;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectedItem == 3)
            {
                Application.Quit();
            }

            if (selectedItem == 0)
            {
                Application.LoadLevel(1);
            }
        }
	
	}
}
