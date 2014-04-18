using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class TextControll : MonoBehaviour {

    public Color alpha;
    GUIText text;
	// Use this for initialization
	void Start () {
        text = GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha.a);	
	}
}
