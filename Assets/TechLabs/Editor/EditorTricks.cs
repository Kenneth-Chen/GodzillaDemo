using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EditorTricks : MonoBehaviour
{

	[MenuItem("Assets/Setup 3DS Lights")]
	 static void SetupLightsFrom3DSMax ()
	{
		foreach (var i in Selection.gameObjects) {
			var parts = i.name.Split ("--".ToCharArray ());
			var name = parts [0];
			var shadows = parts [2] == "true";
			
			var intensity = float.Parse (parts [4]);
			var r = float.Parse (parts [6]);
			var g = float.Parse (parts [8]);
			var b = float.Parse (parts [10]);
			var light = i.AddComponent<Light> ();
			light.color = new Color (r / 255f, g / 255f, b / 255f, 1f);
			light.intensity = intensity;
			light.type = LightType.Point;
			light.shadows = LightShadows.Soft;
			
		}
	}
	
	[MenuItem("Assets/Create/Prefabs From Selected")]
	static void CreatePrefabsFromSelection ()
	{
		foreach (var i in Selection.transforms) {
			var path = "Assets/" + i.gameObject.name + ".prefab";
			var prefab = PrefabUtility.CreateEmptyPrefab (path);
			PrefabUtility.ReplacePrefab (i.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);	
		}
	}
	
	[MenuItem("Assets/Apply Prefabs")]
	static void ApplyPrefabs () {
		foreach (var i in Selection.transforms) {
			PrefabUtility.ReplacePrefab(i.gameObject, PrefabUtility.GetPrefabParent(i.gameObject), ReplacePrefabOptions.ConnectToPrefab);
		}
	}
	
	[MenuItem("Assets/Aggregate Names")]
	static void AggregateNames ()
	{
		Undo.RegisterUndo(Selection.transforms, "Aggregate Names");
		foreach (var i in Selection.transforms) {
			var names = new List<string>();
			GetNames(i, names);
			i.name = string.Join(".", names.ToArray());
		}
	}
	
	
	static void GetNames(Transform t, List<string> names) {
		
		names.Add(t.gameObject.name);
		foreach(Transform child in t) {
			
			GetNames(child, names);
		}
		
	}
	
}
