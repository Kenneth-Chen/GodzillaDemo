using UnityEngine;
using System;

static class Grid
{
	public static UnityEngine.Object explosionPrefab;
	public static UnityEngine.Object flamesPrefab;
	public static UnityEngine.Object boulderPrefab;

	public static GameObject playerObject;
	public static GameObject playerModel;
	public static DiveFPSController playerComponent;
	public static GameObject mogaManagerObject;
	public static GameObject cameraObject;
	public static GameObject leftCameraObject;
	public static GameObject rightCameraObject;
	public static GameObject rightHandItemSlot;
	public static GameObject fireBreath;
	public static GUIText facelaserText;
	public static GameObject lightbulb;
	public static GameObject roomObject;
	public static GameObject monitorSetA, monitorSetB;
	public static GameObject actualRoomObject;
	public static GameObject ceilingObject;
	public static GameObject utopiaWorld;
	public static GameObject dystopiaWorld;
	public static GameObject dystopiaWorldLight;
	public static GameObject blackShell;
	public static GameObject realEstate;
	public static GameObject twitchRoomObject;
	public static GameObject twitchVideoFeed;
	public static GameObject posterRoom, posterTwitch;
	public static Material materialMonitorDefault;
	public static Material materialMonitorPreviewUtopia;
	public static Material materialMonitorMainUtopia;
	public static Material materialMonitorHardwareUtopia;
	public static Material materialMonitorPreviewAlien;
	public static Material materialMonitorMainAlien;
	public static Material materialMonitorHardwareAlien;

	static Grid()
	{
		explosionPrefab = LoadPrefab ("Effects/Explosion");
		flamesPrefab = LoadPrefab ("Effects/Flames");
		boulderPrefab = LoadPrefab ("Weapons/BoulderWeapon");
	}

	public static void LoadAllGameObjects() {
		GameObject o;
		mogaManagerObject = SafeFind ("/MogaControllerManager");
		playerObject = SafeFind ("/Player");
		playerModel = SafeFind ("/Player/PlayerModel");
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
		lightbulb = SafeFind ("/Lightbulb");
		roomObject = SafeFind ("/Room");
		monitorSetA = SafeFind ("/Room/MonitorSetA");
		monitorSetB = SafeFind ("/Room/MonitorSetB");
		actualRoomObject = SafeFind ("/Room/ActualRoom");
		ceilingObject = SafeFind ("/Room/ActualRoom/Ceiling");
		utopiaWorld = SafeFind ("/UtopiaWorld");
		dystopiaWorld = SafeFind ("/DystopiaWorld");
		dystopiaWorldLight = SafeFind ("/DystopiaWorld/Directional light");
		blackShell = SafeFind ("/BlackShell");
		realEstate = SafeFind ("/RealEstate");
		twitchRoomObject = SafeFind ("/RoomTwitch");
		twitchVideoFeed = SafeFind ("/RoomTwitch/TwitchVideoFeed");
		if(twitchRoomObject != null) {
			twitchRoomObject.SetActive(false);
		}
		posterRoom = SafeFind ("/PosterRoom");
		posterTwitch = SafeFind ("/PosterTwitch");
		materialMonitorDefault = LoadMaterial ("Materials/Monitor-Default");
		materialMonitorPreviewUtopia = LoadMaterial ("Materials/Monitor-Polyworld-Preview");
		materialMonitorMainUtopia = LoadMaterial ("Materials/Monitor-Polyworld-Main");
		materialMonitorHardwareUtopia = LoadMaterial ("Materials/Monitor-Polyworld-Hardware");
		materialMonitorPreviewAlien = LoadMaterial ("Materials/Monitor-Alien-Preview");
		materialMonitorMainAlien = LoadMaterial ("Materials/Monitor-Alien-Main");
		materialMonitorHardwareAlien = LoadMaterial ("Materials/Monitor-Alien-Hardware");
	}

	private static Material LoadMaterial(string s)
	{
		Material o = (Material) Resources.Load (s, typeof(Material));
		if ( o == null ) Debug.LogError("Could not find Object " +s);
		return o;
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
