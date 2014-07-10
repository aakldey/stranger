using UnityEngine;
using System.Collections;

public class SquaresStart : MonoBehaviour {
	public SquareRotate square1;
	public SquareRotate square2;
	public SquareRotate square3;

	private bool flag = false;
	private float time = 0.0f;
	public float delay = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (flag) {
			if (time == 0.0f)
				square1.play = true;
			if (time >= delay)
				square2.play = true;
			if (time >= delay*2)
				square3.play = true;

			time += Time.deltaTime;

		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "PlayerTrigger") {
			flag = true;
		}
	}
}
