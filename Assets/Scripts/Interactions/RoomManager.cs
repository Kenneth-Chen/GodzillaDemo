using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour {
	
	public enum Room { Menu=0, Utopia=1, Dystopia=2, SpaceWorld=3, RealEstate=4 };
	public static Room currentRoom = Room.Menu;
	private Room highlightedRoom = Room.Utopia;
	public Material opaqueMaterial, transparentMaterial;
	public Material utopiaSkybox, spaceSkybox;
	private bool needToSwitchBackToMenu = false;

	float fadeDuration = 1.0f;
	float dissectSpeed = 0.8f;
	float bringUpDuration = 1.2f;

	private bool currentlySwitchingRooms = false;

	// whether or not we have initiated a room fade, and are in a waiting loop for that to finish
	private static bool beginFade = false;
	// whether or not we are currently fading the room from one state to another
	private static bool fading = false;

	private float selectorAnimationDuration = 0.4f;
	private bool selectorCurrentlyAnimating = false;

	private GameObject activeMonitorSet, bufferMonitorSet;

	private bool postersVisible = true;

	private AudioSource scrollSound, selectSound, errorSound;

	void Start() {
		activeMonitorSet = Grid.monitorSetA;
		bufferMonitorSet = Grid.monitorSetB;
		AudioSource[] aSources = GetComponents<AudioSource> ();
		scrollSound = aSources [0];
		selectSound = aSources [1];
		errorSound = aSources [2];
	}

	public void SwitchRoom(Room targetRoom) {
		StartCoroutine (SwitchRoomHelper (targetRoom));
	}
	
	IEnumerator SwitchRoomHelper(Room targetRoom)
	{
		Debug.Log (targetRoom);
		if(currentRoom == targetRoom || currentlySwitchingRooms) {
			yield break;
		}
		currentlySwitchingRooms = true;
		Room originalRoom = currentRoom;
		currentRoom = targetRoom;
		bool hideAllWorlds = targetRoom == Room.SpaceWorld || targetRoom == Room.RealEstate;
		// fade in the menu room before doing anything else
		switch (originalRoom) {
		case Room.Menu:
			break;
		case Room.Utopia:
			StartCoroutine(FadeMenuRoom(true));
			break;
		case Room.Dystopia:
			StartCoroutine(FadeMenuRoom(true));
			break;
		case Room.RealEstate:
			StartCoroutine(FadeMenuRoom(true));
			break;
		case Room.SpaceWorld:
			StartCoroutine(FadeMenuRoom(true));
			break;
		}
		if(targetRoom == Room.Menu) {
			// bring the posters back, if they aren't visible
			if(!postersVisible) {
				StartCoroutine(FadeObject(Grid.posterRoom, true, fadeDuration, null));
				StartCoroutine(FadeObject(Grid.posterTwitch, true, fadeDuration, null));
				Grid.posterRoom.GetComponent<Collider>().enabled = true;
				Grid.posterTwitch.GetComponent<Collider>().enabled = true;
				postersVisible = true;
			}
		}
		// wait for menu room to fade before continuing
		if(beginFade) {
			while(fading) {
				yield return 0;
			}
			beginFade = false;
		}
		// transport the old environment away from our position
		switch(originalRoom) {
		case Room.Menu:
			// enable shadows when we're not in the menu room
			Grid.leftCameraObject.GetComponent<SSAOEffect>().enabled = true;
			Grid.rightCameraObject.GetComponent<SSAOEffect>().enabled = true;
			break;
		case Room.Utopia:
			Grid.utopiaWorld.transform.position = -9999 * Vector3.down;
			break;
		case Room.Dystopia:
			Grid.dystopiaWorldLight.GetComponent<Light>().enabled = false;
			Grid.blackShell.renderer.enabled = false;
			Grid.dystopiaWorld.transform.position = -9999 * Vector3.down;
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
		// move the target environment to our location and fade out the menu room
		switch(targetRoom) {
		case Room.Menu:
			// disable shadows when we're not in the menu room
			Grid.leftCameraObject.GetComponent<SSAOEffect>().enabled = false;
			Grid.rightCameraObject.GetComponent<SSAOEffect>().enabled = false;
			needToSwitchBackToMenu = false;
			break;
		case Room.Utopia:
			Grid.utopiaWorld.transform.position = new Vector3(69.76709f, -21.1427f, 103.3317f);
			RenderSettings.skybox = utopiaSkybox;
			StartCoroutine(FadeMenuRoom(false));
//			StartCoroutine (MoveRoom (Grid.utopiaWorld, new Vector3(64.32498f, -49.72105f, 105.2743f), new Vector3(64.32498f, -19.72105f, 105.2743f), hideAllWorlds));
			break;
		case Room.Dystopia:
			Grid.blackShell.renderer.enabled = true;
			Grid.dystopiaWorld.transform.position = new Vector3(119.9163f, 36.40965f, 100.4285f);
			Grid.dystopiaWorldLight.GetComponent<Light>().enabled = true;
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
		currentlySwitchingRooms = false;
		// check to see if we need to initiate a transition back to the menu
		if(needToSwitchBackToMenu && currentRoom != Room.Menu) {
			if(highlightedRoom == currentRoom) {
				needToSwitchBackToMenu = false;
			} else {
				SwitchRoom(Room.Menu);
			}
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
			lerp += Time.deltaTime / fadeDuration;
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

	public delegate void Callback();

	// given a parent object, fade all of its children to either alpha 1 or alpha 0, depending what fadeIn is set to, over a period of time in seconds
	IEnumerator FadeObjects(GameObject parentObj, bool fadeIn, float duration, Color baseColor, Callback callback) {
		// turn on all renderers
		foreach(Transform child in parentObj.transform) {
			child.renderer.enabled = true;
		}
		for(float lerp = 0.0f; lerp < 1.0f; ) {
			lerp += Time.deltaTime / duration;
			float alpha = fadeIn ? lerp : (1.0f - lerp);
			if(alpha < 0.0f) {
				alpha = 0.0f;
			}
			foreach(Transform child in parentObj.transform) {
				child.renderer.material.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
			}
			yield return 0;
		}
		// if we're fading out, turn off the renderer after we're done
		if(!fadeIn) {
			foreach(Transform child in parentObj.transform) {
				child.renderer.enabled = false;
			}
		}
		if(callback != null) {
			callback ();
		}
	}

	IEnumerator FadeObject(GameObject obj, bool fadeIn, float duration, Callback callback) {
		Color baseColor = obj.renderer.material.color;
		for(float lerp = 0.0f; lerp < 1.0f; ) {
			lerp += Time.deltaTime / duration;
			float alpha = fadeIn ? lerp : (1.0f - lerp);
			if(alpha < 0.0f) {
				alpha = 0.0f;
			}
			obj.renderer.material.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
			yield return 0;
		}
		if(callback != null) {
			callback ();
		}
	}
	
	// currently unused
	IEnumerator DissectMenuRoom() {
		for(float deltaY = 0.0f; deltaY < dissectSpeed; ) {
			deltaY += dissectSpeed * Time.deltaTime / fadeDuration;
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

	// moves the object from the given position to the given destination over a period of time, and calls HideAllWorlds() after it's done, if hideAllWorlds set to true
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
	
	void Update () {
		bool cursorMoved = false;
		if(InputManager.GetAction("CursorLeft")) {
			if(selectorCurrentlyAnimating) {
				return;
			}
			if((int)highlightedRoom <= 1) {
				errorSound.Play ();
				return;
			}
			cursorMoved = true;
			highlightedRoom--;
			StartCoroutine(AnimateMovement(2.0f * Vector3.back));
		}
		else if(InputManager.GetAction("CursorRight")) {
			if(selectorCurrentlyAnimating) {
				return;
			}
			if((int)highlightedRoom >= 4) {
				errorSound.Play ();
				return;
			}
			cursorMoved = true;
			highlightedRoom++;
			StartCoroutine(AnimateMovement(2.0f * Vector3.forward));
		} else if(InputManager.GetAction("Select")) {
			if(currentRoom != highlightedRoom && !currentlySwitchingRooms) {
				selectSound.Play ();
				// make the monitors fade
				Color activeMonitorSetBaseColor = bufferMonitorSet.transform.Find ("Monitor-Main").renderer.material.color;
				StartCoroutine(FadeObjects(activeMonitorSet, false, selectorAnimationDuration, activeMonitorSetBaseColor, null));
				// hide the posters
				postersVisible = false;
				StartCoroutine(FadeObject(Grid.posterRoom, false, selectorAnimationDuration, null));
				StartCoroutine(FadeObject(Grid.posterTwitch, false, selectorAnimationDuration, null));
				Grid.posterRoom.GetComponent<Collider>().enabled = false;
				Grid.posterTwitch.GetComponent<Collider>().enabled = false;
				// load the room
				SwitchRoom(highlightedRoom);
			}
		}
		if(cursorMoved) {
			scrollSound.Play();
			UpdateMonitors (highlightedRoom);
			needToSwitchBackToMenu = true;
			SwitchRoom(Room.Menu);
		}
	}
	
	void UpdateMonitors(Room room) {
		switch(room) {
		case Room.Utopia:
			bufferMonitorSet.transform.Find ("Monitor-Preview").renderer.material = Grid.materialMonitorPreviewUtopia;
			bufferMonitorSet.transform.Find ("Monitor-Main").renderer.material = Grid.materialMonitorMainUtopia;
			bufferMonitorSet.transform.Find ("Monitor-Hardware").renderer.material = Grid.materialMonitorHardwareUtopia;
			break;
		case Room.Dystopia:
			bufferMonitorSet.transform.Find ("Monitor-Preview").renderer.material = Grid.materialMonitorPreviewAlien;
			bufferMonitorSet.transform.Find ("Monitor-Main").renderer.material = Grid.materialMonitorMainAlien;
			bufferMonitorSet.transform.Find ("Monitor-Hardware").renderer.material = Grid.materialMonitorHardwareAlien;
			break;
		default:
			bufferMonitorSet.transform.Find ("Monitor-Preview").renderer.material = Grid.materialMonitorDefault;
			bufferMonitorSet.transform.Find ("Monitor-Main").renderer.material = Grid.materialMonitorDefault;
			bufferMonitorSet.transform.Find ("Monitor-Hardware").renderer.material = Grid.materialMonitorDefault;
			break;
		}
		Color activeMonitorSetBaseColor = bufferMonitorSet.transform.Find ("Monitor-Main").renderer.material.color;
		Color bufferMonitorSetBaseColor = bufferMonitorSet.transform.Find ("Monitor-Main").renderer.material.color;
		StartCoroutine(FadeObjects(activeMonitorSet, false, selectorAnimationDuration, activeMonitorSetBaseColor, null));
		StartCoroutine(FadeObjects(bufferMonitorSet, true, selectorAnimationDuration, bufferMonitorSetBaseColor, SwapMonitorSets));
	}

	void SwapMonitorSets() {
		GameObject temp = activeMonitorSet;
		activeMonitorSet = bufferMonitorSet;
		bufferMonitorSet = temp;
	}
	
	IEnumerator AnimateMovement(Vector3 delta) {
		selectorCurrentlyAnimating = true;
		Vector3 initPosition = transform.position;
		Vector3 finalPosition = initPosition + delta;
		for(float lerp = 0.0f; lerp < 1.0f; ) {
			lerp += Time.deltaTime / selectorAnimationDuration;
			transform.position = Vector3.Lerp(transform.position, finalPosition, lerp);
			yield return 0;
		}
		transform.position = finalPosition;
		selectorCurrentlyAnimating = false;
	}
}
