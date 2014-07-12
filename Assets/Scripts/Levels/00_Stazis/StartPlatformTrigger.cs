using UnityEngine;
using System.Collections;

public class StartPlatformTrigger : MonoBehaviour {

	public float delay = 2.0f;
	public Platform platform;

	private float time = 0.0f;
	private bool started = false;
	public bool active = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(started) {
			time+=Time.deltaTime;
			if(time >= delay) {
				started = false;
				platform.Move();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			if (active) {
				started = true;
				active = false;
				Debug.Log("START PLATFORM");
			}
		}
	}
}
