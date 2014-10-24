using UnityEngine;
using System.Collections;

public class Effects : Singleton<Effects> {

	// guarantee this will be always a singleton only - can't use the constructor
	protected Effects() {}

	void Awake() {
	}

	// destroys all GameObject in the given array, and instantiates an explosion at the position of the first object in the array
	public static void Explode(GameObject[] objs, int seconds) {
		Effects.Instance.StartCoroutine (Effects.Instance.ExplodeHelper (objs, seconds));
	}

	IEnumerator ExplodeHelper(GameObject[] objs, int seconds) {
		yield return new WaitForSeconds (seconds);
		if(objs.Length > 0) {
			if(objs[0] != null) {
				GameObject explosion = (GameObject)Instantiate(Grid.explosionPrefab, objs[0].transform.position, objs[0].transform.rotation);
				Destroy (explosion, 5);
			}
			foreach(GameObject obj in objs) {
				if(obj == null) {
					continue;
				}
				Destroy (obj);
			}
		}
	}
}
