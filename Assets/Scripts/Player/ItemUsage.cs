using UnityEngine;
using System.Collections;

public class ItemUsage : MonoBehaviour {

	public GameObject rightHandSlot;
	// the distance beyond which we drop the object in the player's hand
	private const float itemDropDistanceLimit = 13.0f;
	private const float sqrItemDropDistanceLimit = itemDropDistanceLimit * itemDropDistanceLimit;
	
	void Update() {
		if(InputManager.GetAction("Pickup")) {
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 12)) {
				GameObject obj = hit.collider.gameObject;
				// pick up the object we're looking at, and drop whatever else we're holding
				if(obj.GetComponent<Rigidbody>() != null) {
					DropObjectInRightHandSlot();
					obj.transform.parent = rightHandSlot.transform;
					obj.transform.position = rightHandSlot.transform.position;
					obj.rigidbody.angularVelocity = Vector3.zero;
					obj.rigidbody.velocity = Vector3.zero;
				}
			}
		}
		if(InputManager.GetAction("Drop")) {
			DropObjectInRightHandSlot();
		}
		foreach(Transform child in rightHandSlot.transform) {
			if(rightHandSlot.transform == child) {
				continue;
			}
			// if the object is too far away from our hand, then release it
			Vector3 objectToHand = rightHandSlot.transform.position - child.position;
			if(objectToHand.sqrMagnitude > sqrItemDropDistanceLimit) {
				child.parent = null;
			}
			// apply a returning force so the object stays in our hand
			//child.rigidbody.AddForce(20*objectToHand, ForceMode.Force);
			// or maybe just keep velocity at zero
			if(child.rigidbody != null) {
				child.rigidbody.position = rightHandSlot.transform.position;
				child.rigidbody.velocity = Vector3.zero;
				child.rigidbody.angularVelocity = Vector3.zero;
			}
		}
	}

	void DropObjectInRightHandSlot() {
		foreach(Transform child in rightHandSlot.transform) {
			child.parent = null;
		}
	}
}
