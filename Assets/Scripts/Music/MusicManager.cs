using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public float startDelay = 30.0f;
	public bool requirePlayerMovement = true;
	private bool startedPlaying = false; 
	private float timePassed = 0.0f;
	void Update () {
		if(!startedPlaying) {
			if((transform.position - Grid.playerObject.transform.position).sqrMagnitude < 500.0f*500.0f) {
				timePassed += Time.deltaTime;
				if(timePassed > startDelay) {
					if(!requirePlayerMovement || Grid.playerComponent.DistanceMoved != Vector3.zero) {
						startedPlaying = true;
						audio.Play();
					}
				}
			}
		}
	}
}
