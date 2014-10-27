using UnityEngine;
using System.Collections;

// this script must be attached to a wieldable prefab
// indicates that an item can be picked up by the player
public class Wieldable : MonoBehaviour {
	public AudioSource pickupSound;

	void OnTriggerEnter(Collider collider) {
		CharacterController cc;
		if((cc = collider.GetComponent<CharacterController>()) != null) {
			Transform obj = this.gameObject.transform.parent;
			Quaternion origRotation = obj.rotation;
			obj.parent = Grid.rightHandItemSlot.transform;
			obj.localPosition = Vector3.zero;
			if(pickupSound != null) {
				pickupSound.Play();
			}
			StartCoroutine(ResetRotation(obj, origRotation));
		}
	}

	IEnumerator ResetRotation(Transform obj, Quaternion rotation) {
		yield return 0;
		obj.rotation = rotation;
		// destroy the Wieldable prefab
		Destroy(this.gameObject);
	}
}
