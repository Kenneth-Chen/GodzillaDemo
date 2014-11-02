using UnityEngine;
using System.Collections;

public class ContinuousFootsteps : MonoBehaviour {

	public int audioSourceIndex = 0;
	private bool playing;
	private AudioSource footstepSound;
	private const float epsilon = 0.01f;
	
	DiveFPSController diveFPSController;
	void Start () {
		diveFPSController = this.gameObject.GetComponent<DiveFPSController> ();
		playing = false;
		AudioSource[] aSources = GetComponents<AudioSource>();
		footstepSound = aSources[audioSourceIndex];
	}
	
	void LateUpdate () {
		bool shouldPlay = diveFPSController.isGrounded && diveFPSController.DistanceMoved.sqrMagnitude > epsilon;
		if(playing && !shouldPlay) {
			playing = false;
			footstepSound.Pause();
		} else if(!playing && shouldPlay) {
			playing = true;
			footstepSound.Play();
		}
	}

	void OnDestroy() {
		footstepSound.Stop ();
	}
}
