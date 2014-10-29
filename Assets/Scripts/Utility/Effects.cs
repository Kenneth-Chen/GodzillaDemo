using UnityEngine;
using System.Collections;

public class Effects : Singleton<Effects> {

	// guarantee this will be always a singleton only - can't use the constructor
	protected Effects() {}

	void Awake() {
	}

	public static void SetGlobalFogStartDistance(float distance) {
		GlobalFog[] fogs = Grid.cameraObject.GetComponentsInChildren<GlobalFog> ();
		foreach(GlobalFog fog in fogs) {
			fog.startDistance = distance;
		}
	}

	// sets the garget game object on fire; small chance to add more fire if it's already on fire
	public static void SetOnFire(GameObject target) {
		SetOnFire (target, int.MaxValue);
	}

	// sets the target game object on fire, which will destroy it in the given amount of time; small chance to add more fire if it's already on fire
	public static void SetOnFire(GameObject target, int destroyInSeconds) {
		SetOnFire (target, destroyInSeconds, target.transform.position);
	}

	// sets the target game object on fire, which will destroy it in the given amount of time; small chance to add more fire if it's already on fire
	public static void SetOnFire(GameObject target, int destroyInSeconds, Vector3 flamesPosition) {
		if(target.tag != "Terrain" && target.tag != "Player" && target.tag != "Flames" && target.tag != "Ammo" && target.tag != "Portal"
		   && target.GetComponent<TerrainCollider>() == null) {
			bool isOnFire = target.GetComponent<IsOnFire>() != null;
			if((!isOnFire || Random.value < 0.005) && Grid.flamesPrefab != null) {
				if(!isOnFire) {
					target.AddComponent<IsOnFire>();
				}
				GameObject flames = (GameObject)Instantiate(Grid.flamesPrefab, flamesPosition, target.transform.rotation);
				flames.transform.parent = target.transform;
				if(destroyInSeconds < int.MaxValue) {
					Effects.Explode(new GameObject[] {target, flames}, destroyInSeconds);
				}
			}
		}
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
				Destroy (explosion, 10);
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
