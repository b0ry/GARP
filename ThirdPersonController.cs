using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {
	public Camera myCamera;
	private enum CharacterState {
		Idle = 0,
		Walking = 1,
		Trotting = 2,
		Running = 3,
	}

	public float runTimer = 5.0f;
	public float coolTimer = 5.0f;
	public float oldCooldown = 5.0f;
	public float cooldown = 5.0f;
	public bool flag = false;
	private CharacterState _characterState;

	public float walkSpeed = 2.0f; // The speed when walking
	public float trotSpeed = 4.0f; // after trotAfterSeconds of walking we trot with trotSpeed
	public float runSpeed = 6.0f; // when pressing "Fire3" button (shift) we start running

	public float inAirControlAcceleration = 3.0f;
	public float jumpHeight = 0.5f;// How high do we jump when pressing jump and letting go immediately
	
	public float gravity = 20.0f; // The gravity for the character
	public float speedSmoothing = 10.0f; // The gravity in controlled descent mode
	public float rotateSpeed = 500.0f;

	public float trotAfterSeconds = 3.0f;

	public bool canJump = true;

	private float jumpRepeatTime = 0.05f;
	private float jumpTimeout = 0.15f;
	private float groundedTimeout = 0.25f;

	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.0f; // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.


	private Vector3 moveDirection = Vector3.zero; // The current move direction in x-z
	private float verticalSpeed = 0.0f; 	// The current vertical speed
	private float moveSpeed = 0.0f; 	// The current x-z move speed

	private CollisionFlags collisionFlags; 	// The last collision flags returned from controller.Move

	private bool jumping = false;	// Are we jumping? (Initiated with jump button and not grounded yet)
	private bool jumpingReachedApex = false;


	private bool movingBack = false;	// Are we moving backwards (This locks the camera to not do a 180 degree spin)
	private bool isMoving = false;	// Is the user pressing any keys?
	private float walkTimeStart = 0.0f;	// When did the user start walking (Used for going into trot after a while)
	private float lastJumpButtonTime = -10.0f;	// Last time the jump button was clicked down
	private float lastJumpTime = -1.0f;	// Last time we performed a jump

	private float lastJumpStartHeight = 0.0f;	// the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)

	private Vector3 inAirVelocity = Vector3.zero;
	private float lastGroundedTime = 0.0f;

	private bool isControllable = true;

void Awake ()
{
	moveDirection = transform.TransformDirection(Vector3.forward);	
}

void UpdateSmoothedMovementDirection ()
{
	bool grounded = IsGrounded();
	
	// Forward vector relative to the camera along the x-z plane	
	Vector3 forward = myCamera.transform.TransformDirection(Vector3.forward);
	forward.y = 0f;
	forward = forward.normalized;

	// Right vector relative to the camera
	// Always orthogonal to the forward vector
	Vector3 right = new Vector3(forward.z, 0f, -forward.x);

	float v = Input.GetAxisRaw("Vertical");
	float h = Input.GetAxisRaw("Horizontal");

	// Are we moving backwards or looking backwards
	if (v < -0.2f){
		movingBack = true;
		v = -1f;
		h = 0.0f;
		forward = Vector3.zero;
		right = Vector3.zero;
		}
	else
		movingBack = false;
	
	bool wasMoving = isMoving;
	isMoving = Mathf.Abs (h) > 0.1f || Mathf.Abs (v) > 0.1f;
		
	// Target direction relative to the camera
	Vector3 targetDirection = Vector3.zero;
	bool isBlocking = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
	if (!isBlocking) 
			targetDirection = h * right + v * forward;
	//else
	//targetDirection = Vector3.zero;
	// Grounded controls
	if (grounded)
	{
		// Lock camera for short period when transitioning moving & standing still
		lockCameraTimer += Time.deltaTime;
		if (isMoving != wasMoving)
			lockCameraTimer = 0.0f;

		// We store speed and direction seperately,
		// so that when the character stands still we still have a valid forward direction
		// moveDirection is always normalized, and we only update it if there is user input.
		if (targetDirection != Vector3.zero && !isBlocking)
		{
			// If we are really slow, just snap to the target direction
			if (grounded)
			{
			//	moveDirection = targetDirection.normalized;
			//}
			// Otherwise smoothly turn towards it
			//else
			//{
				moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000);
				
				moveDirection = moveDirection.normalized;
			}
		}
		
		// Smooth the speed based on the current target direction
		float curSmooth = speedSmoothing * Time.deltaTime;
		
		// Choose target speed
		//* We want to support analog input but make sure you cant walk faster diagonally than just forward or sideways
		float targetSpeed = Mathf.Min(targetDirection.magnitude, 1.0f);
	
		_characterState = CharacterState.Idle;


		// Pick speed modifier
			if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift)) && !isBlocking && coolTimer > oldCooldown && runTimer >= 0f)
			{
				if (!flag)
				{
					float run = gameObject.GetComponent<PlayerRunGA>().runOUT;
					float cd = gameObject.GetComponent<PlayerRunGA>().cdOUT;
					cooldown = Random.Range (5.0f,8.0f)-cd;
					if (cooldown < 0){
						cooldown = 0;
					}
					runSpeed = Random.Range (7.0f,10.0f)+run;
					gameObject.GetComponent<PlayerRunGA>().AddToList(runSpeed, cooldown);
					flag = true;
				}
				runTimer -= Time.deltaTime;

				targetSpeed *= runSpeed;
				_characterState = CharacterState.Running;
			}
		else if (Time.time - trotAfterSeconds > walkTimeStart)
			{
				targetSpeed *= trotSpeed;
				_characterState = CharacterState.Trotting;
				if (flag){
					coolTimer = 0f;
					runTimer = 5f;
					oldCooldown = cooldown;
				}
				flag = false;
			}
		else
			{
				if (flag){
					coolTimer = 0f;
					runTimer = 5f;
					oldCooldown = cooldown;
				}
				targetSpeed *= walkSpeed;
				_characterState = CharacterState.Walking;
				flag = false;
			}
		
		moveSpeed = Mathf.Lerp(moveSpeed, targetSpeed, curSmooth);
		
		// Reset walk time start when we slow down
		if (moveSpeed < walkSpeed * 0.3f)
			walkTimeStart = Time.time;
	}
	// In air controls
	else
	{
		// Lock camera while in air
		if (jumping)
			lockCameraTimer = 0.0f;

		if (isMoving)
			inAirVelocity += targetDirection.normalized * Time.deltaTime * inAirControlAcceleration;
	}		
}


