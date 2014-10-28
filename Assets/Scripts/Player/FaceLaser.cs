#pragma strict
using UnityEngine;
using System.Collections;

public class FaceLaser : MonoBehaviour {
	
	public GameObject gunBarrel;
	public GUIText titleTextObject;
	private double defaultDelayTime = 3.0;
	private double nextFireTime = 0.0;
	private GameObject target_object;
	private Highlightable target_script;
	private bool autoSelectEnabled = false;

	void Start ()
	{
		//titleTextObject.text = "hello!";
	}

	void Update ()
	{
		RaycastHit hit;		
		if (Physics.Raycast(gunBarrel.transform.position, gunBarrel.transform.forward, out hit))
		{
			target_object = hit.collider.gameObject;
			while(target_object.transform.parent != null) {
				target_object = target_object.transform.parent.gameObject;
			}
			target_script = target_object.GetComponent<Highlightable>();
			if(target_script != null) {
				setHighlighted(true);
				// if not set, show the box and start timer
				if(nextFireTime == 0.0)
				{
					nextFireTime = Time.time + defaultDelayTime; 
				}
				if(InputManager.GetAction("Use")) {
					actuate();
				}
				// if it's been long enough, actuate
				if(autoSelectEnabled && Time.time > nextFireTime)
				{
					actuate();
				}
			} else {
				// if we lose focus, reset the timer
				reset();
			}
		} else {
			// if we lose focus, reset the timer
			reset();
		}
	}

	void actuate()
	{
		if(target_script != null) {
			target_script.doAction();
		}
		reset ();
	}

	void setHighlighted(bool vis)
	{
		if (vis == true)
		{
			//show title in gui text
			titleTextObject.text = target_script.getTitle();
			if(target_script != null) {
				target_script.highlight(true);
			}
		} else {
			titleTextObject.text = "";
			if(target_script != null) {
				target_script.highlight(false);
			}
		}
	}

	private void reset()
	{
		nextFireTime = 0.0;
		setHighlighted (false);
	}
}