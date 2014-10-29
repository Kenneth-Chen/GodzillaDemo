using UnityEngine;
using System.Collections;
// extend Input to handle moga controller
using Input = Moga_Input;

public class InputManager : MonoBehaviour {

	// if this is on, we will write all inputs to the console every frame
#if UNITY_EDITOR
	private const bool debug = false;
#else
	private const bool debug = true;
#endif
	// input axes need to exceed this value in order to be registered
	private const float epsilon = 0.005f;

	private static KeyCode lastKeyHit = KeyCode.None;

	// for moga
	private KeyCode		aButtonKeyCode,
						bButtonKeyCode,
						xButtonKeyCode,
						yButtonKeyCode;

	public static bool GetAction(string action) {
		switch(action) {
		case "Jump":
			return checkAlternateButton(action, "Y");
		case "Use":
		case "Pickup":
			return checkKeyAndButton(KeyCode.E, "A");
		case "Drop":
			return checkKeyAndButton(KeyCode.Z, "LB");
		case "PrimaryAttack":
			return checkKeyAndButton(KeyCode.F, "RightTrigger");
		case "SecondaryAttack":
			return checkKeyAndButton(KeyCode.G, "LeftTrigger");
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
			return checkAlternateAxis(axis, "LeftAnalogueHorizontal");
		case "Vertical":
			return checkAlternateAxis(axis, "LeftAnalogueVertical");
		default:
			break;
		}
		return Input.GetAxis (axis);
	}

	public static void KeyHit(KeyCode keyCode) {
		InputManager.lastKeyHit = keyCode;
	}

	private static bool checkAlternateButton(string button, string altButton) {
		if(Input.GetButton(altButton))
			return true;
		return Input.GetButton (button);
	}

	private static bool checkKeyAndButton(KeyCode keyCode, string button) {
		if(Input.GetButton(button))
			return true;
		return keyCode == lastKeyHit;
	}

	private static float checkAlternateAxis(string axis, string altAxis) {
		return checkAlternateAxis (axis, altAxis, false);
	}

	private static float checkAlternateAxis(string axis, string altAxis, bool invertAltAxis) {
		float result = Input.GetAxis (altAxis);
		if(result > epsilon || result < -epsilon) {
			return invertAltAxis ? -result : result;
		}
		return Input.GetAxis (axis);
	}

	void Start() {
		// If it exists..
		if (Grid.mogaManagerObject != null)
		{
			// Check the Moga Manager Script is correctly attached to the Moga  Manager Game Object
			Moga_ControllerManager mogaManagerScript = Grid.mogaManagerObject.GetComponent<Moga_ControllerManager>();
			
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
		lastKeyHit = KeyCode.None;
		if(debug) {
			DebugInput();
		}
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
	}
}
