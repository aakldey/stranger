using UnityEngine;
using System.Collections;

public class ActivateDoorsTrigger : MonoBehaviour {
	public DoorsTrigger trigger;
	public bool active = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger" && active) {
			trigger.active = true;
			active = false;
		}
	}
}
