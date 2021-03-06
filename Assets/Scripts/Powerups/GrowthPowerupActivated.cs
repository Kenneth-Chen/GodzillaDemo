﻿using UnityEngine;
using System.Collections;

public class GrowthPowerupActivated : MonoBehaviour {

	private float growthFactor = 10.0f;
	private int frames = 60;
	private float startingRadius;
	private float startingHeight;
	private float finalRadius;
	private float finalHeight;
	private int currentFrame = 0;
	private CharacterController cc;
	private DiveFPSController diveFPSController;

	void Start () {
		cc = this.gameObject.GetComponent<CharacterController> ();
		diveFPSController = Grid.playerComponent;
		startingRadius = cc.radius;
		startingHeight = cc.height;
		finalRadius = growthFactor * cc.radius;
		finalHeight = growthFactor * cc.height;
	}
	
	void Update () {
		++currentFrame;
		if(currentFrame > frames) {
			diveFPSController.max_speed = 0.24f;
			diveFPSController.max_speed_air = 0.24f;
			diveFPSController.max_speed_ground = 0.24f;
			diveFPSController.gravity = -0.41f;
			diveFPSController.jumpspeed = 0.28f;
			Effects.SetGlobalFogStartDistance(30.0f);
			Grid.leftCameraObject.transform.localPosition = new Vector3(-0.3f, 0.2f, 0.2f);
			Grid.rightCameraObject.transform.localPosition = new Vector3(0.3f, 0.2f, 0.2f);
			Grid.rightHandItemSlot.transform.localPosition = new Vector3(2.5f, 0.0f, 5.0f);
			Destroy(this);
			return;
		}
		float ratio = (float)currentFrame / (float)frames;
		float currentRadius = ratio * (finalRadius - startingRadius) + startingRadius;
		float currentHeight = ratio * (finalHeight - startingHeight) + startingHeight;
		cc.radius = currentRadius;
		cc.height = currentHeight;
	}
}
