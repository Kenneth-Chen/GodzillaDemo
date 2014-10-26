using UnityEngine;
using System.Collections;

public class ClipObjects : MonoBehaviour {

	void Start () {
		float[] distances = new float[32];
		distances [8] = 80;
		camera.layerCullDistances = distances;
	}
}
