using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public float startDelay = 30.0f;
	private bool startedPlaying = false; 
	
	void Update () {
		if(!startedPlaying) {
			if(Time.time > startDelay) {
				if(Grid.playerComponent.DistanceMoved != Vector3.zero) {
					startedPlaying = true;
					audio.Play();
				}
			}
		}
	}
}
