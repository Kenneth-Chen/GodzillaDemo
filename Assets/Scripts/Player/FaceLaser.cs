#pragma strict
using UnityEngine;
using System.Collections;

public class FaceLaser : MonoBehaviour {
	
	public GameObject gunBarrel;
	private double defaultDelayTime = 0.5;
	private double nextFireTime = 0.0;
	private GameObject target_object;
	private Highlightable target_script;

	void Update ()
	{
		RaycastHit hit;
		if (Physics.Raycast(gunBarrel.transform.position, gunBarrel.transform.forward, out hit, 8.0f))
		{
			target_object = hit.collider.gameObject;
			while(target_object.transform.parent != null && target_object.transform.parent.gameObject.tag != "Terrain") {
				target_object = target_object.transform.parent.gameObject;
			}
			target_script = target_object.GetComponent<Highlightable>();
			if(target_script != null) {
				setHighlighted(true);
				if(InputManager.GetAction("Use")) {
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
		if(InputManager.GetAction("ToggleLaser")) {
			ToggleLaser();
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
			Grid.facelaserText.text = target_script.getTitle();
			if(target_script != null) {
				target_script.highlight(true);
			}
		} else {
			Grid.facelaserText.text = "";
			if(target_script != null) {
				target_script.highlight(false);
			}
		}
	}

	void ToggleLaser() {
		if(Time.time > nextFireTime) {
			gunBarrel.renderer.enabled = !gunBarrel.renderer.enabled;
			nextFireTime = Time.time + defaultDelayTime;
		}
	}

	private void reset()
	{
		setHighlighted (false);
	}
}