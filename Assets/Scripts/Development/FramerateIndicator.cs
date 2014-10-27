using UnityEngine;
using System.Collections;

public class FramerateIndicator : MonoBehaviour {
	
	float updateInterval = 0.5f;
	
	private float accum = 0.0f; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	public GUIText guiText;

	void Start()
	{
		timeleft = updateInterval;  
	}
	
	void Update()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale/Time.deltaTime;
		++frames;
		
		// Interval ended - update GUI text and start new interval
		if( timeleft <= 0.0f )
		{
			// display two fractional digits (f2 format)
			string strFps = (accum/frames).ToString("f2");
			Debug.Log ("fps: " + strFps);
			if(guiText != null) {
				guiText.text = strFps;
			}
			timeleft = updateInterval;
			accum = 0.0f;
			frames = 0;
		}
	}

}
