using UnityEngine;
using System.Collections;

public class BoulderCollision : MonoBehaviour {

	void OnCollisionEnter(Collision collision) {
		GameObject obj = collision.collider.gameObject;
		if(obj.tag != "Terrain" && obj.tag != "Player" && obj.tag != "Flames" && obj.tag != "Portal"
		   && obj.GetComponent<TerrainCollider>() == null) {
			if(obj.transform.parent == null) {
				MakeRigid(obj);
			} else {
				GameObject parent = obj.transform.parent.gameObject;
				foreach(Transform childTransform in parent.transform) {
					GameObject child = childTransform.gameObject;
					if(child.tag == "Flames") {
						continue;
					}
					MakeRigid(child);
				}
			}
			Vector3 fwd = transform.TransformDirection (Vector3.forward);
			float speed = this.rigidbody.velocity.magnitude;
			obj.rigidbody.AddForce(fwd * speed, ForceMode.Impulse);

			if(Grid.explosionPrefab != null) {
				GameObject explosion = (GameObject)Instantiate(Grid.explosionPrefab, transform.position, transform.rotation);
				Destroy (explosion, 2);
			}
			if(Grid.flamesPrefab != null && obj.tag != "Ammo" && Random.value < 0.4f) {
				GameObject flames = (GameObject)Instantiate(Grid.flamesPrefab, collision.transform.position, transform.rotation);
				flames.transform.parent = obj.transform;
				Effects.Explode(new GameObject[] {obj, flames}, 40);
			}

			audio.Play();
		}
	}

	void MakeRigid(GameObject obj) {
		Rigidbody rb;
		if(obj.GetComponent<Rigidbody>() == null) {
			rb = obj.AddComponent<Rigidbody> ();
			rb.mass = 10.0f;
			rb.useGravity = true;
		}
	}
	
}
