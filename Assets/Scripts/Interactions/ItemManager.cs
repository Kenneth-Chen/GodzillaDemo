using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	public static bool CanPlayerSetThingsOnFire() {
		foreach(Transform child in Grid.rightHandItemSlot.transform) {
			if(child.gameObject.GetComponent<CanSetThingsOnFire>() != null) {
				return true;
			}
		}
		return false;
	}
}
