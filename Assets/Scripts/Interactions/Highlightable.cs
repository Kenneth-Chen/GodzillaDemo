using UnityEngine;
using System.Collections;

public class Highlightable : MonoBehaviour {
	
	public string title;
	public Color HighlightColor = Color.white;
	private WireframeBehaviour wireFrame_script;
	private bool active = false;
	private bool wasActive = false;
	private Color originalColor;
	private float lerpDuration = 1.0f;
	private float rotateAmount = 2.0f;
	
	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if(active) {
			UpdateWhileFocused();
		}
		if(!active) {
			if(wasActive) {
				OnLostFocus();
			}
			wasActive = false;
		}
		active = false;
		PostUpdate ();
	}
	
	public void highlight(bool active){
		if(!wasActive && active) {
			OnGainFocus();
		}
		this.active = active;
		wasActive = active;
	}
	
	public virtual void Init() {
		if(renderer != null) {
			originalColor = renderer.material.color;
		}
	}
	
	public virtual void OnGainFocus() {
		originalColor = renderer.material.color;
	}
	
	public virtual void OnLostFocus() {
		if(renderer != null) {
			renderer.material.color = originalColor;
		}
	}
	
	public virtual void UpdateWhileFocused() {
		if(renderer != null) {
			float lerp = Mathf.PingPong (Time.time, lerpDuration) / lerpDuration;
			renderer.material.color = Color.Lerp (Color.black, HighlightColor, lerp);
		}
		//		transform.RotateAround(transform.position, Vector3.up, rotateAmount);
	}
	
	public virtual void PostUpdate() {
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