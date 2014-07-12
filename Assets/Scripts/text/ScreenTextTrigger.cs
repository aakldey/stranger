using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenTextTrigger : MonoBehaviour {
	public string text;

	private float dt = 0.0f;

	public float startDelay = 2.0f;

	public List<ScreenTextTrigger> activating;

	public int size = 24;

	public float showTime = 2.0f;

	public bool active = true;

	private bool started = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (started) {
			dt+=Time.deltaTime;
			if (dt >= startDelay) {
				ScreenText.textControll.Show();
				started = false;
				if (activating != null) {
					foreach(ScreenTextTrigger trigger in activating) {
						trigger.active = true;
					}
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			if (active) {
				started = true;
				ScreenText.text.text = text;
				ScreenText.textControll.time = showTime;
				ScreenText.text.fontSize = size;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			if (active) {
				started = false;
				ScreenText.textControll.Hide();

				active = false;
			}
		}
	}
}
