using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadRoom : Highlightable {
	
	public enum Room { Menu, Utopia, Dystopia, SpaceWorld, RealEstate };
	public static Room currentRoom = Room.Menu;
	public Material opaqueMaterial, transparentMaterial;
	public Material utopiaSkybox, spaceSkybox;
	// deprecated, for use only on objects that trigger room loads when you select them
	public Room targetRoom;
	float dissectDuration = 1.0f;
	float dissectSpeed = 0.8f;
	float bringUpDuration = 1.2f;
	// whether or not we have initiated a room fade, and are in a waiting loop for that to finish
	private static bool beginFade = false;
	// whether or not we are currently fading the room from one state to another
	private static bool fading = false;

	public override bool doAction()
	{
		SwitchRoom (this.targetRoom);
		return true;
	}

	public void SwitchRoom(Room targetRoom) {
		StartCoroutine (SwitchRoomHelper (targetRoom));
	}

	IEnumerator SwitchRoomHelper(Room targetRoom)
	{
		if(currentRoom == targetRoom) {
			yield break;
		}
		Room originalRoom = currentRoom;
		currentRoom = targetRoom;
		bool hideAllWorlds = targetRoom == Room.SpaceWorld || targetRoom == Room.RealEstate;
		// fade in the menu room before doing anything else
		switch (originalRoom) {
		case Room.Menu:
			break;
		case Room.Utopia:
		case Room.Dystopia:
		case Room.RealEstate:
		case Room.SpaceWorld:
			StartCoroutine(FadeMenuRoom(true));
			break;
		}
		// wait for menu room to fade before continuing
		if(beginFade) {
			while(fading) {
				yield return 0;
			}
			beginFade = false;
		}
		// transport the target environment to our coordinates
		switch(originalRoom) {
		case Room.Menu:
			Grid.leftCameraObject.GetComponent<SSAOEffect>().enabled = true;
			Grid.rightCameraObject.GetComponent<SSAOEffect>().enabled = true;
			break;
		case Room.Utopia:
			Grid.utopiaWorld.transform.position = new Vector3(64.32498f, -49.72105f, 105.2743f);
			break;
		case Room.Dystopia:
			Grid.blackShell.renderer.enabled = false;
			Grid.dystopiaWorld.transform.position = new Vector3(66.37248f, 0.58784f, 40.3905f);
			break;
		case Room.SpaceWorld:
			break;
		case Room.RealEstate:
			Grid.realEstate.renderer.enabled = false;
			break;
		}
		if(hideAllWorlds) {
			Grid.blackShell.renderer.enabled = false;
			HideAllWorlds();
		}
		// fade out the menu room
		switch(targetRoom) {
		case Room.Menu:
			Grid.leftCameraObject.GetComponent<SSAOEffect>().enabled = false;
			Grid.rightCameraObject.GetComponent<SSAOEffect>().enabled = false;
			break;
		case Room.Utopia:
			Grid.utopiaWorld.transform.position = new Vector3(69.76709f, -21.1427f, 103.3317f);
			Debug.Log (utopiaSkybox);
			RenderSettings.skybox = utopiaSkybox;
			StartCoroutine(FadeMenuRoom(false));
//			StartCoroutine (MoveRoom (Grid.utopiaWorld, new Vector3(64.32498f, -49.72105f, 105.2743f), new Vector3(64.32498f, -19.72105f, 105.2743f), hideAllWorlds));
			break;
		case Room.Dystopia:
			Grid.blackShell.renderer.enabled = true;
			Grid.dystopiaWorld.transform.position = new Vector3(66.37248f, 30.58784f, 40.3905f);
			StartCoroutine(FadeMenuRoom(false));
//			StartCoroutine (MoveRoom (Grid.dystopiaWorld, new Vector3(66.37248f, 0.58784f, 40.3905f), new Vector3(66.37248f, 30.58784f, 40.3905f), hideAllWorlds));
			break;
		case Room.RealEstate:
			Grid.realEstate.renderer.enabled = true;
			StartCoroutine(FadeMenuRoom(false));
			break;
		case Room.SpaceWorld:
			RenderSettings.skybox = spaceSkybox;
			StartCoroutine(FadeMenuRoom(false));
			break;
		}
		// wait for menu room to fade before exiting this coroutine
		if(beginFade) {
			while(fading) {
				yield return 0;
			}
			beginFade = false;
		}
	}

	void HideAllWorlds() {
		Grid.utopiaWorld.transform.position = -9999 * Vector3.down;
		Grid.dystopiaWorld.transform.position = -9999 * Vector3.down;
	}

	IEnumerator FadeMenuRoom(bool fadeIn) {
		beginFade = true;
		fading = true;
		foreach(Transform child in Grid.actualRoomObject.transform) {
			child.renderer.material = transparentMaterial;
		}
		Color baseColor = Grid.ceilingObject.renderer.material.color;
		for(float lerp = 0.0f; lerp < 1.0f; ) {
			lerp += Time.deltaTime / dissectDuration;
			float alpha = fadeIn ? lerp : (1.0f - lerp);
			foreach(Transform child in Grid.actualRoomObject.transform) {
				child.renderer.material.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
			}
			yield return 0;
		}
		if(fadeIn) {
			foreach(Transform child in Grid.actualRoomObject.transform) {
				child.renderer.material = opaqueMaterial;
			}
		}
		fading = false;
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
