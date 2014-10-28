using UnityEngine;
using System.Collections;

public class FlameBreath : MonoBehaviour {
	public float flameDuration = 10.0f;
	private float durationRemaining = 0.0f;
	private bool breathOn = false;
	private ParticleSystem particles;

	void Start() {
		particles = Grid.fireBreath.GetComponent<ParticleSystem> ();
	}

	void Update() {
		if(InputManager.GetAction("SecondaryAttack")) {
			durationRemaining = flameDuration;
			breathOn = true;
			Grid.fireBreath.SetActive(true);
			particles.loop = true;
		}
		if(durationRemaining > 0.0f) {
			durationRemaining -= 0.1f;
		}
		if(durationRemaining <= 0.0f) {
			durationRemaining = 0.0f;
			if(breathOn) {
				particles.loop = false;
				StartCoroutine(DeactivateBreath());
			}
		}
		if(breathOn) {
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 12)) {
				GameObject target = hit.collider.gameObject;
				Effects.SetOnFire(target, 40, hit.point);
			}
		}
	}
	
	IEnumerator DeactivateBreath() {
		yield return new WaitForSeconds(5);
		if(durationRemaining <= 0.0f) {
			Grid.fireBreath.SetActive(false);
			breathOn = false;
		}
	}

}
