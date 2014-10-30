// Tutorial Script for MOGA Unity Android/WP8
// YouTube Tutorial Link: http://youtu.be/YryG2hoeLsI
// ©2013 Bensussen Deutsch and Associates, Inc. All rights reserved.

using UnityEngine;
using System.Collections;

// Extends Unitys inbuilt InputManager with our custom one (Moga_Input)
using Input = Moga_Input; 

public class RotateCube : MonoBehaviour {
	
	private KeyCode						aButtonKeyCode,
										bButtonKeyCode,
										xButtonKeyCode,
										yButtonKeyCode;
	private GameObject 					mogaManagerObject;
	private Moga_ControllerManager 		mogaManagerScript;
	bool mogaFound = false;
	
	// Use this for initialization
	void Start () {
			
		// Try Find our Moga Manager Game Object
		mogaManagerObject = GameObject.Find("MogaControllerManager");
		
		// If it exists..
		if (mogaManagerObject != null)
		{
			// Check the Moga Manager Script is correctly attached to the Moga  Manager Game Object
			mogaManagerScript = mogaManagerObject.GetComponent<Moga_ControllerManager>();
			
			// If it is attached...
			if (mogaManagerScript != null)
			{
				// Register MOGA Controller
				Input.RegisterMogaController();

				// Get our mapped KeyCode Values and assign them.
				aButtonKeyCode = mogaManagerScript.p1ButtonA;
				bButtonKeyCode = mogaManagerScript.p1ButtonB;
				xButtonKeyCode = mogaManagerScript.p1ButtonX;
				yButtonKeyCode = mogaManagerScript.p1ButtonY;
				
				mogaFound = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (mogaFound)
		{
			this.transform.Rotate( 0, -Input.GetAxis("Horizontal"), 0, Space.World );
        	this.transform.Rotate( Input.GetAxis ("Vertical"), 0, 0, Space.World );
		
			if (Input.GetKeyDown(aButtonKeyCode))
			{
				this.renderer.material.color = Color.red;
			}
			else if (Input.GetKeyUp(aButtonKeyCode))
			{
				this.renderer.material.color = Color.white;
			}
			
			if (Input.GetKeyDown(bButtonKeyCode))
			{
				this.renderer.material.color = Color.blue;
			}
			else if (Input.GetKeyUp(bButtonKeyCode))
			{
				this.renderer.material.color = Color.white;
			}
			
			if (Input.GetKeyDown(xButtonKeyCode))
			{
				this.renderer.material.color = Color.yellow;
			}
			else if (Input.GetKeyUp(xButtonKeyCode))
			{
				this.renderer.material.color = Color.white;
			}
			
			if (Input.GetKeyDown(yButtonKeyCode))
			{
				this.renderer.material.color = Color.green;
			}
			else if (Input.GetKeyUp(yButtonKeyCode))
			{
				this.renderer.material.color = Color.white;
			}
		}
	}
}
