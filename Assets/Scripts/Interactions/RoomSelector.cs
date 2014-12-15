using UnityEngine;
using System.Collections;

public class RoomSelector : MonoBehaviour {

	public enum Selectable { Utopia=0, Dystopia=1, SpaceWorld=2, RealEstate=3 };
	private Selectable currentItem = Selectable.Utopia;
	private float duration = 0.4f;
	private bool currentlyAnimating = false;

	void Update () {
		bool updateMonitors = false;
		if(InputManager.GetAction("CursorLeft")) {
			if((int)currentItem <= 0 || currentlyAnimating) {
				return;
			}
			updateMonitors = true;
			currentItem--;
			StartCoroutine(AnimateMovement(2.0f * Vector3.back));
		}
		else if(InputManager.GetAction("CursorRight")) {
			if((int)currentItem >= 3 || currentlyAnimating) {
				return;
			}
			updateMonitors = true;
			currentItem++;
			StartCoroutine(AnimateMovement(2.0f * Vector3.forward));
		} else if(InputManager.GetAction("Use")) {
			GetComponent<LoadRoom>().SwitchRoom(GetRoomFromSelectable(currentItem));
		}
		if(updateMonitors) {
			UpdateMonitors ();
		}
	}

	void UpdateMonitors() {
		switch(currentItem) {
			
		}
	}

	private LoadRoom.Room GetRoomFromSelectable(Selectable selectable) {
		switch(selectable) {
		case Selectable.Utopia:
			return LoadRoom.Room.Utopia;
		case Selectable.Dystopia:
			return LoadRoom.Room.Dystopia;
		case Selectable.SpaceWorld:
			return LoadRoom.Room.SpaceWorld;
		case Selectable.RealEstate:
			return LoadRoom.Room.RealEstate;
		default:
			return LoadRoom.Room.Menu;
		}
	}

	IEnumerator AnimateMovement(Vector3 delta) {
		currentlyAnimating = true;
		Vector3 initPosition = transform.position;
		Vector3 finalPosition = initPosition + delta;
		for(float lerp = 0.0f; lerp < 1.0f; ) {
			lerp += Time.deltaTime / duration;
			transform.position = Vector3.Lerp(transform.position, finalPosition, lerp);
			yield return 0;
		}
		transform.position = finalPosition;
		currentlyAnimating = false;
	}

}
