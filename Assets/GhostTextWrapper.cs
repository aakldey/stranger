using UnityEngine;
using System.Collections;

public class GhostTextWrapper : MonoBehaviour {

    public TextMesh text;
    public Color color;
	// Use this for initialization
	void Start () {

        text = GetComponent<TextMesh>();
        text.color = color;
	
	}
	
	// Update is called once per frame
	void Update () {

        text.color = color;
	
	}
}
