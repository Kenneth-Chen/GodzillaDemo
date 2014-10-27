using UnityEngine;
using System.Collections;

public class FlameBreath : MonoBehaviour {
	public GameObject fireBreath;
	public float flameDuration = 10.0f;
	private float durationRemaining = 0.0f;
	private bool breathOn = false;
	private ParticleSystem particles;

	void Start() {
		particles = fireBreath.GetComponent<ParticleSystem> ();
	}
	
	void OnGUI () {
		Event e = Event.current;
		if (e.isKey) {
			if(e.keyCode == KeyCode.B) {
				durationRemaining = flameDuration;
				breathOn = true;
				fireBreath.SetActive(true);
				particles.loop = true;
			}
		}

	}

	void Update() {
		if(durationRemaining > 0.0f) {
			durationRemaining -= 0.1f;
		}
		if(durationRemaining <= 0.0f) {
			durationRemaining = 0.0f;
			particles.loop = false;
			StartCoroutine(DeactivateBreath());
		}
		if(breathOn) {
			RaycastHit hit;
			if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 12)) {
				GameObject target = hit.collider.gameObject;
				if(target.tag != "Terrain" && target.tag != "Player" && target.tag != "Flames" && target.tag != "Ammo" && target.tag != "Portal"
				   && target.GetComponent<TerrainCollider>() == null) {
					bool isOnFire = target.GetComponent<IsOnFire>() != null;
					if((!isOnFire || Random.value < 0.005) && Grid.flamesPrefab != null) {
						if(!isOnFire) {
							target.AddComponent<IsOnFire>();
						}
						GameObject flames = (GameObject)Instantiate(Grid.flamesPrefab, hit.point, transform.rotation);
						flames.transform.parent = target.transform;
						Effects.Explode(new GameObject[] {target, flames}, 40);
					}
				}
			}
		}
	}
	
	IEnumerator DeactivateBreath() {
		yield return new WaitForSeconds(5);
		if(durationRemaining <= 0.0f) {
			fireBreath.SetActive(false);
			breathOn = false;
		}
	}

}
