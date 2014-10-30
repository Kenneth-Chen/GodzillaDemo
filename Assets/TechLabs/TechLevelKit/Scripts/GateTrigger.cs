using UnityEngine;
using System.Collections;

public class GateTrigger : MonoBehaviour {
	public GateControl gate;
	
	void OnTriggerEnter() {
		gate.Open();
	}
	void OnTriggerExit() {
		gate.Close();		
	}
	
}
