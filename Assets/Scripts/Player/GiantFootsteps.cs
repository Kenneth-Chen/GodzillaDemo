using UnityEngine;
using System.Collections;

public class GiantFootsteps : MonoBehaviour {

	public int footstepLength = 40;
	private int currentFootstepCount = 0;
	private AudioSource footstepSound;

	DiveFPSController diveFPSController;
	void Start () {
		diveFPSController = this.gameObject.GetComponent<DiveFPSController> ();
		AudioSource[] aSources = GetComponents<AudioSource>();
		footstepSound = aSources[1];
	}

	void LateUpdate () {
		if (diveFPSController.isGrounded && diveFPSController.DistanceMoved != Vector3.zero) {
			if(++currentFootstepCount >= footstepLength) {
				currentFootstepCount = 0;
				footstepSound.Play();
			}
		}
	}
}
