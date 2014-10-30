using UnityEngine;
using System.Collections;

public class AttachCamera : MonoBehaviour
{
	Transform myTransform;
	public Transform target;
	public Vector3 offset = new Vector3(0, 5, -5);
	public float distance = 5;
    public float sensitivityDistance = 5;
    public float damping = 10;
    public float minFOV = 5;
    public float maxFOV = 100;

	void Start()
	{
		myTransform = this.transform;
		distance = camera.fieldOfView;

	}
	
	void FixedUpdate()
	{
		if (target != null)
		{	
			distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityDistance;
			myTransform.position = target.position  + new Vector3(0, distance / 10, -5);// offset;
			myTransform.LookAt(target.position, Vector3.up);
			distance -= Input.GetAxis("Mouse ScrollWheel") * sensitivityDistance;
      	 	distance = Mathf.Clamp(distance, minFOV, maxFOV);
     	  	camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, distance,  Time.deltaTime * damping);
			
		}
	}
}
