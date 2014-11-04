using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {
	private Animator anim;
	private DiveFPSController controller;
	private GameObject player;
	private float epsilon = 0.01f;

	void Start() {
		anim = GetComponent<Animator> ();
		controller = Grid.playerComponent;
		player = Grid.playerObject;
	}

	void Update() {
		// left-right speed from -1 to 1
		float speedX = controller.SpeedX;
		// forward-back speed from -1 to 1
		float speedZ = controller.SpeedZ;
		// player orientation in degrees from 0 to 360
		float eulerOrientation = player.transform.localRotation.eulerAngles.y;

		float forwardSpeed = speedX * Mathf.Sin(Mathf.Deg2Rad*eulerOrientation) + speedZ * Mathf.Cos(Mathf.Deg2Rad*eulerOrientation);
		float strafeSpeed = speedX * Mathf.Cos(Mathf.Deg2Rad*eulerOrientation) - speedZ * Mathf.Sin(Mathf.Deg2Rad*eulerOrientation);
		anim.SetBool ("IsLocomoting", (forwardSpeed > epsilon || forwardSpeed < -epsilon) || (strafeSpeed > epsilon || strafeSpeed < -epsilon));
		anim.SetBool ("IsJumping", controller.IsJumping);
		anim.SetFloat ("ForwardSpeed", forwardSpeed);
		anim.SetFloat ("StrafeSpeed", strafeSpeed);
	}
}
