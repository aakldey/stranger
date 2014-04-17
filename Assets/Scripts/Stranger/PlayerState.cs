using UnityEngine;
using System.Collections;

public enum State
{
	World1, World2, World3, All	
};

public class PlayerState : MonoBehaviour {

	public State CurrentState = State.World1;
	public bool AllowTransfer = false;
	public bool AllowRotation = false;
	
	public bool AllowToFirst = false;
	public bool AllowToSecond = false;
	public bool AllowToThird = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
