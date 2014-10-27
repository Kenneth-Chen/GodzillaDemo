using UnityEngine;
using System.Collections;

public class GiantFootsteps : MonoBehaviour {

	public int footstepLength = 80;
	private int currentFootstepCount = 0;

	DiveFPSController diveFPSController;
	void Start () {
		diveFPSController = this.gameObject.GetComponent<DiveFPSController> ();
	
	}

	void LateUpdate () {
		if (diveFPSController.isGrounded && diveFPSController.movementDirection != Vector3.zero) {
			if(++currentFootstepCount >= footstepLength) {
				currentFootstepCount = 0;
				AudioSource[] aSources = GetComponents<AudioSource>();
				AudioSource footstepSound = aSources[1];
				footstepSound.Play();
			}
		}
	}
}
