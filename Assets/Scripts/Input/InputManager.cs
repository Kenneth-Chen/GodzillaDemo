using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// extend Input to handle moga controller
using Input = Moga_Input;

public class InputManager : MonoBehaviour {

	// if this is on, we will write all inputs to the console every frame
	private static bool debug;
	// note that turning on debugging for moga consumes moga button events
	private static bool debugMoga = false;
	// input axes need to exceed this value in order to be registered
	private const float epsilon = 0.005f;
	private static KeyCode lastKeyHit = KeyCode.None;
	private static Moga_ControllerManager mogaManagerScript;

	public static bool GetAction(string action) {
		// Debug.Log (mogaManagerScript.isControllerConnected ());
		switch(action) {
		case "Jump":
			return checkAlternateButton(action, "Y", mogaManagerScript.p1ButtonY);
		case "Use":
		case "Pickup":
			return checkKeyAndButton(KeyCode.E, "A", mogaManagerScript.p1ButtonA);
		case "Select":
			return checkKeyAndButton(KeyCode.Space, "Y", mogaManagerScript.p1ButtonY);
		case "Drop":
			return checkKeyAndButton(KeyCode.Z, "LB", mogaManagerScript.p1ButtonL1);
		case "ToggleLaser":
			return checkKeyAndButton(KeyCode.Backslash, "RB", mogaManagerScript.p1ButtonR1);
		case "CursorLeft":
			return checkKeyAndAxisThresholdLower(KeyCode.Z, "LeftAnalogueHorizontal", mogaManagerScript.p1ButtonL2);
		case "CursorRight":
			return checkKeyAndAxisThresholdUpper(KeyCode.X, "LeftAnalogueHorizontal", mogaManagerScript.p1ButtonR2);
		case "PrimaryAttack":
			return checkKeyAndButton(KeyCode.F, "RightTrigger", mogaManagerScript.p1ButtonR2);
		case "SecondaryAttack":
			return checkKeyAndButton(KeyCode.G, "LeftTrigger", mogaManagerScript.p1ButtonL2);
		case "Suicide":
			return KeyCode.Escape == lastKeyHit;
		default:
			break;
		}
		return Input.GetButton (action);
	}

	public static float GetAxis(string axis) {
		switch(axis) {
		case "Horizontal":
			return checkAxes(new Dictionary<string, bool>() {{axis, false}, {"LeftAnalogueHorizontal", false}});
		case "Vertical":
			return checkAxes(new Dictionary<string, bool>() {{axis, false}, {"LeftAnalogueVertical", false}});
		case "RHorizontal":
			return checkAxes(new Dictionary<string, bool>() {{"RightAnalogueHorizontal", false}});
		case "RVertical":
			return checkAxes(new Dictionary<string, bool>() {{"RightAnalogueVertical", false}});
		default:
			break;
		}
		return Input.GetAxis (axis);
	}

	public static void KeyHit(KeyCode keyCode) {
		InputManager.lastKeyHit = keyCode;
		if(debug) {
			Debug.Log("key: " + keyCode);
		}
	}

	private static bool checkAlternateButton(string button, string altButton, KeyCode mogaKey) {
		if(Input.GetButton(altButton))
			return true;
		if(Input.GetKeyDown(mogaKey))
			return true;
		return Input.GetButton (button);
	}

	private static bool checkKeyAndButton(KeyCode keyCode, string button, KeyCode mogaKey) {
		if(Input.GetButton(button))
			return true;
		if(Input.GetKeyDown(mogaKey))
			return true;
		return keyCode == lastKeyHit;
	}

	private static bool checkKeyAndAxisThresholdUpper(KeyCode keyCode, string axis, KeyCode mogaKey) {
		if(Input.GetAxis(axis) > 0.95f)
			return true;
		if(Input.GetKeyDown(mogaKey))
			return true;
		return keyCode == lastKeyHit;
	}
	
	private static bool checkKeyAndAxisThresholdLower(KeyCode keyCode, string axis, KeyCode mogaKey) {
		if(Input.GetAxis(axis) < -0.95f)
			return true;
		if(Input.GetKeyDown(mogaKey))
			return true;
		return keyCode == lastKeyHit;
	}

	// dictionary of axes: name of axis -> whether or not to invert this axis
	private static float checkAxes(Dictionary<string, bool> axes) {
		foreach(KeyValuePair<string, bool> entry in axes) {
			float result = Input.GetAxis (entry.Key);
			if(result > epsilon || result < -epsilon) {
				return entry.Value ? -result : result;
			}
		}
		return 0.0f;
	}

	void Start() {
		#if UNITY_EDITOR
		debug = false;
		#else
		debug = false;
		#endif
		if (Grid.mogaManagerObject != null)
		{
			// Check the Moga Manager Script is correctly attached to the Moga  Manager Game Object
			mogaManagerScript = Grid.mogaManagerObject.GetComponent<Moga_ControllerManager>();
			if (mogaManagerScript != null)
			{
				Input.RegisterMogaController();			
			}
		}
	}

	void OnGUI() {
		Event e = Event.current;
		if(e.isKey) {
			KeyHit(e.keyCode);
		}
	}

	void LateUpdate() {
		if(debug) {
			DebugInput();
		}
		lastKeyHit = KeyCode.None;
	}

	void DebugInput() {
		string[] buttons = new string[] {
			"A",
			"B",
			"X",
			"Jump",
			"Y",
			"LB",
			"RB",
			"LeftTrigger",
			"RightTrigger",
			"Start",
			"Select"
		};
		foreach(string button in buttons) {
			if(Input.GetButton(button)) {
				Debug.Log(button);
			}
		}
		string[] axes = new string[] {
			"Horizontal",
			"Vertical",
			"LeftAnalogueHorizontal",
			"LeftAnalogueVertical",
			"RightAnalogueHorizontal",
			"RightAnalogueVertical",
			"DPadHorizontal",
			"DPadVertical",
			"Axis7",
			"Axis8",
			"Axis9",
		};
		foreach(string axis in axes) {
			float result = Input.GetAxis(axis);
			if(result > epsilon || result < -epsilon) {
				Debug.Log(axis + ": " + result);
			}
		}
		if(debugMoga) {
			KeyCode[] joystickKeys = new KeyCode[] {
				mogaManagerScript.p1ButtonA,
				mogaManagerScript.p1ButtonB,
				mogaManagerScript.p1ButtonX,
				mogaManagerScript.p1ButtonY,
				mogaManagerScript.p1ButtonL1,
				mogaManagerScript.p1ButtonR1,
				mogaManagerScript.p1ButtonL2,
				mogaManagerScript.p1ButtonR2,
				mogaManagerScript.p1ButtonStart,
				mogaManagerScript.p1ButtonSelect,
				mogaManagerScript.p1ButtonDPadLeft,
				mogaManagerScript.p1ButtonDPadRight,
				mogaManagerScript.p1ButtonDPadUp,
				mogaManagerScript.p1ButtonDPadDown
			};
			foreach(KeyCode keyCode in joystickKeys) {
				if(Input.GetKeyDown(keyCode)) {
					Debug.Log (keyCode);
				}
			}
			string[] mogaAxes = new string[] {
				mogaManagerScript.p1AxisLookHorizontal,
				mogaManagerScript.p1AxisLookVertical
			};
			foreach(string axis in mogaAxes) {
				float result = Input.GetAxis(axis);
				if(result > epsilon || result < -epsilon) {
					Debug.Log(axis + ": " + result);
				}
			}
		}
	}
}
