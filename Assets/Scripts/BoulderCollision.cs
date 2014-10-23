using UnityEngine;
using System.Collections;

public class BoulderCollision : MonoBehaviour {

	public GameObject prefabExplosion;
	public GameObject prefabFlames;

	void OnCollisionEnter(Collision collision) {
		GameObject obj = collision.collider.gameObject;
		// Debug.Log (collision.collider.gameObject);
		if(obj.tag != "Terrain" && obj.tag != "Player") {
			Rigidbody rb;
			if(obj.GetComponent<Rigidbody>() == null) {
				rb = obj.AddComponent<Rigidbody> ();
				rb.mass = 10.0f;
				rb.useGravity = true;
			} else {
				rb = obj.GetComponent<Rigidbody>();
			}
			Vector3 fwd = transform.TransformDirection (Vector3.forward);
			float speed = 5.0f;
			rb.AddForce(fwd * speed, ForceMode.Impulse);
			if(prefabExplosion != null) {
				GameObject explosion = (GameObject)Instantiate(prefabExplosion, transform.position, transform.rotation);
				Destroy (explosion, 5);
			}
			if(obj.tag != "Ammo" && Random.value < 0.2f) {
				GameObject flames = (GameObject)Instantiate(prefabFlames, collision.transform.position, transform.rotation);
				flames.transform.parent = obj.transform;
				Destroy (flames, 60);
				Destroy (obj, 60);
			}

			audio.Play();
		}
	}

}
