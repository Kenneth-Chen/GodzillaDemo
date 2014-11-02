using UnityEngine;
using System.Collections;

public class ApplyMaterialRecursively : MonoBehaviour {

	public Material material;
	public Material materialTerrain;

	// Use this for initialization
	void Start () {
		ApplyMaterial (transform);
	}
	
	// Update is called once per frame
	void ApplyMaterial (Transform node) {
		foreach(Transform child in node) {
			ApplyMaterial(child);
		}
		if(node.renderer != null && node.gameObject.tag != "Flames" && node.gameObject.tag != "Portal") {
			if(node.gameObject.layer == LayerMask.NameToLayer("Terrain") || node.gameObject.tag == "Terrain") {
				for(int i = 0; i < node.renderer.materials.Length; i++) {
					node.renderer.materials[i].mainTexture = materialTerrain.mainTexture;
				}
				node.renderer.material = materialTerrain;
			} else {
				for(int i = 0; i < node.renderer.materials.Length; i++) {
					node.renderer.materials[i].mainTexture = material.mainTexture;
				}
				node.renderer.material = material;
			}
		}
	}
}
