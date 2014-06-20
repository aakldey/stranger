using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {

	public int seenRooms = 1;
	public int ROOM_SIZE = 16;
	private GameObject world1;

	// Use this for initialization
	void Start () {

		world1 = GameObject.FindGameObjectWithTag("World1");

		for (int i = -seenRooms; i <= seenRooms; i++) {
			Vector3 position = new Vector3(i*ROOM_SIZE, 0, 0 );
			Instantiate(Resources.Load("Room1"), position, Quaternion.identity);
			Debug.Log("Instanciating");
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
