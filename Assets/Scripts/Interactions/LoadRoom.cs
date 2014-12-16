using UnityEngine;
using System.Collections;

public class LoadRoom : Highlightable {

	public enum Room { AppStore, Twitch };
	public Room targetRoom;
	private static Room currentRoom = Room.AppStore;
	private MovieTexture twitchStreamTexture;

	void Start() {
		twitchStreamTexture = (MovieTexture)Grid.twitchVideoFeed.renderer.material.mainTexture;
		Grid.twitchRoomObject.audio.clip = twitchStreamTexture.audioClip;
		twitchStreamTexture.loop = true;
		Grid.twitchRoomObject.audio.loop = true;
	}

	public override bool doAction ()
	{
		if(currentRoom == targetRoom) {
			return false;
		}
		switch(currentRoom) {
		case Room.Twitch:
			twitchStreamTexture.Pause();
			Grid.twitchRoomObject.audio.Pause();
			break;
		}
		switch(targetRoom) {
		case Room.AppStore:
			Grid.roomObject.SetActive(true);
			Grid.twitchRoomObject.SetActive(false);
			break;
		case Room.Twitch:
			Grid.roomObject.SetActive(false);
			Grid.twitchRoomObject.SetActive(true);
			twitchStreamTexture.Play();
			Grid.twitchRoomObject.audio.Play();
			break;
		}
		currentRoom = targetRoom;
		audio.Play ();
		return true;
	}
}
