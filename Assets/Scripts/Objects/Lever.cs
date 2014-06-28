using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Lever : MonoBehaviour {

	public bool switched = false; // init pos - left
	public bool busy = false;
	public Animator triggered;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponentInChildren<Animator>().IsInTransition(0)) {
			busy = true;
		} else {
			busy = false;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().lever = this.gameObject;
			Debug.Log("Enter");

		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().lever = null;
			Debug.Log("Left");
		}
	}

	public void Switch() {
		switched = !switched;
		GetComponentInChildren<Animator>().SetBool("Switched", switched);
		triggered.SetBool("Triggered", !triggered.GetBool("Triggered"));
	}
}
