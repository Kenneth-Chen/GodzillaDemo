using UnityEngine;
using System.Collections;

public class Flammable : Highlightable {
	public int burnTimeSeconds = 10;

	public override string getTitle() {
		return ItemManager.CanPlayerSetThingsOnFire () ? "Set on fire (E)" : base.title ;
	}

	public override bool doAction() {
		if(!ItemManager.CanPlayerSetThingsOnFire ()) {
			return false;
		}
		Effects.SetOnFire(this.gameObject, burnTimeSeconds);
		return true;
	}
}
