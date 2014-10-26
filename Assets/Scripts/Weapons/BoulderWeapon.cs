using UnityEngine;
using System.Collections;

public class BoulderWeapon : MonoBehaviour {
	public GameObject cameraObject;
	private float unit = 2.5f;
	private float speed = 20.0f;
	private float currentCooldown = 0.0f;
	private float cooldown = 3.0f;
	private bool giantMode = false;
	private float scaleFactor = 10.0f;
	
	void Start() {
	}

	void OnGUI () {
		Event e = Event.current;
		if (e.isKey) {
			// Debug.Log("Detected key code: " + e.keyCode);
			if(e.keyCode == KeyCode.Z && !giantMode) {
				giantMode = true;
//				this.transform.localScale *= 10;
				CharacterController cc = GetComponent<CharacterController>();
//				cc.radius *= scaleFactor;
//				cc.height *= scaleFactor;
				GameObject cameraLeft = cameraObject.transform.FindChild("Camera_left").gameObject;
				GameObject cameraRight = cameraObject.transform.FindChild("Camera_right").gameObject;
//				cameraLeft.transform.position = new Vector3(scaleFactor * cameraLeft.transform.position.x, cameraLeft.transform.position.y, cameraLeft.transform.position.z);
//				cameraRight.transform.position = new Vector3(scaleFactor * cameraRight.transform.position.x, cameraRight.transform.position.y, cameraRight.transform.position.z);
			}
			// fire boulder weapon
			if(e.keyCode == KeyCode.F && currentCooldown <= 0.0f) {
				currentCooldown = cooldown;
				Vector3 fwd = cameraObject.transform.TransformDirection(Vector3.forward) * unit;
				Vector3 offset = transform.position + fwd + 2*Vector3.down;
				GameObject boulder = (GameObject)Instantiate(Grid.boulderPrefab, offset, transform.rotation);
				Rigidbody rb = boulder.AddComponent<Rigidbody>();
				rb.mass = 1.0f;
				rb.AddForce(fwd * speed, ForceMode.Impulse);
				audio.Play();
				Destroy (boulder, 120);
			}
		}
		
	}
	
	void Update() {
		if(currentCooldown > 0.0f) {
			currentCooldown -= 0.1f;
		}
		if(currentCooldown < 0.0f) {
			currentCooldown = 0.0f;
		}
	}
}