void ApplyJumping ()
{
	// Prevent jumping too fast after each other
	if (lastJumpTime + jumpRepeatTime > Time.time) 
		return;

	if (IsGrounded()) {
		// Jump
		// - Only when pressing the button down
		// - With a timeout so you can press the button slightly before landing		
		if (canJump && Time.time < lastJumpButtonTime + jumpTimeout) {
				float jump = gameObject.GetComponent<PlayerJumpGA>().jumpOUT;
			jumpHeight = Random.Range(1.0f,5.0f)+jump;
				/////////////////////////////
				// Next gen goes here!
				/////////////////////////////
			gameObject.GetComponent<PlayerJumpGA>().AddToList(jumpHeight);
			verticalSpeed = CalculateJumpVerticalSpeed (jumpHeight);
			SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
		}
	}
}


void ApplyGravity ()
{
	if (isControllable)	// don't move player at all if not controllable.
	{
		// Apply gravity
		bool jumpButton = Input.GetButton("Jump");
		
		
		// When we reach the apex of the jump we send out a message
		if (jumping && !jumpingReachedApex && verticalSpeed <= 0.0f)
		{
			jumpingReachedApex = true;
			SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
		}
	
		if (IsGrounded ())
			verticalSpeed = 0.0f;
		else
			verticalSpeed -= gravity * Time.deltaTime;
	}
}

	float CalculateJumpVerticalSpeed (float targetJumpHeight)
{
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt(2 * targetJumpHeight * gravity);
}

void DidJump ()
{
	jumping = true;
	jumpingReachedApex = false;
	lastJumpTime = Time.time;
	lastJumpStartHeight = transform.position.y;
	lastJumpButtonTime = -10f;
}

void Update() {
	coolTimer += Time.deltaTime;
	
	if (!isControllable)
	{
		// kill all inputs if not controllable.
		Input.ResetInputAxes();
	}

	if (Input.GetButtonDown ("Jump"))
	{
		lastJumpButtonTime = Time.time;
	}

	UpdateSmoothedMovementDirection();
	
	// Apply gravity
	// - extra power jump modifies gravity
	// - controlledDescent mode modifies gravity
	ApplyGravity ();

	// Apply jumping logic
	ApplyJumping ();
	
	// Calculate actual motion
	Vector3 movement = moveDirection * moveSpeed + new Vector3 (0f, verticalSpeed, 0f) + inAirVelocity;
	movement *= Time.deltaTime;
	
	// Move the controller
	CharacterController controller = GetComponent<CharacterController>();
	collisionFlags = controller.Move(movement);
	
		
	// Set rotation to the move direction
	if (IsGrounded())
	{		
		transform.rotation = Quaternion.LookRotation(moveDirection);			
	}	
	else
	{
		Vector3 xzMove = movement;
		xzMove.y = 0f;
		if (xzMove.sqrMagnitude > 0.001f)
		{
			transform.rotation = Quaternion.LookRotation(xzMove);
		}
	}	
	
	// We are in jump mode but just became grounded
	if (IsGrounded())
	{
		lastGroundedTime = Time.time;
		inAirVelocity = Vector3.zero;
		if (jumping)
		{
			jumping = false;
			SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
		}
	}
}

//ControllerColliderHit OnControllerColliderHit (ControllerColliderHit hit)
//{
//	Debug.DrawRay(hit.point, hit.normal);
//	if (hit.moveDirection.y > 0.01f) 
//		return;
//}

float GetSpeed () {
	return moveSpeed;
}

public bool IsJumping () {
	return jumping;
}

bool IsGrounded () {
	return (collisionFlags & CollisionFlags.CollidedBelow) != 0f;
}

Vector3 GetDirection () {
	return moveDirection;
}

public bool IsMovingBackwards () {
	return movingBack;
}

public float GetLockCameraTimer () 
{
	return lockCameraTimer;
}

bool IsMoving ()
{
	return Mathf.Abs(Input.GetAxisRaw("Vertical")) + Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f;
}

bool HasJumpReachedApex ()
{
	return jumpingReachedApex;
}

bool IsGroundedWithTimeout ()
{
	return lastGroundedTime + groundedTimeout > Time.time;
}

void Reset ()
{
	gameObject.tag = "Player";
}

}