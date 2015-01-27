using UnityEngine;
using System.Collections;

public class Focusable : Highlightable {

	Color normalAmbientLightColor;
	Color ambientLightTargetColor = Color.black;
	float normalLightbulbIntensity;
	private float timeToFocus = 5.0f;
	float lerp = 1.0f;
	float target = 1.0f;

	public override void Init() {
		normalAmbientLightColor = RenderSettings.ambientLight;
		normalLightbulbIntensity = Grid.lightbulb.GetComponent<Light> ().intensity;
	}
	
	public override void OnGainFocus() {
		target = 0.0f;
	}
	
	public override void OnLostFocus() {
		target = 1.0f;
	}
	
	public override void UpdateWhileFocused() {
	}

	public override void PostUpdate() {
		float delta = Time.deltaTime / timeToFocus;
		if(lerp < target) {
			lerp += delta;
			if(lerp > target) {
				lerp = target;
			}
		} else if (lerp > target) {
			lerp -= delta;
			if(lerp < target) {
				lerp = target;
			}
		} else {
			return;
		}
		Grid.lightbulb.GetComponent<Light> ().intensity = lerp * normalLightbulbIntensity;
		RenderSettings.ambientLight = Color.Lerp (ambientLightTargetColor, normalAmbientLightColor, lerp);
	}
}
