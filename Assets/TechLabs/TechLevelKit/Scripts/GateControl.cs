using UnityEngine;
using System.Collections;

public class GateControl : MonoBehaviour
{
	public bool addTrigger = true;
	public Transform leftDoor;
	public Transform rightDoor;
	public float travelDistance = 2.7f;
	public float time = 1;
	public Vector3 direction = Vector3.up;
	Vector3 closedLeft, closedRight;
	Vector3 openLeft, openRight;
	public bool open = false;
	public bool inTransition = false;
	public AudioClip soundfxOpen;
	public AudioClip soundfxClose;
	
	void Reset ()
	{
		if (transform.childCount > 0) 
			leftDoor = transform.GetChild (0);
		if (transform.childCount > 1) 
			rightDoor = transform.GetChild (1);
	}
	
	void Start ()
	{
		if (leftDoor) {
			closedLeft = leftDoor.position;
			openLeft = leftDoor.position + (transform.TransformDirection (direction) * -travelDistance);
		}
		if (rightDoor) {
			openRight = rightDoor.position + (transform.TransformDirection (direction) * travelDistance);
			closedRight = rightDoor.position;
		}
		if(addTrigger) {
			var sc = new GameObject("Trigger", typeof(SphereCollider)).GetComponent<SphereCollider>();
			sc.radius = 2;
			sc.isTrigger = true;
			sc.transform.parent = this.transform;
			sc.transform.position = transform.position + Vector3.up;
			var gt = sc.gameObject.AddComponent<GateTrigger>();
			gt.gate = this;
		}
		
	}
	
	[ContextMenu("Remove Children")]
	public void RemoveChildren ()
	{
		foreach (var c in GetComponentsInChildren<Transform>()) {
			if (c.gameObject != gameObject)
				DestroyImmediate (c.gameObject);	
		}
	}
	
	[ContextMenu("Open")]
	public void Open ()
	{
		if (!inTransition) {
			inTransition = true;
			StartCoroutine (_Open ());
		}
	}
	
	[ContextMenu("Close")]
	public void Close ()
	{
		if (!inTransition) {
			inTransition = true;
			StartCoroutine (_Close ());
		}
	}
	
	IEnumerator _Open ()
	{
		var T = 0f;
		if (audio != null && soundfxOpen != null) {
			audio.PlayOneShot (soundfxOpen);	
		}
		while (T <= 1f) {
			T += Time.deltaTime / time;
			var p = Mathf.SmoothStep (0, 1, T);
			if (leftDoor)
				leftDoor.position = Vector3.Lerp (leftDoor.position, openLeft, p);
			if (rightDoor)
				rightDoor.position = Vector3.Lerp (rightDoor.position, openRight, p);
			yield return null;
		}
		open = true;
		inTransition = false;
	}
	
	IEnumerator _Close ()
	{
		var T = 0f;
		if (audio != null && soundfxClose != null) {
			audio.PlayOneShot (soundfxClose);	
		}
		while (T <= 1f) {
			T += Time.deltaTime / time;
			var p = Mathf.SmoothStep (0, 1, T);
			if (leftDoor)
				leftDoor.position = Vector3.Lerp (leftDoor.position, closedLeft, p);
			if (rightDoor)
				rightDoor.position = Vector3.Lerp (rightDoor.position, closedRight, p);
			yield return null;
		}
		open = false;
		inTransition = false;
				
	}
	
	

	
}
