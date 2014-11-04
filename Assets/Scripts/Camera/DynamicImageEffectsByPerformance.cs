using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicImageEffectsByPerformance : MonoBehaviour {

	private const bool dynamicEffectsEnabled = true;
	private const float dynamicsEffectsEnabledDelayTime = 1f;
	private int targetFramerateLower = 30;
	private int targetFramerateUpper = 45;
	private int framesPassedBelowFramerate = 0;
	private int framesPassedAboveFramerate = 0;
	private const int framesBeforeUpdate = 60;
	private int step = 0;

	private class ImageEffectPair {
		public Behaviour left, right;
		public ImageEffectPair(Behaviour left, Behaviour right) {
			this.left = left;
			this.right = right;
		}
	}

	void Update () {
		if(!dynamicEffectsEnabled || Time.time < dynamicsEffectsEnabledDelayTime) {
			return;
		}
		Behaviour leftCameraComponent = null, rightCameraComponent = null;
		if(FramerateIndicator.FrameRate < targetFramerateLower) {
			framesPassedAboveFramerate = 0;
			if(framesPassedBelowFramerate > framesBeforeUpdate) {
				ImageEffectPair result = GetImageEffectPairForStep(step);
				if(result != null) {
					Debug.Log ("Disabling camera effect due to low framerate: " + result.left);
					result.left.enabled = false;
					result.right.enabled = false;
					step++;
					framesPassedBelowFramerate = 0;
				}
			} else {
				framesPassedBelowFramerate++;
			}
		} else if(FramerateIndicator.FrameRate > targetFramerateUpper) {
			framesPassedBelowFramerate = 0;
			if(framesPassedAboveFramerate > framesBeforeUpdate && step > 0) {
				ImageEffectPair result = GetImageEffectPairForStep(step - 1);
				if(result != null) {
					Debug.Log ("Reenabling camera effect due to high framerate: " + result.left);
					result.left.enabled = true;
					result.right.enabled = true;
					step--;
					framesPassedAboveFramerate = 0;
				}
			} else {
				framesPassedAboveFramerate++;
			}
		}
	}

	private ImageEffectPair GetImageEffectPairForStep(int step) {
		ImageEffectPair result = null;
		switch(step) {
		case 0:
			result = new ImageEffectPair(Grid.leftCameraObject.GetComponent<SSAOEffect>(), Grid.rightCameraObject.GetComponent<SSAOEffect>());
			break;
		case 1:
			result = new ImageEffectPair((Behaviour)Grid.leftCameraObject.GetComponent("AntialiasingAsPostEffect"), (Behaviour)Grid.rightCameraObject.GetComponent("AntialiasingAsPostEffect"));
			break;
		case 2:
			result = new ImageEffectPair(Grid.leftCameraObject.GetComponent<MotionBlur>(), Grid.rightCameraObject.GetComponent<MotionBlur>());
			break;
		default:
			break;
		}
		return result;
	}
}
