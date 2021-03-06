﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(CharacterController))]
public class DiveFPSController : MonoBehaviour {
		
	private Vector3 distanceMoved;
	public Vector3 DistanceMoved {
		get {
			return distanceMoved;
		}
	}
	public float SpeedX {
		get {
			return distanceMoved.x / max_speed;
		}
	}
	public float SpeedZ {
		get {
			return distanceMoved.z / max_speed;
		}
	}
	private bool isJumping = false;
	public bool IsJumping {
		get {
			return isJumping;
		}
	}
	bool inputJump=false;
	bool grounded=false;
	public bool isGrounded {
		get {
			return grounded;
		}
	}
	Vector3 velocity;
	public float max_speed=0.1f;
	public float max_speed_air=0.13f;
	public float max_speed_ground=0.1f;
	public int acceleration=10;
	public int acceleration_air=10;
	public float gravity=-0.18f;
	public float friction=0.8f;
	private float terminalVelocity=-2.5f;
	private CharacterController controller;
	public Vector3 groundNormal;
	public float jumpspeed=0.16f;
	public bool stopmovingup=false;
	public float fallkillspeed=-0.38f;
	public float killPositionLowerBound=-100;
	public CollisionFlags collisionFlags; 
	private float rotationSpeed = 30.0f; // rotate speed in degrees/second

	public GameObject ground_gameobject=null;
	public Vector3 last_ground_pos;
	public bool floating=false;
	
	public int autowalk=0;
	public float inhibit_autowalk=1;
	public int reload_once=0;
	
	void Awake (){
		controller = GetComponent<CharacterController>();
	}
	
	void toggle_autowalk (){
		if (autowalk==0)autowalk=1;
		else autowalk=0;
	}

	void Start (){
		reload_once=0;
	}

	void OnControllerColliderHit ( ControllerColliderHit hit  ){
		if (hit.normal.y > 0 && hit.moveDirection.y < 0) {
			groundNormal = hit.normal;
			grounded=true;	
			ground_gameobject=hit.gameObject;
			
			//print("Landed on: ground"+ground_gameobject.name);
			stopmovingup=false;
		}
	}

	float fadeduration= 2.0f; // fade duration in seconds
	
	IEnumerator Die() {
		// create a GUITexture:
		GameObject fade = new GameObject();
		fade.AddComponent<GUITexture>();
		// and set it to the screen dimensions:
		fade.guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
		// set its texture to a black pixel:
		Texture2D tex= new Texture2D(1, 1);
		tex.SetPixel(0, 0, Color.black);
		tex.Apply();
		fade.guiTexture.texture = tex;
		// then fade it during duration seconds
		for (float alpha = 0.0f; alpha < 1.0f; ){
			alpha += Time.deltaTime / fadeduration;
			fade.guiTexture.color = new Color(fade.guiTexture.color.r, fade.guiTexture.color.g, fade.guiTexture.color.b, alpha);
			yield return 0;
		}
		// finally, reload the current level:
		if (reload_once==0){
			reload_once =1;
			AsyncOperation async = Application.LoadLevelAsync(Application.loadedLevel);
			yield return async;
		}
		Debug.Log ("reloading all game objects");
		Grid.LoadAllGameObjects ();
	}

	void Update () {
		if (velocity.y < fallkillspeed || this.transform.position.y < killPositionLowerBound || InputManager.GetAction("Suicide")) {
			StartCoroutine(Die());
			return;
		}

		Debug.DrawLine (transform.position, transform.position+groundNormal, Color.red);
		//print("GroundNormal y" +groundNormal.y);

		Vector3 directionVector;
		directionVector = new Vector3(InputManager.GetAxis("Horizontal"), 0, InputManager.GetAxis("Vertical"));

		if (autowalk==1) directionVector = new Vector3(0,0,1*inhibit_autowalk);
		if (directionVector != Vector3.zero) {
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			float directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		inputJump = InputManager.GetAction("Jump");

		grounded = (collisionFlags & CollisionFlags.Below) != 0;

		if (floating){
			velocity.y=0.1f;
		}
		if (inputJump&&grounded){
			isJumping = true;
			velocity.y+=jumpspeed;
			velocity.z*=1.5f;

			grounded=false;
		}

		bool flightEnabled = false;
		float inputRotation = InputManager.GetAxis ("RHorizontal");
		float inputFlight = InputManager.GetAxis ("RVertical");
		if(flightEnabled) {
			velocity.y += -0.1f * inputFlight * jumpspeed;
		}

		if(grounded) {
			isJumping = false;
		}

		if (grounded) {
			velocity+=directionVector*acceleration*Time.deltaTime;
		}
		else {
			velocity+=directionVector*acceleration_air*Time.deltaTime;
		}
		
		Vector3 translation = new Vector3(velocity.x,0,velocity.z);
		
		translation = Vector3.Lerp(Vector3.zero,translation,friction);
		
		velocity.x = translation.x;
		velocity.z = translation.z;
		velocity.y = velocity.y + gravity * Time.deltaTime;
		if(velocity.y < terminalVelocity) {
			velocity.y = terminalVelocity;
		}
		//print ("Time deltatime "+Time.deltaTime);

		if (grounded)velocity.y=0;
		//print ("Vel y "+velocity.y);

		if (grounded) max_speed = max_speed_ground;
		if (!grounded) max_speed = max_speed_air;
		
		if (translation.magnitude > max_speed){
			translation = translation / translation.magnitude;
			translation = translation * max_speed;
		}
		velocity.x = translation.x;
		velocity.z = translation.z;
		translation.y = velocity.y;

		Quaternion yrotation_camera = Quaternion.Euler(0, Grid.cameraObject.transform.rotation.eulerAngles.y, 0);
		//transform.position+=yrotation_camera*translation;

		Vector3 platformdelta = Vector3.zero;
		
		if(ground_gameobject != null) {
			//print("ground object ungleich null");
			platformdelta = ground_gameobject.transform.position - last_ground_pos;
		}
		
		//if (!grounded)platformdelta=Vector3.zero;

		Vector3 origPosition = controller.transform.position;

		//MAKE A MOVE!
		transform.Rotate(0, inputRotation * rotationSpeed * Time.deltaTime, 0);
		collisionFlags=controller.Move(yrotation_camera*translation+platformdelta);

		distanceMoved = controller.transform.position - origPosition;

		if ((collisionFlags & CollisionFlags.CollidedAbove) != 0)
		{
			if (stopmovingup==false){
				velocity.y=0;
				stopmovingup=true;
			}
		}

		if (ground_gameobject!=null && !grounded){
			ground_gameobject=null;
		}

		if(ground_gameobject!=null)last_ground_pos=ground_gameobject.transform.position;
	}

	void  OnTriggerEnter (Collider other){
		if (other.name == "Float") {
			floating=true;
			inhibit_autowalk=0.1f;	
		}
	}
	
	void  OnTriggerExit (Collider other){
		if (other.name == "Float") {
			floating=false;
			inhibit_autowalk=1;
		}
	}
}