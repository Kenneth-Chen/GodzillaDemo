using UnityEngine;
using System.Collections;

public class ClipObjects : MonoBehaviour {

	public float clipObjectDistance = 40.0f;
	void Start () {
		float[] distances = new float[32];
		distances [8] = clipObjectDistance;
		camera.layerCullDistances = distances;
	}
}
