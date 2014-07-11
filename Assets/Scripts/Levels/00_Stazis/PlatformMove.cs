using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour {

	public Platform platform;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			platform.Move();
		}
	}
}
