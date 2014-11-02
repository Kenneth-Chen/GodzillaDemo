using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {

	public Material mat;
	public float dx = 0.000f;
	public float dy = 0.002f;
	private float x, y = 0f;
	private float threshold = 1.0f;
	void Update () {
		x += dx * Random.value;
		y += dy * Random.value;
		if(x > threshold) {
			x -= threshold;
		}
		if(y > threshold) {
			y -= threshold;
		}
		Vector2 offset = new Vector2 (x, y);
		mat.SetTextureOffset ("_MainTex", offset);
	}
}
