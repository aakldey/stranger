using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RotationState
{
	left_0, left_90, left_180, left_270
}

public class LevelRotation : MonoBehaviour
{
    GameObject player;
	bool rot = false;
	float last_rotation = 0;
	List<RotationState> rotationState;
	public RotationState currentRotationState;
	int index = 0;
	Camera camera;
	public float time = 1;
    private float time2;
    public float double_rotation_time_k = 1.5f;
	float dt = 0;
	public bool rotating = false;
	float angle = 0;
	Quaternion lastquat;
	
	
	
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		rotationState = new List<RotationState>();
		rotationState.Add(RotationState.left_0);
		rotationState.Add(RotationState.left_90);
		rotationState.Add(RotationState.left_180);
		rotationState.Add(RotationState.left_270);
		
		currentRotationState = rotationState[0];
		camera = Camera.main;
        time2 = time * double_rotation_time_k;
       
		
	}
	
	public void ResetRotation()
	{
		index = 0;
		currentRotationState = RotationState.left_0;
		camera.transform.rotation = new Quaternion(0,0,0,0);
		//player.transform.rotation=camera.transform.rotation.eulerAngles.z*Mathf.PI/180;
        player.transform.rotation = camera.transform.rotation;
		Physics2D.gravity = new Vector2(0,-30);
	}
	
	// Update is called once per frame
	void Update ()
	{
        player.transform.rotation = camera.transform.rotation;
       // player.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
		if (rotating)
		{
			camera.transform.Rotate(new Vector3(0,0,1),angle/(time/Time.deltaTime));
            player.transform.rotation = camera.transform.rotation;
			dt+=Time.deltaTime;
			if (dt > time)
			{
				rotating = false; 
				dt = 0;
				camera.transform.rotation = lastquat*Quaternion.AngleAxis(angle,new Vector3(0,0,1));

                player.transform.rotation = camera.transform.rotation;
                time = time2 / double_rotation_time_k;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.A) && !rotating && GetComponent<PlayerState>().AllowRotation && GetComponent<PlayerState>().CurrentState == State.World2) // rotate left
		{
			index-=1;
			if (index == -1)
				index = 3;
			currentRotationState = rotationState[index];
			rotating = true;
			lastquat = camera.transform.rotation;
			angle = -90;
			
			switch (currentRotationState)
			{
			case RotationState.left_0:
                    Physics2D.gravity = new Vector2(0, -30);
				break;
			case RotationState.left_90:
                Physics2D.gravity = new Vector2(30, 0);
				break;
			case RotationState.left_180:
                Physics2D.gravity = new Vector2(0, 30);
				break;
			case RotationState.left_270:
                Physics2D.gravity = new Vector2(-30, 0);
				break;
					
			}
			
			//player.Rect.Rotation-=Mathf.PI/2;
			
			//foreach (Body b in FSWorldComponent.PhysicsWorld.BodyList)
			//{
			//	b.Awake = true;
		//	}
		}
		if (Input.GetKeyDown (KeyCode.D) &&!rotating && GetComponent<PlayerState>().AllowRotation && GetComponent<PlayerState>().CurrentState == State.World2) // rotate left
		{
			index+=1;
			if (index == 4)
				index = 0;
			currentRotationState = rotationState[index];
			rotating = true;
			angle = 90;
			lastquat = camera.transform.rotation;
			switch (currentRotationState)
			{
			case RotationState.left_0:
                    Physics2D.gravity = new Vector2(0, -30);
				break;
			case RotationState.left_90:
                Physics2D.gravity = new Vector2(30, 0);
				break;
			case RotationState.left_180:
                Physics2D.gravity = new Vector2(0, 30);
				break;
			case RotationState.left_270:
                Physics2D.gravity = new Vector2(-30, 0);
				break;
					
			}
			//player.Rect.Rotation+=Mathf.PI/2;
			
		//	foreach (Body b in FSWorldComponent.PhysicsWorld.BodyList)
			//{
		//		b.Awake = true;
		//	}
		}

        if (Input.GetKeyDown(KeyCode.W) && !rotating && GetComponent<PlayerState>().AllowRotation && GetComponent<PlayerState>().CurrentState == State.World2) // rotate left
        {
            index += 2;
            if (index == 4)
                index = 0;
            if (index == 5)
                index = 1;

            time = time2;

            currentRotationState = rotationState[index];
            rotating = true;
            angle = 180;
            lastquat = camera.transform.rotation;
            switch (currentRotationState)
            {
                case RotationState.left_0:
                    Physics2D.gravity = new Vector2(0, -30);
                    break;
                case RotationState.left_90:
                    Physics2D.gravity = new Vector2(30, 0);
                    break;
                case RotationState.left_180:
                    Physics2D.gravity = new Vector2(0, 30);
                    break;
                case RotationState.left_270:
                    Physics2D.gravity = new Vector2(-30, 0);
                    break;

            }
            //player.Rect.Rotation+=Mathf.PI/2;

            //	foreach (Body b in FSWorldComponent.PhysicsWorld.BodyList)
            //{
            //		b.Awake = true;
            //	}
        }
		
		if (Input.GetKeyDown (KeyCode.S) && rotating == false) 
		{
			ResetRotation();	
		
		}
		
		
		
	}
}
