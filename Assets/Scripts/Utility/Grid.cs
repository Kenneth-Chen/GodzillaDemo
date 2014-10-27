using UnityEngine;

static class Grid
{
	public static Object explosionPrefab;
	public static Object flamesPrefab;
	public static Object boulderPrefab;
	public static GameObject playerObject;
	public static DiveFPSController playerComponent;
	public static GameObject cameraObject;
	public static GameObject rightHandItemSlot;

	static Grid()
	{
		explosionPrefab = LoadPrefab("Effects/Explosion");
		flamesPrefab = LoadPrefab("Effects/Flames");
		boulderPrefab = LoadPrefab ("Weapons/BoulderWeapon");
		cameraObject = SafeFind ("Dive_Camera");
		playerObject = SafeFind ("Player");
		playerComponent = (DiveFPSController)SafeComponent (playerObject, "DiveFPSController");
		rightHandItemSlot = SafeFind ("RightHandItemSlot");
	}

	private static Object LoadPrefab(string s)
	{
		Object o = Resources.Load (s);
		if ( o == null ) Debug.LogError("Could not find Object " +s);
		return o;
	}

	private static GameObject SafeFind(string s)
	{
		GameObject g = GameObject.Find(s);
		if ( g == null ) Debug.LogError("Could not find GameObject " +s);
		return g;
	}

	private static Component SafeComponent(GameObject g, string s)
	{
		if(g == null) {
			Debug.LogError ("Could not find Component " + s + " because GameObject is null");
			return null;
		}
		Component c = g.GetComponent(s);
		if (c == null) {
			Debug.LogError("Could not find Component " +s);
		}
		return c;
	}
}
