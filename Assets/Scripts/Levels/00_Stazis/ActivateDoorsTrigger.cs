using UnityEngine;
using System.Collections;

public class ActivateDoorsTrigger : MonoBehaviour {
	public DoorsTrigger trigger;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			trigger.active = true;
		}
	}
}
