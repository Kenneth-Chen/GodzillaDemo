using UnityEngine;
using System.Collections;

public class LoadRoom : Highlightable {

	public enum Room { AppStore, Twitch };
	public Room targetRoom;
	private static Room currentRoom = Room.AppStore;
#if UNITY_EDITOR
//	private MovieTexture twitchStreamTexture;
#endif

	void Start() {
#if UNITY_EDITOR
//		twitchStreamTexture = (MovieTexture)Grid.twitchVideoFeed.renderer.material.mainTexture;
//		Grid.twitchRoomObject.audio.clip = twitchStreamTexture.audioClip;
//		twitchStreamTexture.loop = true;
//		Grid.twitchRoomObject.audio.loop = true;
#endif
	}

	public override bool doAction ()
	{
		if(currentRoom == targetRoom) {
			return false;
		}
		switch(currentRoom) {
		case Room.Twitch:
#if UNITY_EDITOR
//			twitchStreamTexture.Pause();
//			Grid.twitchRoomObject.audio.Pause();
#elif UNITY_ANDROID
			Grid.twitchVideoManager.GetComponent<MediaPlayerCtrl>().Stop();
#endif
			break;
		}
		switch(targetRoom) {
		case Room.AppStore:
			Grid.roomObject.SetActive(true);
			Grid.twitchRoomObject.SetActive(false);
			break;
		case Room.Twitch:
			Grid.twitchRoomObject.SetActive(true);
			Grid.roomObject.SetActive(false);
#if UNITY_EDITOR
//			twitchStreamTexture.Play();
//			Grid.twitchRoomObject.audio.Play();
#elif UNITY_ANDROID
			Grid.twitchVideoManager.GetComponent<MediaPlayerCtrl>().Play();
#endif
			break;
		}
		currentRoom = targetRoom;
		audio.Play ();
		return true;
	}
}
