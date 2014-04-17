using UnityEngine;
using System.Collections;

public class MenuItemSelected : MonoBehaviour {

    public TextMesh textMesh;
    public Color color;

	// Use this for initialization
	void Start () {

        textMesh = GetComponent<TextMesh>();
        textMesh.color = color;
	}
	
	// Update is called once per frame
	void Update () {

        textMesh.color = color;
	
	}
}
