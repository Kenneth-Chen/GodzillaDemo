using UnityEngine;
using System.Collections;

public class LoadRoom : Highlightable {

	public enum Room { AppStore, Twitch };
	public Room targetRoom;

	public override bool doAction ()
	{
		switch(targetRoom) {
		case Room.AppStore:
			Grid.roomObject.SetActive(true);
			Grid.twitchRoomObject.SetActive(false);
			break;
		case Room.Twitch:
			Grid.roomObject.SetActive(false);
			Grid.twitchRoomObject.SetActive(true);
			break;
		}
		return true;
	}
}
