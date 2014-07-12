using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class TextControll : MonoBehaviour {

    public Color alpha;
	public float time = 2.0f;
	private float dt = 0.0f;
	private bool showing = false;
    GUIText text;
	// Use this for initialization
	void Start () {
        text = GetComponent<GUIText>();
	}
	
	// Update is called once per frame
	void Update () {
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha.a);	

		if (showing) {
			dt += Time.deltaTime;
			/*if (dt >= time) {
				dt = 0.0f;
				showing = false;
				GetComponent<Animator>().SetBool("Triggered", false);
			}*/
		}
	}

	public void Show() {
		showing = true;
		GetComponent<Animator>().SetBool("Triggered", true);
	}

	public void Hide() {
		showing = false;
		GetComponent<Animator>().SetBool("Triggered", false);
	}

}
