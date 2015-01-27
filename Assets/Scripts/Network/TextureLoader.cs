using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;

public class TextureLoader : MonoBehaviour
{
	public string spheres_json_url;
	public static Dictionary<String, WWW> webResources;
	public static Dictionary<String, String> sphereIdToUrlMapping;
	public static JsonData json;

	private static String[] roomSuffixes = { "front.jpg", "back.jpg", "right.jpg", "left.jpg", "top.jpg", "bottom.jpg" };
	private static string[] shaderFaceParams = { "_FrontTex", "_BackTex", "_RightTex", "_LeftTex", "_UpTex", "_DownTex" };		
	
	private String currentRoomId;
	public String CurrentRoomId {
		get {
			return currentRoomId;
		}
	}
	
	void Start()
	{
		StartCoroutine (LoadBuilding ());
	}

	public void NextRoom() {

	}

	public IEnumerator LoadBuilding()
	{
		Debug.Log ("Loading JSON from URL: " + spheres_json_url);
		WWW www = new WWW(spheres_json_url);
		
		//Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
		yield return www;
		if (www.error == null)
		{
			//Sucessfully loaded the JSON string
			Debug.Log("Loaded following JSON string:\n" + www.text);
			
			//Process books found in JSON file
			ProcessJSON(www.text);
		}
		else
		{
			Debug.Log("ERROR: " + www.error);
		}
	}
	
	private void ProcessJSON(string jsonString)
	{
		json = JsonMapper.ToObject(jsonString);
		int numberOfRooms = json ["results"].Count;
		webResources = new Dictionary<string, WWW> (numberOfRooms);
		sphereIdToUrlMapping = new Dictionary<string, string> (numberOfRooms);
		String initial_id = "2";
		for (int i = 0; i<json["results"].Count; i++) {
			JsonData item = json ["results"] [i];
			String id = item["id"].ToString();
			if(initial_id == null) {
				initial_id = id;
			}
			for(int j = 0; j < item["screenshot_images"].Count; j++) {
				JsonData screenshot = item["screenshot_images"][j];
				String url = screenshot["internal_file"].ToString();
				Debug.Log (id + ": " + url);
				if(id != null && url != null) {
//					sphereIdToUrlMapping.Add(id, url);
				}
			}
		}
		StartCoroutine (LoadBox (initial_id));
	}
	
	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle) {
		Vector3 result = angle * ( point - pivot) + pivot;
		Debug.Log ("rotation vector: " + result);
		return result;
	}
	
	public float CalculateRotation(int coordinate, int resolution){
		return 360-(((float)coordinate/(float)resolution) * 360);
	}
	
	IEnumerator DownloadRemoteImageIfNotLoaded(string key, string url) 
	{
		WWW www;
		if (!webResources.TryGetValue (key, out www)) {
			Debug.Log ("downloading image url: " + url);
			//download the image
			www = new WWW(url);
			// wait for it
			yield return www;
			webResources.Add(key, www);
			Debug.Log ("finished download; image url: " + url);
		} else {
			yield return www;
		}
		try {
			print ("accessing image texture (key=" + key + "): " + www.texture);
		} catch (Exception e) {
			Debug.LogException(e);
			StartCoroutine(DownloadRemoteImageIfNotLoaded(key, url));
		}
	}

	public IEnumerator LoadBox(String sphereId)
	{
		currentRoomId = sphereId;
		Debug.Log ("load box: " + sphereId.ToString ());
		JsonData jsonSphereData = GetSphereByID (sphereId);
		for(int i = 0; i < jsonSphereData ["screenshot_images"].Count; i++) {
			JsonData screenshot = jsonSphereData["screenshot_images"][i];
			String url = screenshot["internal_file"].ToString();
			String screenshotId = screenshot["id"].ToString();
			//load and display the image
			String key = sphereId + ";" + screenshotId;
			yield return StartCoroutine(DownloadRemoteImageIfNotLoaded(key, url));
			Debug.Log ("got image for sphereId: " + sphereId);
			WWW www = null;
			if(webResources.TryGetValue(key, out www)) {
				Transform monitorMain = Grid.monitorSetA.transform.Find("Monitor-Main");
				monitorMain.GetComponent<Renderer>().material.SetTexture("_MainTex", www.texture);
			}
		}

		// after image downloaded, set the object's main texture to the download
		Debug.Log ("set skybox for sphereId: " + sphereId);
//		SetSkybox (sphereId);
		
		Debug.Log ("set up portals");
//		SetupPortals (sphereId);
		
//		GameObject player = GameObject.FindWithTag("Player");
//		if (player != null) {
//			Debug.Log("Found player");
//			player.transform.Rotate(new Vector3(1.0f,0.0f,0.0f));
//		}
	}
	
	void SetSkybox(String sphereId) {
		// wait for all the resources to load
		for (int i = 0; i < 6; i++) {
			String key = sphereId + roomSuffixes[i];
			WWW www = null;
			if (!webResources.TryGetValue (key, out www)) {
				continue;
			}
			while (!www.isDone) {
			}
			Debug.Log ("web resource is ready (key: " + key + ";");
		}
		Debug.Log ("all web resources are ready");
		// set the skybox at once
		for (int i = 0; i < 6; i++) {
			String key = sphereId + roomSuffixes[i];
			WWW www = null;
			if (!webResources.TryGetValue (key, out www)) {
				continue;
			}
			while(!www.isDone) {
			}
			Texture2D texture = new Texture2D (1024, 1024);
			www.LoadImageIntoTexture (texture);
			texture.wrapMode = TextureWrapMode.Clamp;
			RenderSettings.skybox.SetTexture (shaderFaceParams[i], texture);
		}
	}
	
	public void SetupPortals(String sphere_id) {
		JsonData jsonSphereData = GetSphereByID (sphere_id);

		// preload each adjacent room
//		String sphereId = portal.destination_id;
//		String portalDestinationUrl;
//		if(!sphereIdToUrlMapping.TryGetValue(sphereId, out portalDestinationUrl)) {
//			continue;
//		}
//		Debug.Log ("preloading image for sphereId: " + sphereId);
	}
	
	public JsonData GetSphereByID(string sphere_id){
		for (int i = 0; i<json["results"].Count; i++) {
			var dict = json["results"][i];
			if(dict["id"].ToString().Equals(sphere_id))
			{
				return dict;
			}
		}
		return null;
	}

	
}
