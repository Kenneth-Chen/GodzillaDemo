using UnityEngine;
using System;

static class Grid
{
	public static UnityEngine.Object explosionPrefab;
	public static UnityEngine.Object flamesPrefab;
	public static UnityEngine.Object boulderPrefab;

	public static GameObject playerObject;
	public static DiveFPSController playerComponent;
	public static GameObject mogaManagerObject;
	public static GameObject cameraObject;
	public static GameObject leftCameraObject;
	public static GameObject rightCameraObject;
	public static GameObject rightHandItemSlot;
	public static GameObject fireBreath;
	public static GUIText facelaserText;

	static Grid()
	{
		explosionPrefab = LoadPrefab("Effects/Explosion");
		flamesPrefab = LoadPrefab("Effects/Flames");
		boulderPrefab = LoadPrefab ("Weapons/BoulderWeapon");
		LoadAllGameObjects ();
	}

	public static void LoadAllGameObjects() {
		GameObject o;
		mogaManagerObject = SafeFind ("/MogaControllerManager");
		playerObject = SafeFind ("/Player");
		cameraObject = SafeFind ("/Player/Dive_Camera");
		leftCameraObject = SafeFind ("/Player/Dive_Camera/Camera_left");
		rightCameraObject = SafeFind ("/Player/Dive_Camera/Camera_right");
		playerComponent = SafeComponent<DiveFPSController> (playerObject, "DiveFPSController");
		rightHandItemSlot = SafeFind ("/Player/Dive_Camera/RightHandItemSlot");
		fireBreath = SafeFind ("/Player/Dive_Camera/FireBreath");
		// todo: fix so that we can find inactive objects
		if(fireBreath != null) {
			fireBreath.SetActive (false);
		}
		o = SafeFind ("/FaceLaserText");
		facelaserText = SafeComponent<GUIText> (o, "GUIText");
	}

	private static UnityEngine.Object LoadPrefab(string s)
	{
		UnityEngine.Object o = Resources.Load (s);
		if ( o == null ) Debug.LogError("Could not find Object " +s);
		return o;
	}

	private static GameObject SafeFind(string s)
	{
		GameObject g = GameObject.Find(s);
		if ( g == null ) Debug.LogError("Could not find GameObject " +s);
		return g;
	}
	
	private static T SafeComponent<T>(GameObject g, string s)
	{
		if(g == null) {
			Debug.LogError ("Could not find Component " + s + " because GameObject is null");
			return default(T);
		}
		Component c = g.GetComponent(s);
		if (c == null) {
			Debug.LogError("Could not find Component " +s);
			return default(T);
		}
		if (c.GetType() != typeof(T)) Debug.LogError ("Type mismatch when finding Component " + s + ": expected " + typeof(T).ToString() + " but got " + c.GetType());
		return (T)Convert.ChangeType(c, typeof(T));
	}
}
