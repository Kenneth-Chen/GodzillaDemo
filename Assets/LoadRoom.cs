using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadRoom : Highlightable {
	
	public enum Room { Menu, Utopia, Dystopia, SpaceWorld, RealEstate };
	public Room targetRoom;
	public static Room currentRoom = Room.Menu;
	float dissectDuration = 1.0f;
	float dissectSpeed = 0.8f;
	float bringUpDuration = 1.2f;
	bool loading = false;

	public override bool doAction()
	{
		SwitchRoom ();
		return true;
	}
	
	void SwitchRoom()
	{
		if(currentRoom == targetRoom) {
			return;
		}
		bool hideAllWorlds = targetRoom == Room.SpaceWorld || targetRoom == Room.RealEstate;
		switch (currentRoom) {
		case Room.Menu:
			StartCoroutine(DissectMenuRoom());
			break;
		case Room.Utopia:
			StartCoroutine (MoveRoom (Grid.utopiaWorld, Vector3.zero, new Vector3(64.32498f, -49.72105f, 105.2743f), hideAllWorlds));
			break;
		case Room.Dystopia:
			StartCoroutine (MoveRoom (Grid.dystopiaWorld, Vector3.zero, new Vector3(66.37248f, 0.58784f, 40.3905f), hideAllWorlds));
			break;
		case Room.RealEstate:
			Grid.realEstate.renderer.enabled = false;
			Grid.blackShell.renderer.enabled = true;
			Grid.floorObject.renderer.enabled = true;
			break;
		case Room.SpaceWorld:
			Grid.blackShell.renderer.enabled = true;
			Grid.floorObject.renderer.enabled = true;
			break;
		}
		currentRoom = targetRoom;
		if(hideAllWorlds) {
			Grid.blackShell.renderer.enabled = false;
			Grid.floorObject.renderer.enabled = false;
			HideAllWorlds();
		}
		switch(targetRoom) {
		case Room.Utopia:
			StartCoroutine (MoveRoom (Grid.utopiaWorld, new Vector3(64.32498f, -49.72105f, 105.2743f), new Vector3(64.32498f, -19.72105f, 105.2743f), hideAllWorlds));
			break;
		case Room.Dystopia:
			StartCoroutine (MoveRoom (Grid.dystopiaWorld, new Vector3(66.37248f, 0.58784f, 40.3905f), new Vector3(66.37248f, 30.58784f, 40.3905f), hideAllWorlds));
			break;
		case Room.RealEstate:
			Grid.realEstate.renderer.enabled = true;
			break;
		}
	}

	void HideAllWorlds() {
		Grid.utopiaWorld.transform.position = -9999 * Vector3.down;
		Grid.dystopiaWorld.transform.position = -9999 * Vector3.down;
	}

	IEnumerator DissectMenuRoom() {
		for(float deltaY = 0.0f; deltaY < dissectSpeed; ) {
			deltaY += dissectSpeed * Time.deltaTime / dissectDuration;
			foreach (Transform child in Grid.roomObject.transform) {
				if(child.tag != "Terrain") {
					continue;
				}
				child.localPosition += deltaY * Vector3.down;
			}
			Grid.ceilingObject.transform.localPosition += 5.0f * deltaY * Vector3.up;
			yield return 0;
		}
		Grid.ceilingObject.renderer.enabled = false;
		foreach (Transform child in Grid.roomObject.transform) {
			if(child.tag != "Terrain") {
				continue;
			}
			child.renderer.enabled = false;
		}
	}
	
	IEnumerator MoveRoom(GameObject room, Vector3 initPosition, Vector3 finalPosition, bool hideAllWorlds) {
		if(initPosition == Vector3.zero) {
			initPosition = room.transform.position;
		} else {
			room.transform.position = initPosition;
		}
		for(float lerp = 0.0f; lerp < 1.0; ) {
			lerp += Time.deltaTime / bringUpDuration;
			room.transform.position = Vector3.Lerp(initPosition, finalPosition, lerp);
			yield return 0;
		}
		if(hideAllWorlds) {
			HideAllWorlds();
		}
	}
}
