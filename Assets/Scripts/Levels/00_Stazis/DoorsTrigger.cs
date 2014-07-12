using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Stranger/DoorsTrigger")]
public class DoorsTrigger : MonoBehaviour {

	public List<Door> doors;

	public bool active = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			if (active) {
				foreach(Door door in doors) 
					door.opened = !door.opened;
				active = false;
			}
		}
	}
}
