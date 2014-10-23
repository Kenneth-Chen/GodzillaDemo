#pragma strict
using UnityEngine;
using System.Collections;

public class FaceLaser : MonoBehaviour {
	
	public GameObject gunBarrel;
	public GameObject cameraObject;
	public GUIText titleTextObject;
	private double defaultDelayTime = 3.0;
	private double nextFireTime = 0.0;
	private GameObject target_object;
	private LaserTarget target_script;
	//private Texture2D door_green = Resources.Load("door_green_bg") as Texture2D;
	//private Texture2D door_trans = Resources.Load("door_trans_bg") as Texture2D;
	
	//Use this for initilization
	void Start ()
	{
		print ("Facelaser start");
		titleTextObject.text = "hello!";
	}
	
	//Update is called once per frame
	void Update ()
	{
		Fire();
	}
	
	void actuate()
	{
		print("jumpLocation : "+target_object.name);
		action();
		resetTarget ();
	}
	void action()
	{
		//Portal doorway_portal = (Portal) target_object.GetComponent(typeof(Portal));
		//doorway_portal.teleport();
	}
	void set_visibility(bool vis)
	{

		if (vis == true) 
		{
			//show title in gui text
			titleTextObject.text = target_script.getTitle();
			target_script.highlight(true);
		} else {
			titleTextObject.text = "";
			target_script.highlight(false);
		}
	}
	private void resetTarget()
	{
		if (target_object != null) 
		{
			//print("resetDoorway : "+doorway.name);
			set_visibility(false);

		}
	}
	private void resetTimer()
	{
		nextFireTime = 0.0;
	}
	
	private void Fire()
	{
		//add reload delay for fire - no button mashing : )
		
		RaycastHit hit;
		
		if (Physics.Raycast(gunBarrel.transform.position, gunBarrel.transform.forward, out hit))
		{
			
			if(hit.collider.tag == "LaserTarget")
			{
				print ("Target hit");
				target_object = hit.collider.gameObject;
				target_script = (LaserTarget) target_object.GetComponent(typeof(LaserTarget));

				//doorway.renderer.material.color = new Color( 0, 0, 255, 0 );
				
				
				//if not set, show the box and start timer
				if(nextFireTime == 0.0)
				{
					//print("Hit : "+doorway.name);
					nextFireTime = Time.time + defaultDelayTime; 
					float alphaDelta = (float)(Time.deltaTime/defaultDelayTime);
					set_visibility(true);
				}
				//if it's been long enough, go through the door
				if(Time.time > nextFireTime)
				{
					//cameraObject.transform.position = new Vector3(5, 0, 0);
					actuate();
				}
			}else{
				resetTimer();
				resetTarget();}
			
		}else{
			//if not hitting the doorway, hide the doorway and reset timer
			resetTimer();
			resetTarget();
			
		}
	}
}