using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	public enum Level { Utopia=0, Dystopia=1, Mainframe=2 };
	public Level level;
	float fadeDuration = 3.0f;
	bool loading = false;

	void OnControllerColliderHit(ControllerColliderHit hit) {
		CheckLoadLevel ();
	}

	void OnTriggerEnter(Collider collider) {
		CheckLoadLevel ();
	}

	void CheckLoadLevel() {
		if(loading) {
			return;
		}
		loading = true;
		IEnumerator levelLoader;
		StartCoroutine (LoadLevelAsync(level));
	}

	IEnumerator LoadLevelAsync(Level level) {
		StartCoroutine (FadeScreen ());
		AsyncOperation async = Application.LoadLevelAsync((int)level);
		yield return async;
	}

	IEnumerator FadeScreen() {
		// create a GUITexture:
		GameObject fade = new GameObject();
		fade.AddComponent<GUITexture>();
		// and set it to the screen dimensions:
		fade.guiTexture.pixelInset = new Rect(0, 0, Screen.width, Screen.height);
		// set its texture to a black pixel:
		Texture2D tex= new Texture2D(1, 1);
		tex.SetPixel(0, 0, Color.black);
		tex.Apply();
		fade.guiTexture.texture = tex;
		// then fade it during duration seconds
		for (float alpha = 0.0f; alpha < 1.0f; ){
			alpha += Time.deltaTime / fadeDuration;
			fade.guiTexture.color = new Color(fade.guiTexture.color.r, fade.guiTexture.color.g, fade.guiTexture.color.b, alpha);
			yield return 0;
		}
	}

}
