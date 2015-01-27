using UnityEngine;
using System.Collections;

public class FramerateIndicator : MonoBehaviour {
	
	float updateInterval = 2.0f;
	
	private float accum = 0.0f; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	public GUIText guiText;
	public static float FrameRate {
		get {
			return frameRate;
		}
	}
	private static float frameRate = 0;

	void Start()
	{
		Application.targetFrameRate = 500;
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
			frameRate = accum/frames;
			// display two fractional digits (f2 format)
			string strFps = frameRate.ToString("f2");
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
