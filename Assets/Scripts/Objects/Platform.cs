using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {
	public GameObject targetA;
	public GameObject targetB;

	public bool fromAtoB = false;

	public bool looped = false;

	private bool moving = false;

	public float speed = 0.9f;
	public float delay = 0.0f;

	private float time  = 0.0f;

	private float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		Debug.Log("Start time: "+startTime);
	}
	
	// Update is called once per frame
	void Update () {


		if (moving) {
			float length = (targetA.transform.position-targetB.transform.position).magnitude;
			float weight = Vector3.Dot(targetB.transform.position - targetA.transform.position,transform.position - targetA.transform.position) / (length*length);
			//Debug.Log(""+weight);
			if(weight >=1.01f) {
				if(fromAtoB) {
					moving = false;
					GameObject tmp = targetA;
					targetA = targetB;
					targetB = tmp;
					rigidbody2D.velocity = Vector2.zero;
					audio.Stop();
					Debug.Log ("Stop moving mypos = "+ Mathf.Abs(transform.localPosition.magnitude)+" targetPos = "+Mathf.Abs(targetB.transform.localPosition.magnitude));
				} else if (looped) {
					moving = false;
					GameObject tmp = targetA;
					targetA = targetB;
					targetB = tmp;
					rigidbody2D.velocity = Vector2.zero;
					Move ();

				}
				GameObject.FindGameObjectWithTag("Player").rigidbody2D.velocity=Vector2.zero;
			} 
		}

	}

	public void Move() {
		startTime = Time.time - startTime;

		Vector2 a = new Vector2(targetA.transform.localPosition.x, targetA.transform.localPosition.y);
		Vector2 b = new Vector2(targetB.transform.localPosition.x, targetB.transform.localPosition.y);
		Vector2 dir = b - a;
		dir.Normalize();
		rigidbody2D.velocity = dir*speed;
		Debug.Log("Move! vel= "+rigidbody2D.velocity);
		moving = true;
		audio.Play();
	}
}
