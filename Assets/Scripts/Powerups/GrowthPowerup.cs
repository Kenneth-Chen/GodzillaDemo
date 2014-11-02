using UnityEngine;
using System.Collections;

public class GrowthPowerup : MonoBehaviour {
	private bool enabled = true;
	void OnTriggerEnter(Collider collider) {
		if(!enabled) {
			return;
		}
		CharacterController cc;
		if((cc = collider.GetComponent<CharacterController>()) != null) {
			if(cc.gameObject.GetComponent<GiantFootsteps>() == null) {
				enabled = false;
				gameObject.GetComponent<ParticleSystem>().enableEmission = false;
				(gameObject.GetComponent("Halo") as Behaviour).enabled = false;
				Destroy (cc.gameObject.GetComponent<ContinuousFootsteps>());
				cc.gameObject.AddComponent<GrowthPowerupActivated>();
				cc.gameObject.AddComponent<GiantFootsteps>();
				cc.gameObject.AddComponent<BoulderWeapon>();
				cc.gameObject.AddComponent<FlameBreath>();
				audio.Play ();
			}
			Destroy (this.gameObject, 3);
		}
	}
}
