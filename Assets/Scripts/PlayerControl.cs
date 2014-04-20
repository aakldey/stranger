using UnityEngine;
using System.Collections;


public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.


	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.


	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	public Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.

    PlayerState playerState;
    public bool ladder = false;
    private string layerName;
    private GameObject player;

    public bool enableControl = true;

    void Start()
    {

        playerState = GetComponent<PlayerState>();
        layerName = playerState.CurrentState.ToString();
        player = GameObject.FindGameObjectWithTag("Player");
        //Debug.LogError(layerName);
    }
	

	void Awake()
	{
		// Setting up references.

		anim = GetComponent<Animator>();
	}


	void Update()
	{
        if (enableControl)
        {

            if (Input.GetButtonDown("Jump") && !ladder)
            {
                
                grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer(layerName));
                Debug.Log(grounded.ToString());

                if (grounded)
                    jump = true;
            }

            if (Input.GetButton("Use"))
            {
                //    Camera camera = GameObject.FindGameObjectWithTag("MainCamera").camera;
                //    FXPostProcess[] list = camera.GetComponents<FXPostProcess>();
                //    list[list.Length - 1].enabled = !list[list.Length - 1].enabled;
            }

            if (Input.GetButtonDown("Transfer") && GetComponent<PlayerState>().AllowTransfer && GetComponent<LevelRotation>().rotating == false)
            {
                if (playerState.AllowToFirst == true)
                {

                    if (playerState.CurrentState == State.World2)
                    {
                        Debug.Log("to 1");
                        ChangeStateToWorld1();
                        goto OutOf;
                    }


                }

                if (playerState.AllowToSecond == true)
                {


                    if (playerState.CurrentState == State.World1)
                    {
                        Debug.Log("to 2");
                        ChangeStateToWorld2();
                    }

                }

            OutOf:
                int a = 0;
            }

            if (Input.GetButtonDown("Use"))
            {

            }
        }
	
	}


	void FixedUpdate ()
	{
        if (enableControl)
        {

            // Cache the horizontal input.
            float h = Input.GetAxis("Horizontal");
            float l = Input.GetAxis("Vertical");

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            //anim.SetFloat("Speed", Mathf.Abs(h));

            // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...

            if (ladder)
            {
                rigidbody2D.gravityScale = 0;
                rigidbody2D.velocity = new Vector2(0, 0);
            }
            else
            {
                rigidbody2D.gravityScale = 1;

            }
            // ... add a force to the player.
            switch (GetComponent<LevelRotation>().currentRotationState)
            {
                case RotationState.left_0:
                    if (h * rigidbody2D.velocity.x < maxSpeed)
                        rigidbody2D.AddForce(Vector2.right * h * moveForce);

                    if (ladder && l * rigidbody2D.velocity.y < maxSpeed * 2)
                        rigidbody2D.AddForce(Vector2.up * l * moveForce);

                    break;
                case RotationState.left_180:
                    if (-h * rigidbody2D.velocity.x < maxSpeed)
                        rigidbody2D.AddForce(-Vector2.right * h * moveForce);

                    if (ladder && l * rigidbody2D.velocity.y < maxSpeed * 2)
                        rigidbody2D.AddForce(-Vector2.up * l * moveForce);
                    break;
                case RotationState.left_90:
                    if (h * rigidbody2D.velocity.y < maxSpeed)
                        rigidbody2D.AddForce(Vector2.up * h * moveForce);

                    if (ladder && l * rigidbody2D.velocity.y < maxSpeed * 2)
                        rigidbody2D.AddForce(-Vector2.right * l * moveForce);
                    break;
                case RotationState.left_270:
                    if (-h * rigidbody2D.velocity.y < maxSpeed)
                        rigidbody2D.AddForce(-Vector2.up * h * moveForce);

                    if (ladder && l * rigidbody2D.velocity.y < maxSpeed * 2)
                        rigidbody2D.AddForce(Vector2.right * l * moveForce);
                    break;

            }



            // If the player's horizontal velocity is greater than the maxSpeed...
            switch (GetComponent<LevelRotation>().currentRotationState)
            {
                case RotationState.left_0:
                    if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
                        // ... set the player's velocity to the maxSpeed in the x axis.
                        rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
                    break;
                case RotationState.left_180:
                    if (Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
                        // ... set the player's velocity to the maxSpeed in the x axis.
                        rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
                    break;
                case RotationState.left_90:
                    if (Mathf.Abs(rigidbody2D.velocity.y) > maxSpeed)
                        // ... set the player's velocity to the maxSpeed in the x axis.
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed);
                    break;
                case RotationState.left_270:
                    if (Mathf.Abs(rigidbody2D.velocity.y) > maxSpeed)
                        // ... set the player's velocity to the maxSpeed in the x axis.
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, Mathf.Sign(rigidbody2D.velocity.y) * maxSpeed);
                    break;
            }

            if (ladder)
            {

            }

            // If the input is moving the player right and the player is facing left...
            if (h > 0 && !facingRight)
                // ... flip the player.
                Flip();
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (h < 0 && facingRight)
                // ... flip the player.
                Flip();

            // If the player should jump...
            if (jump && !ladder)
            {
                // Set the Jump animator trigger parameter.
               // anim.SetTrigger("Jump");

                // Play a random jump audio clip.
                int i = Random.Range(0, jumpClips.Length);
                //AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

                // Add a vertical force to the player.
                switch (GetComponent<LevelRotation>().currentRotationState)
                {
                    case RotationState.left_0:
                        rigidbody2D.AddForce(new Vector2(0f, jumpForce));
                        break;
                    case RotationState.left_180:
                        rigidbody2D.AddForce(new Vector2(0f, -jumpForce));
                        break;
                    case RotationState.left_90:
                        rigidbody2D.AddForce(new Vector2(-jumpForce, 0f));
                        break;
                    case RotationState.left_270:
                        rigidbody2D.AddForce(new Vector2(jumpForce, 0f));
                        break;
                }

                // Make sure the player can't jump again until the jump conditions from Update are satisfied.
                jump = false;
            }



            float transfer = Input.GetAxis("Transfer");
        }

       }
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


    public void ChangeStateToWorld1()
    {
        playerState.CurrentState = State.World1;
        player.layer = LayerMask.NameToLayer("Player1");

        Transform[] objects = GameObject.FindGameObjectWithTag("AllWorlds").GetComponentsInChildren<Transform>();

        foreach (Transform t in objects)
        {
            t.gameObject.layer = LayerMask.NameToLayer("AllWorlds1"); 
        }

        GameObject.FindGameObjectWithTag("PlayerTrigger").layer = LayerMask.NameToLayer("Player1");
        Camera.main.backgroundColor = new Color(255.0f/255, 255.0f/255, 255.0f/255);

        Rigidbody2D[] bodies = GameObject.FindGameObjectWithTag("AllWorlds").GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D b in bodies)
        {
            b.AddForce(new Vector2(0.001f, 0.0001f));
        }


    }

    public void ChangeStateToWorld2()
    {
        Transform[] objects = GameObject.FindGameObjectWithTag("AllWorlds").GetComponentsInChildren<Transform>();

        foreach (Transform t in objects)
        {
            t.gameObject.layer = LayerMask.NameToLayer("AllWorlds2");
        }

        Rigidbody2D[] bodies = GameObject.FindGameObjectWithTag("AllWorlds").GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D b in bodies)
        {
            b.AddForce(new Vector2(0.001f, 0.0001f));
        }

        Camera.main.backgroundColor = new Color(255.0f/255, 255.0f/255, 100.0f/255);
        playerState.CurrentState = State.World2;
        player.layer = LayerMask.NameToLayer("Player2");
        GameObject.FindGameObjectWithTag("PlayerTrigger").layer = LayerMask.NameToLayer("Player2"); 

    }




}
