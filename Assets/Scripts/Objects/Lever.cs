using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		if (!switched && triggered.GetBool("Triggered") == true) {
			triggered.SetBool("Triggered", false);
			GetComponentInChildren<Animator>().SetBool("Switched", false);
		} else if (switched && triggered.GetBool("Triggered") == false) {
			triggered.SetBool("Triggered", true);
			GetComponentInChildren<Animator>().SetBool("Switched", true);
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
		RoomManager manager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
		GameObject obj = manager.roomPrefabs.Find(a => (a.GetComponent<Room>().name == transform.parent.gameObject.GetComponent<Room>().name));
		List<Room> list = new List<Room>(GameObject.FindGameObjectWithTag("RoomsPull").GetComponentsInChildren<Room>());
		//Room room = list.Find(a => (a.name == obj.GetComponent<Room>().name));
		GameObject newRoom = (GameObject)(Instantiate(transform.parent.gameObject));
		newRoom.transform.parent = GameObject.FindGameObjectWithTag("RoomsPull").transform;
		newRoom.transform.position = new Vector3(-10000,-10000, -10000);
		newRoom.name = obj.GetComponent<Room>().name;
		foreach(EntryPoint p in newRoom.GetComponent<Room>().getAllEntryPoints()) {
			p.connectedEntry = null;
		}
		manager.roomPrefabs.Add(newRoom);
		manager.roomPrefabs.Remove(obj);
		Destroy(obj);
	}
}
