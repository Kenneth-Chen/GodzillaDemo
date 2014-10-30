using UnityEngine;
using System.Collections;

public class LoadGrid : MonoBehaviour {
	void Awake () {
		Grid.LoadAllGameObjects ();
	}
}
