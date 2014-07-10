using UnityEngine;
using System.Collections;

public class SquareRotate : MonoBehaviour {
	public float rotationSpeed = 0.5f;

	public bool play = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (play) {
			transform.Rotate(new Vector3(0,0,rotationSpeed));
		}
	}
}
