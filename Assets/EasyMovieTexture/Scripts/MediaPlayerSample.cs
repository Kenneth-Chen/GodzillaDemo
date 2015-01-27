using UnityEngine;
using System.Collections;

public class MediaPlayerSample : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = GetComponent<MediaPlayerCtrl>().GetVideoTexture();
	}
	
	// Update is called once per frame
	void Update () {
	
	}	
	
}
