/////////////////////////////////////////////////////////////////////////////////
//
//	Moga_Input.cs
//	Unity MOGA Plugin for Android/WP8
//	© 2013 Bensussen Deutsch and Associates, Inc. All rights reserved.
//
//	description:	Enables MOGA Controller functionality within Unity.  This 
//					Script is a wrapper for Unitys InputManager.
//
/////////////////////////////////////////////////////////////////////////////////

//#define GPS_ENABLED

using UnityEngine;
using System;
using System.Collections.Generic;

public class Moga_Input : MonoBehaviour
{
#if UNITY_WP8 || UNITY_ANDROID
	const float ANALOG_DEADZONE = 0.19f;
	public int controllerID = 1;
	
	// MOGA Controller Manager GameObject
	private static GameObject mogaManager = null;
	
	// MOGA Controller Script

	private static Moga_ControllerManager mogaControllerManager = null;
#endif
#if UNITY_ANDROID
	Moga_Controller mControllerManager;
#endif
#if UNITY_ANDROID || UNITY_WP8
	
	enum ButtonState
	{
		RELEASED,
		PRESSING,	// initial frame of pressed
		PRESSED,
		RELEASING,	// initial frame of released
	};
	
	bool mFocused;

	public static bool keypressedHere = false;
	
	static Dictionary<String, Dictionary<int, float>> mAxes = new Dictionary<String, Dictionary<int, float>> ();

	static Dictionary<int, ButtonState> mogaButtons = new Dictionary<int, ButtonState>();
	static Dictionary<String, int> buttonStrings = new Dictionary<String, int>();
	static Dictionary<KeyCode, int> buttonKeyCodes = new Dictionary<KeyCode, int>();
	
	static bool mAnyKey;
	static bool mAnyKeyDown;
#endif
	
#if UNITY_ANDROID
	// Use this for initialization
	void Start() 
	{
		mControllerManager = GameObject.Find("MogaControllerManager").GetComponent<Moga_ControllerManager>().sMogaController;
	}
#endif
	
#if UNITY_ANDROID
	void Update()
	{
		foreach (Dictionary<int, float> axes in mAxes.Values)
		{
			foreach (int axisKey in new List<int>(axes.Keys))
			{
				axes[axisKey] = mControllerManager.getAxisValue(axisKey);
			}
		}
		
		foreach (int buttonKey in new List<int>(mogaButtons.Keys))
		{
			int action = mControllerManager.getKeyCode(buttonKey);
			
			switch(mogaButtons[buttonKey])
			{
			case ButtonState.RELEASED:
				if(action == Moga_Controller.ACTION_DOWN)
				{
					mogaButtons[buttonKey] = ButtonState.PRESSING;
				}
				break;

			case ButtonState.PRESSING:
				if(action == Moga_Controller.ACTION_UP)
				{
					mogaButtons[buttonKey] = ButtonState.RELEASING;
				}
				else
				{
					mogaButtons[buttonKey] = ButtonState.PRESSED;
				}
				break;
					
			case ButtonState.PRESSED:
				
				mAnyKeyDown = true;
				mAnyKey = true;
				
				if(action == Moga_Controller.ACTION_UP)
				{
					mogaButtons[buttonKey] = ButtonState.RELEASING;
				}
				break;

			case ButtonState.RELEASING:
				
				mAnyKeyDown = false;
				mAnyKey = false;
				
				if(action == Moga_Controller.ACTION_DOWN)
				{
					mogaButtons[buttonKey] = ButtonState.PRESSING;
				}
				else
				{
					mogaButtons[buttonKey] = ButtonState.RELEASED;
				}
				break;
			}
		}
	}
#endif
	
#if UNITY_WP8
	void Update()
	{
		foreach (Dictionary<int, float> axes in mAxes.Values)
		{
			foreach (int axisKey in new List<int>(axes.Keys))
			{
				axes[axisKey] = Moga_Controller.getAxisValue(axisKey);
			}
		}
		
		foreach (int buttonKey in new List<int>(mogaButtons.Keys))
		{
			int action = Moga_Controller.getKeyCode(buttonKey);
				
			switch(mogaButtons[buttonKey])
			{
			case ButtonState.RELEASED:
				if(action == Moga_Controller.ACTION_DOWN)
				{
					mogaButtons[buttonKey] = ButtonState.PRESSING;
				}
				break;
	
			case ButtonState.PRESSING:
				if(action == Moga_Controller.ACTION_UP)
				{
					mogaButtons[buttonKey] = ButtonState.RELEASING;
				}
				else
				{
					mogaButtons[buttonKey] = ButtonState.PRESSED;
				}
				break;
						
			case ButtonState.PRESSED:
					
				mAnyKeyDown = true;
				mAnyKey = true;
					
				if(action == Moga_Controller.ACTION_UP)
				{
					mogaButtons[buttonKey] = ButtonState.RELEASING;
				}
				break;
	
			case ButtonState.RELEASING:
					
				mAnyKeyDown = false;
				mAnyKey = false;
					
				if(action == Moga_Controller.ACTION_DOWN)
				{
					mogaButtons[buttonKey] = ButtonState.PRESSING;
				}
				else
				{
					mogaButtons[buttonKey] = ButtonState.RELEASED;
				}
				break;
			}
		}
	}
#endif
	
#if UNITY_ANDROID || UNITY_WP8
	// ENDREGION
	
