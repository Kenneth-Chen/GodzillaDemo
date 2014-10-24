using UnityEngine;
using System.Collections;

public class LaserTarget : MonoBehaviour {

	public string title;
	public int destinationNumber;
	public Color HighlightColor = Color.green;
	private WireframeBehaviour wireFrame_script;

	// Use this for initialization
	void Start () {

		wireFrame_script = gameObject.AddComponent<WireframeBehaviour>();
		wireFrame_script.LineColor = HighlightColor;
		wireFrame_script.ShowLines = false;
		//wireFrame_script = (WireframeBehaviour) GetComponent(typeof(WireframeBehaviour));
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void highlight(bool active){
		wireFrame_script.ShowLines = active;


		}

	public void pickUp(GameObject pickerUpper){
		transform.parent = pickerUpper.transform;
		}


	public string getTitle()
	{
		return title;
	}


}
