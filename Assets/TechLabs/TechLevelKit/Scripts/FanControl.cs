using UnityEngine;
using System.Collections;

public class FanControl : MonoBehaviour {
	
	public float speed = 1.0f;
	public Vector3 axis = Vector3.left;
	
	public GameObject particleSystemPrefab;
	
	Transform child;
	
	
	void Start() {
		child = transform.GetChild(0);
		if(particleSystemPrefab != null) {
			var g = Instantiate(particleSystemPrefab) as GameObject;
			g.transform.parent = transform;
			g.transform.localPosition = Vector3.zero;
			g.transform.localRotation = child.localRotation;
			
		}
	}
	
	void Update () {
		child.RotateAround(transform.TransformDirection(axis), Time.deltaTime*speed);
	}
	
}