	public static void RegisterMogaController()
	{		
		mogaManager = GameObject.Find("MogaControllerManager");
		
		if (mogaManager != null)
		{
			mogaControllerManager = mogaManager.GetComponent<Moga_ControllerManager>();
		}
		
		if (mogaControllerManager == null)
		{
			Debug.Log ("MOGA Controller Manager could not be found.  Access the MOGA Menu to create one!");
		}
		else
		{
			MapController();
		}
	}
	
	// Use the defined MOGAControllerManager Mappings -------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	private static void MapController()
	{
		RegisterInputKey(mogaControllerManager.p1ButtonA, 				Moga_Controller.KEYCODE_BUTTON_A);
		RegisterInputKey(mogaControllerManager.p1ButtonB, 				Moga_Controller.KEYCODE_BUTTON_B);
		RegisterInputKey(mogaControllerManager.p1ButtonX, 				Moga_Controller.KEYCODE_BUTTON_X);
		RegisterInputKey(mogaControllerManager.p1ButtonY, 				Moga_Controller.KEYCODE_BUTTON_Y);
		RegisterInputKey(mogaControllerManager.p1ButtonL1, 				Moga_Controller.KEYCODE_BUTTON_L1);
		RegisterInputKey(mogaControllerManager.p1ButtonR1, 				Moga_Controller.KEYCODE_BUTTON_R1);
		RegisterInputKey(mogaControllerManager.p1ButtonSelect, 			Moga_Controller.KEYCODE_BUTTON_SELECT);
		RegisterInputKey(mogaControllerManager.p1ButtonStart, 			Moga_Controller.KEYCODE_BUTTON_START);
		RegisterInputKey(mogaControllerManager.p1ButtonL3, 				Moga_Controller.KEYCODE_BUTTON_THUMBL);
		RegisterInputKey(mogaControllerManager.p1ButtonR3, 				Moga_Controller.KEYCODE_BUTTON_THUMBR);
		RegisterInputKey(mogaControllerManager.p1ButtonL2, 				Moga_Controller.KEYCODE_BUTTON_L2);
		RegisterInputKey(mogaControllerManager.p1ButtonR2, 				Moga_Controller.KEYCODE_BUTTON_R2);
		RegisterInputKey(mogaControllerManager.p1ButtonDPadUp, 			Moga_Controller.KEYCODE_DPAD_UP);
		RegisterInputKey(mogaControllerManager.p1ButtonDPadDown, 		Moga_Controller.KEYCODE_DPAD_DOWN);
		RegisterInputKey(mogaControllerManager.p1ButtonDPadLeft, 		Moga_Controller.KEYCODE_DPAD_LEFT);
		RegisterInputKey(mogaControllerManager.p1ButtonDPadRight, 		Moga_Controller.KEYCODE_DPAD_RIGHT);
		RegisterInputAxis(mogaControllerManager.p1AxisHorizontal, 		Moga_Controller.AXIS_X);
		RegisterInputAxis(mogaControllerManager.p1AxisVertical, 		Moga_Controller.AXIS_Y);
		RegisterInputAxis(mogaControllerManager.p1AxisLookHorizontal, 	Moga_Controller.AXIS_Z);
		RegisterInputAxis(mogaControllerManager.p1AxisLookVertical, 	Moga_Controller.AXIS_RZ);
		RegisterInputAxis(mogaControllerManager.p1AxisL2, 				Moga_Controller.AXIS_LTRIGGER);
		RegisterInputAxis(mogaControllerManager.p1AxisR2, 				Moga_Controller.AXIS_RTRIGGER);
	}		
	
	// Map the String to an Axis. ---------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static void RegisterInputAxis (String name, int axis)
	{
		Dictionary<int, float> axes;
		if (!mAxes.TryGetValue (name, out axes))
		{
			axes = new Dictionary<int, float> ();
			mAxes.Add (name, axes);
		}
		axes.Add (axis, 0.0f);
	}
	
