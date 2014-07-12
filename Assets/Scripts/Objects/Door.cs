using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
	public bool opened = false;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator>();
		anim.SetBool("Triggered", opened);

	}
	
	// Update is called once per frame
	void Update () {
		if (opened != anim.GetBool("Triggered")) {
			anim.SetBool("Triggered", opened);
			audio.Play();
		}
	}
}
