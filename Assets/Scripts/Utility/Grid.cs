using UnityEngine;

static class Grid
{
	public static Object explosionPrefab;
	public static Object flamesPrefab;
	public static Object boulderPrefab;

	static Grid()
	{
		explosionPrefab = LoadPrefab("Effects/Explosion");
		flamesPrefab = LoadPrefab("Effects/Flames");
		boulderPrefab = LoadPrefab ("Weapons/BoulderWeapon");
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
		Component c = g.GetComponent(s);
		if ( c == null ) Debug.LogError("Could not find Component " +s);
		return c;
	}
}