	// Map the String to a button. --------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static void RegisterInputButton (String name, int button)
	{	
		// Has the String already been added?  If Not...
		if (!buttonStrings.ContainsKey(name))
		{
			buttonStrings.Add(name, button); // Keep Record String was Added
			mogaButtons.Add(button, ButtonState.RELEASED); // Create Moga Button State
		}
	}
	
	// Map the KeyCode to a button. -------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static void RegisterInputKey (KeyCode name, int buttonID)
	{
		// Has the KeyCode already been added?  If Not...
		if (!buttonKeyCodes.ContainsKey(name))
		{
			buttonKeyCodes.Add(name, buttonID); // Keep Record String was Added
			mogaButtons.Add(buttonID, ButtonState.RELEASED); // Create Moga Button State
		}
	}
	
	// REGION Input properties
	public static Vector3 acceleration
	{
		get{return UnityEngine.Input.acceleration;}
	}

	public static int accelerationEventCount
	{
		get{return UnityEngine.Input.accelerationEventCount;}
	}
	
	public static bool anyKey
	{
		get{ return mAnyKey || UnityEngine.Input.anyKey;}
	}

	public static bool anyKeyDown
	{
		get{ return mAnyKeyDown || UnityEngine.Input.anyKeyDown;}
	}

	public static Compass compass
	{
		get{return UnityEngine.Input.compass;}
	}

	public static string compositionString
	{
		get{return UnityEngine.Input.compositionString;}
	}
	
	public static Vector2 compositionCursorPos
	{
		get{return UnityEngine.Input.compositionCursorPos;}
	}

	public static DeviceOrientation deviceOrientation
	{
		get{return UnityEngine.Input.deviceOrientation;}
	}

	public static Gyroscope gyro
	{
		get{return UnityEngine.Input.gyro;}
	}

	public static IMECompositionMode imeCompositionMode
	{
		get{return UnityEngine.Input.imeCompositionMode;}
		set{UnityEngine.Input.imeCompositionMode = value;}
	}

	public static bool imeIsSelected
	{
		get{return UnityEngine.Input.imeIsSelected;}
	}

	public static string inputString
	{
		get{return UnityEngine.Input.inputString;}
	}

	public static Vector3 mousePosition
	{
		get{return UnityEngine.Input.mousePosition;}
	}

	public static bool multiTouchEnabled
	{
		get{return UnityEngine.Input.multiTouchEnabled;}
		set{UnityEngine.Input.multiTouchEnabled = value;}
	}

	public static int touchCount
	{
		get{return UnityEngine.Input.touchCount;}
	}
	
	public static Touch[] touches
	{
		get{return UnityEngine.Input.touches;}
	}
	
	/*
	// ENDREGION
	// REGION Input methods
	
	// Returns any of the STATE_xxx constants. --------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static int GetState(int controllerID, int state)
	{
		return Moga_Controller.getState(1, state);
	}
	
	public static int GetState(int state)
	{
		return Moga_Controller.getState(1, state);
	}
	
	// Returns any of the INFO_xxx constants. ---------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static int getInfo(int controllerID, int info)
	{
		return Moga_Controller.getInfo(1, info);
	}
	
	public static int getInfo(int info)
	{
		return Moga_Controller.getInfo(1, info);
	}
	*/
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static AccelerationEvent GetAccelerationEvent (int index)
	{
		return UnityEngine.Input.GetAccelerationEvent (index);
	}
	
	// Retrieves a Controllers axis. ------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static float GetAxis (String axisName)
	{
		Dictionary<int, float> axes;
		if (mAxes.TryGetValue (axisName, out axes))
		{
			foreach (float axisValue in axes.Values)
			{
				if (Math.Abs (axisValue) > ANALOG_DEADZONE)
				{
					return axisValue;
				}
			}
		}
		return UnityEngine.Input.GetAxis (axisName);
	}
	
	// Retrieves a Controllers axis. ------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static float GetAxisRaw (String axisName)
	{
		Dictionary<int, float> axes;
		if (mAxes.TryGetValue (axisName, out axes))
		{
			foreach (float axisValue in axes.Values)
			{
				if (Math.Abs (axisValue) > ANALOG_DEADZONE)
				{
					return axisValue;
				}
			}
		}
		return UnityEngine.Input.GetAxisRaw (axisName);
	}
	
