using UnityEngine;
using System.Collections;

public class AlternatingFootsteps : MonoBehaviour {

	public int footstepLength = 7;
	private int currentFootstepCount = 0;
	private AudioSource footstepSoundL;
	private AudioSource footstepSoundR;
	enum Foot { Left, Right };
	private Foot nextFoot;

	DiveFPSController diveFPSController;
	void Start () {
		diveFPSController = this.gameObject.GetComponent<DiveFPSController> ();
		nextFoot = Foot.Left;
		AudioSource[] aSources = GetComponents<AudioSource>();
		footstepSoundL = aSources[0];
		footstepSoundR = aSources[1];
	}
	
	void LateUpdate () {
		bool isMoving = diveFPSController.isGrounded && diveFPSController.DistanceMoved != Vector3.zero;
		bool shouldPlay = isMoving && ++currentFootstepCount >= footstepLength;
		if(shouldPlay) {
			currentFootstepCount = 0;
			if(nextFoot == Foot.Left) {
				nextFoot = Foot.Right;
				footstepSoundL.Stop();
				footstepSoundL.Play();
			} else {
				nextFoot = Foot.Left;
				footstepSoundR.Stop();
				footstepSoundR.Play();
			}
		}
	}
}
