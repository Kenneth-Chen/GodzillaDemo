using UnityEngine;
using System.Collections;

public class GrowthPowerup : MonoBehaviour {
	void OnTriggerEnter(Collider collider) {
		CharacterController cc;
		if((cc = collider.GetComponent<CharacterController>()) != null) {
			cc.gameObject.AddComponent<GrowthPowerupActivated>();
			cc.gameObject.AddComponent<GiantFootsteps>();
			Destroy (this.gameObject);
		}
	}
}
