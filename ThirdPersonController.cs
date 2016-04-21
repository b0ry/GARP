using UnityEngine;
using System.Collections;
using GARP.GA;

public class ThirdPersonController : MonoBehaviour {
	public int hits;
	public GameObject hit;
	public float blockEffect ;
	private bool blockFlag;
	private int damage;

	public GameObject projectile;
	public GameObject holder;

	public GameObject smoke;

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
	public bool canJump = true;
	private CharacterState _characterState;

	private float walkSpeed = 6.0f; // The speed when walking
	private float trotSpeed = 7.0f; // after trotAfterSeconds of walking we trot with trotSpeed
	private float runSpeed = 9.0f; // when pressing "Fire3" button (shift) we start running

	private float inAirControlAcceleration = 3.0f;
	private float jumpHeight = 0.5f;// How high do we jump when pressing jump and letting go immediately
	
	private float gravity = 20.0f; // The gravity for the character
	private float speedSmoothing = 10.0f; // The gravity in controlled descent mode
	private float rotateSpeed = 500.0f;

	private float trotAfterSeconds = 3.0f;

	private float jumpRepeatTime = 0.05f;
	private float jumpTimeout = 0.15f;
	private float groundedTimeout = 0.25f;

	// The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.
	private float lockCameraTimer = 0.3f; // The camera doesnt start following the target immediately but waits for a split second to avoid too much waving around.


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
	private static GameObject jumper;

void Awake ()
{
	moveDirection = transform.TransformDirection(Vector3.forward);	
}

void PlayerFire(){
	if (!blockFlag){
		GameObject h = (GameObject)Instantiate(holder,new Vector3(transform.localPosition.x, transform.localPosition.y+0.5f, transform.localPosition.z+1f),transform.localRotation);
		GameObject g = (GameObject)Instantiate(projectile,h.transform.position,h.transform.rotation);
		g.transform.SetParent (h.transform);
		Destroy (g,3f);
		Destroy (h,3f);
	}
}

void Block() {
		
	// Block
	if (Input.GetKeyDown(KeyCode.S))
		blockFlag = true;
	
	if (Input.GetKey(KeyCode.S)){
		for (int i = 0; i < 3; i++){
			damage= Random.Range (1,11);
			float rnd = ((float)damage/40f)+0.25f;
			gameObject.transform.GetChild (i).GetComponent<Renderer>().material.color = new Color (rnd,rnd,1f,1f);
		}
	}
	// Not Block
	else
	{
		blockFlag = false;
		for (int i = 0; i < 3; i++){
			gameObject.transform.GetChild (i).GetComponent<Renderer>().material.color = gameObject.transform.GetChild (i).GetComponent<ShapeMove>().normal;
		}
	}
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

	// Don't move if pressing backwards
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
	isMoving = Mathf.Abs (h) > 0.5f || Mathf.Abs (v) > 0.5f;
		
	// Target direction relative to the camera
	Vector3 targetDirection = Vector3.zero;
	bool isBlocking = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
	if (!isBlocking) 
			targetDirection = h * right + v * forward;

	// Grounded controls
	if (grounded)
	{
			for (int i = 0; i < 3; i++){
				float bounce = gameObject.transform.GetChild (i).GetComponent<ShapeMove>().initial;
				if( bounce < 3f || isMoving)
				{
					BroadcastMessage("Bounce", SendMessageOptions.DontRequireReceiver);
				}
			}

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
					Run run4list = new Run();
					run4list.speed = runSpeed;
					run4list.cooldown = cooldown;
					SendMessage("AddToRunList", run4list, SendMessageOptions.DontRequireReceiver);

					GameObject runShape = (GameObject)Instantiate(smoke, transform.position, transform.rotation);
					runShape.transform.parent = this.transform;
					Destroy (runShape, 5f);

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
			//Debug.Log(inAirVelocity);
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
	if( Input.GetMouseButtonDown(0)) {InvokeRepeating("PlayerFire",0f,0.5f);}
	if( Input.GetMouseButtonUp(0)) {CancelInvoke("PlayerFire");}
	
	coolTimer += Time.deltaTime;

	if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.A)) {
		BroadcastMessage("Strafe","Left",SendMessageOptions.DontRequireReceiver);
	}
	if (Input.GetKey (KeyCode.S) && Input.GetKey (KeyCode.D)) {
		BroadcastMessage("Strafe","Right",SendMessageOptions.DontRequireReceiver);
	}

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

	Block ();
	
	// Apply gravity
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

	void OnCollisionEnter(Collision slash){
		if (slash.gameObject.tag == "slash"){
			GameObject healthBar = GameObject.Find("Health Bar");
			int level = transform.GetComponent<PlayerBlockGA>().levels.blockLevel;
			if (!blockFlag){
				healthBar.GetComponent<HealthBar>().currentDamage = damage;
				Rigidbody g = Instantiate(hit,transform.position,transform.rotation)as Rigidbody;
				BroadcastMessage("PlayerFlash", SendMessageOptions.DontRequireReceiver);
			}
			else if (level == 1) {
				blockEffect = Random.value;
				float newDam = (1-blockEffect) * (float)damage;
				healthBar.GetComponent<HealthBar>().currentDamage = (int)newDam;
				TextMesh damCount = GetComponentInChildren<TextMesh>();
				blockEffect *= 100;
				int block4text = (int)blockEffect;
				damCount.text = block4text.ToString () + "%";
				int counter = GetComponentInChildren<ShowText>().counter;
				counter = 50;
				Block block = new Block();
				block.block = block4text;
				SendMessage("AddToBlockList", block, SendMessageOptions.DontRequireReceiver);
				hits++;
			}
			else {
				int rnd = Random.Range (0,transform.GetComponent<PlayerBlockGA>().blockEffectOUT.Count);
				int block4text = transform.GetComponent<PlayerBlockGA>().blockEffectOUT[rnd];
				blockEffect = 1-((float)block4text/100f);
				//block4text = 100 - block4text;
				int mutation = Random.Range (0,10);
				if (mutation == 0) { 
					blockEffect = Random.Range(0.0f, 0.5f);
					blockEffect *= 100;
					block4text = 100-(int)blockEffect;
					Debug.Log (blockEffect);
				}
				float newDam = blockEffect * (float)damage;
				healthBar.GetComponent<HealthBar>().currentDamage = (int)newDam;
				TextMesh damCount = GetComponentInChildren<TextMesh>();
				damCount.text = block4text.ToString () + "%";
				int counter = GetComponentInChildren<ShowText>().counter;
				counter = 50;
				int idx = transform.GetComponent<PlayerBlockGA>().i;
				if (idx == 0) { 
					transform.GetComponent<PlayerBlockGA>().blockEffectIN.Clear();
				}
				Block block = new Block();
				block.block = block4text;
				SendMessage("AddToBlockList", block, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	

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