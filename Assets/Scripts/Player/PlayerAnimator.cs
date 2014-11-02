using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {
	private Animator anim;
	private DiveFPSController controller;

	void Start() {
		anim = GetComponent<Animator> ();
		controller = Grid.playerComponent;
	}

	void Update() {
		float speed = controller.Speed;
		anim.SetFloat ("Speed", speed);
	}
}
