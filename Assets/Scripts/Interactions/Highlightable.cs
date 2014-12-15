﻿using UnityEngine;
using System.Collections;

public class Highlightable : MonoBehaviour {

	public string title;
	public int destinationNumber;
	public Color HighlightColor = Color.white;
	private WireframeBehaviour wireFrame_script;
	private bool active = false;
	private bool wasActive = false;
	private Color originalColor;
	private float lerpDuration = 1.0f;
	private float rotateAmount = 2.0f;

	// Use this for initialization
	void Start () {
		if(renderer != null) {
			originalColor = renderer.material.color;
		}
//		wireFrame_script = gameObject.AddComponent<WireframeBehaviour>();
//		wireFrame_script.LineColor = HighlightColor;
//		wireFrame_script.ShowLines = false;
		//wireFrame_script = (WireframeBehaviour) GetComponent(typeof(WireframeBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
		if(active) {
			// wireFrame_script.ShowLines = true;
			if(renderer != null) {
				float lerp = Mathf.PingPong (Time.time, lerpDuration) / lerpDuration;
				renderer.material.color = Color.Lerp (Color.black, HighlightColor, lerp);
			}
//			transform.RotateAround(transform.position, Vector3.up, rotateAmount);
		}
		if(!active) {
//			wireFrame_script.ShowLines = false;
			if(wasActive) {
				if(renderer != null) {
					renderer.material.color = originalColor;
				}
			}
			wasActive = false;
		}
		active = false;
	}

	public void highlight(bool active){
		if(!wasActive && active) {
			originalColor = renderer.material.color;
		}
		this.active = active;
		wasActive = active;
	}

	public void pickUp(GameObject pickerUpper){
		transform.parent = pickerUpper.transform;
	}

	// text to display when highlighting this object
	public virtual string getTitle()
	{
		return title;
	}

	// action to perform when this object is selected
	public virtual bool doAction()
	{
		return true;
	}
}