	// Retrieves a Controllers button State -----------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetButton (String buttonName)
	{
		int buttonID;
		ButtonState buttonState;
		
		// Does this String exist? If so...
		if (buttonStrings.TryGetValue (buttonName, out buttonID)) 
		{		
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.PRESSING:
					case ButtonState.PRESSED:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetButton (buttonName);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetButtonDown (String buttonName)
	{
		int buttonID;
		ButtonState buttonState;
		
		// Does this String exist? If so...
		if (buttonStrings.TryGetValue (buttonName, out buttonID))
		{	
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState))
			{ 	
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.PRESSING:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetButtonDown (buttonName);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetButtonUp (String buttonName)
	{
		int buttonID;
		ButtonState buttonState;
		
		// Does this String exist? If so...
		if (buttonStrings.TryGetValue (buttonName, out buttonID)) 
		{	
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{ 	
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.RELEASING:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetButtonUp (buttonName);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static String[] GetJoystickNames ()
	{
		//return UnityEngine.Input.GetJoystickNames ();
		return null;
	}
	
	// Detect Continuous Key Presses with KeyCode -----------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetKey (KeyCode key)
	{
		int buttonID;
		ButtonState buttonState;
			
		// Does this KeyCode exist? If so...
		if (buttonKeyCodes.TryGetValue (key, out buttonID)) 
		{
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.PRESSING:
					case ButtonState.PRESSED:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKey (key);
	}
	
	// Detect Continuous Key Presses with String ------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetKey (String name)
	{
		int buttonID;
		ButtonState buttonState;
		
		// Does this String exist? If so...
		if (buttonStrings.TryGetValue (name, out buttonID)) 
		{	
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{ 	
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.PRESSING:
					case ButtonState.PRESSED:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKey (name);
	}
	
	// Detect Single Key Press with KeyCode -----------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetKeyDown (KeyCode key)
	{
		int buttonID;
		ButtonState buttonState;
			
		// Does this Key exist? If so...
		if (buttonKeyCodes.TryGetValue (key, out buttonID)) 
		{
			keypressedHere = true;
				
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.PRESSING:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKeyDown (key);
	}

	// Detect Single Key Press with String ------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetKeyDown (String name)
	{
		int buttonID;
		ButtonState buttonState;
		
		// Does this String exist? If so...
		if (buttonStrings.TryGetValue (name, out buttonID)) 
		{	
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{ 		
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.PRESSING:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKeyDown (name);
	}
	
	// Detect Key Release with KeyCode ----------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetKeyUp (KeyCode key)
	{
		int buttonID;
		ButtonState buttonState;
		
		// Does this Key exist? If so...
		if (buttonKeyCodes.TryGetValue (key, out buttonID)) 
		{		
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{ 	
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.RELEASING:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKeyUp (key);
	}
	
	// Detect Key Release with String -----------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetKeyUp (String name)
	{
		int buttonID;
		ButtonState buttonState;
		
		// Does this String exist? If so...
		if (buttonStrings.TryGetValue (name, out buttonID)) 
		{
			// Get Corrosponding buttonID from Moga Dictionary ...
			if (mogaButtons.TryGetValue(buttonID, out buttonState)) 
			{ 	
				switch (buttonState)	// If Button State is Pressed or Pressing...
				{
					case ButtonState.RELEASING:
					return true;
				}
			}
		}
		return UnityEngine.Input.GetKeyUp (name);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetMouseButton (int button)
	{
		return UnityEngine.Input.GetMouseButton (button);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetMouseButtonDown (int button)
	{
		return UnityEngine.Input.GetMouseButtonUp (button);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static bool GetMouseButtonUp (int button)
	{
		return UnityEngine.Input.GetMouseButtonUp (button);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static Touch GetTouch (int index)
	{
		return UnityEngine.Input.GetTouch (index);
	}
	
	// ------------------------------------------------------------------------------------------------
	// ------------------------------------------------------------------------------------------------
	public static void ResetInputAxes ()
	{
		foreach (Dictionary<int, float> axes in mAxes.Values)
		{
			foreach (int axisKey in new List<int>(axes.Keys))
			{
				axes[axisKey] = 0.0f;
			}
		}

		foreach (int buttonKey in new List<int>(mogaButtons.Keys))
		{
			mogaButtons[buttonKey] = ButtonState.RELEASED;
		}

		UnityEngine.Input.ResetInputAxes ();
	}
#endif
	
#if GPS_ENABLED && UNITY_ANDROID || UNITY_WP8
	public static LocationService location
	{
		get{return UnityEngine.Input.location;}
	}
#elif !GPS_ENABLED && UNITY_ANDROID || UNITY_WP8
	public static LocationService location
	{
		get { Debug.LogError("Define GPS_ENABLED to use this property"); return null; }
	}
#endif


}