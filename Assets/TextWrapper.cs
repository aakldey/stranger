using UnityEngine;
using System.Collections;

public class TextWrapper : MonoBehaviour {

    public GUIText guiText;
    public Color color;
    public Color size;
    public bool selected = true;

	// Use this for initialization
	void Start () {

        guiText = GetComponent<GUIText>();
        guiText.material.color = color;
        //guiText.fontSize = (int)size.r*20;
	
	}
	
	// Update is called once per frame
	void Update () {

       

        guiText.material.color = color;

	
	}
}
