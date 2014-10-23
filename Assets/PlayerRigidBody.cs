using UnityEngine;
using System.Collections;

public class PlayerRigidBody : MonoBehaviour {

	// Script added to a player for it to be able to push rigidbodies around.
	
	// How hard the player can push
	float LightpushPower = 0.5f;
	float HeavypushPower = 0.1f;
	
	// Which layers the player can push
	// This is useful to make unpushable rigidbodies
	LayerMask pushLayers = -1;
	
	// pointer to the player so we can get values from it quickly
	private CharacterController controller;
	
	void Start () {
		controller = GetComponent<CharacterController>();	
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit) {
		// if(hit.gameObject.tag == "enemy") {
			Rigidbody body = hit.collider.attachedRigidbody;
			// no rigidbody
			if (body == null || body.isKinematic)
				return;
			
			// Only push rigidbodies in the right layers
			var bodyLayerMask = 1 << body.gameObject.layer;
			if ((bodyLayerMask & pushLayers.value) == 0)
				return;
			
			// We dont want to push objects below us
			if (hit.moveDirection.y < -0.3) 
				return;
			
			// Calculate push direction from move direction, we only push objects to the sides
			// never up and down
			Vector3 pushDir = new Vector3 (hit.moveDirection.x, 0, hit.moveDirection.z);
			
			body.AddForce(pushDir * LightpushPower, ForceMode.Impulse);
		// }
	}

}
