using UnityEngine;
using System.Collections;
using System.Linq;


[System.Serializable]
public class PrefabReplacement {
	public string childName;
	public GameObject prefab;
}

public class ReplaceSubMeshWithPrefab : MonoBehaviour {
	public PrefabReplacement[] replacements;
	
	
	void Start () {
		foreach(var r in replacements) {
			var child = transform.FindChild(r.childName);
			if(child != null && r.prefab != null) {
				var newChild = Instantiate(r.prefab) as GameObject;
				newChild.transform.position = child.transform.position;
				newChild.transform.rotation = child.transform.rotation;
				newChild.transform.localScale = child.transform.localScale;
				newChild.transform.parent = transform;
				Destroy(child.gameObject);
			}
		}
	}
	
	void Reset() {
		replacements = (from n in (from Transform t in transform select t.gameObject.name).Distinct() select new PrefabReplacement() { childName = n }).ToArray();
	}
}
