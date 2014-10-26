using UnityEngine;
using System.Collections;

public class ActivateScreaming : MonoBehaviour {

	private bool activated = false;
	private CharacterController controller;
	public const float activationRadius = 90.0f;
	
	void Start () {
		controller = GameObject.FindObjectOfType<CharacterController> ();	
	}

	void OnCollisionEnter(Collision collision) {
		if(activated)
			return;
		Debug.Log (collision.gameObject);
		if(collision.gameObject.tag == "Flames" || collision.gameObject.tag == "Explosion") {
			Debug.Log ("activated");
			activated = true;
			audio.Play();
		}
	}

	void Update () {
		if(activated)
			return;

		Collider[] hit = Physics.OverlapSphere (transform.position, activationRadius);
		foreach(Collider collider in hit) {
			if(collider.gameObject.tag == "Flames" || collider.gameObject.tag == "Explosion") {
				Debug.Log ("activated");
				activated = true;
				audio.Play();
				break;
			}
		}
	}
}
