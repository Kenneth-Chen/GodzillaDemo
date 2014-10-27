using UnityEngine;
using System.Collections;

public class Highlightable : MonoBehaviour {

	public string title;
	public int destinationNumber;
	public Color HighlightColor = Color.blue;
	private WireframeBehaviour wireFrame_script;
	private bool active = false;
	private Color originalColor;
	private float lerpDuration = 1.0f;

	// Use this for initialization
	void Start () {
		//originalColor = renderer.material.color;
		wireFrame_script = gameObject.AddComponent<WireframeBehaviour>();
		wireFrame_script.LineColor = HighlightColor;
		wireFrame_script.ShowLines = false;
		//wireFrame_script = (WireframeBehaviour) GetComponent(typeof(WireframeBehaviour));
	}
	
	// Update is called once per frame
	void Update () {
		if(active) {
			wireFrame_script.ShowLines = true;
			if(renderer != null) {
				float lerp = Mathf.PingPong (Time.time, lerpDuration) / lerpDuration;
				renderer.material.color = Color.Lerp (originalColor, HighlightColor, lerp);
			}
		}
		if(!active) {
			wireFrame_script.ShowLines = false;
			if(renderer != null) {
				renderer.material.color = originalColor;
			}
		}
		active = false;
	}

	public void highlight(bool active){
		this.active = active;
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
