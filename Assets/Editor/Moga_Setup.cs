/////////////////////////////////////////////////////////////////////////////////
//
//	Moga_Setup.cs
//	Unity MOGA Plugin for Android/WP8
//	© 2013 Bensussen Deutsch and Associates, Inc. All rights reserved.
//
//	description:	Provides MOGA Controller functionality within Unity.
//					This Script creates the MOGA Setup Editor Window which
//					provides a simple GUI setup for adding MOGA Controls.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System;
using System.IO;

#pragma warning disable 0414

#if UNITY_ANDROID || UNITY_WP8
public class Moga_Setup : EditorWindow 
{	
	Texture 				btnTexture;
	
	private bool 			controllerManagerExists = false;
	
    private static 			Moga_ControllerManager mogaControllerManger;
	
	Moga_Input[] 			inputScripts;
#endif

#if UNITY_WP8
    [MenuItem ("MOGA/WP8/MOGA Controller Setup")]
#elif UNITY_ANDROID
	[MenuItem ("MOGA/Android/MOGA Controller Setup")]
#endif

#if UNITY_ANDROID || UNITY_WP8
    static void Init ()
	{
#endif
#if UNITY_WP8
		// Get existing open window or if none, make a new one:
		Moga_Setup window = (Moga_Setup)EditorWindow.GetWindow (typeof (Moga_Setup), true, "MOGA WP8 Setup");
#elif UNITY_ANDROID
		Moga_Setup window = (Moga_Setup)EditorWindow.GetWindow (typeof (Moga_Setup), true, "MOGA Android Setup");
#endif
#if UNITY_ANDROID || UNITY_WP8
        window.position = new Rect(100,100, 350,540);
		
		window.minSize = new Vector2(350,540);
		window.maxSize = new Vector2(350,540);
		
		window.btnTexture = (Texture)Resources.LoadAssetAtPath("Assets/Editor/Texture_EditorHeader.psd", typeof(Texture));
		
    }

    void OnGUI ()
	{	
		if (btnTexture == null)
		{
			if (GUILayout.Button("\nMOGA\n"))
			{
	            Application.OpenURL ("http://www.mogaanywhere.com/");
			}
		}
		else
		{
			if (GUILayout.Button(btnTexture, GUIStyle.none))
			{
	            Application.OpenURL ("http://www.mogaanywhere.com/");
			}
		}

		if (GameObject.Find("MogaControllerManager") != null)
		{
			controllerManagerExists = true;
			mogaControllerManger = GameObject.Find("MogaControllerManager").GetComponent<Moga_ControllerManager>();
		}

		if (GameObject.Find("MogaControllerManager") != null)
		{
			GUILayout.BeginVertical("box");
			GUILayout.Space (3);
			GUILayout.Label ("Controller Manager", EditorStyles.boldLabel);
			GUILayout.Space (4);
			GUILayout.EndHorizontal();
			if(GUILayout.Button("\nDelete Moga Controller Manager\n"))
			{
				DestroyImmediate(GameObject.Find("MogaControllerManager"));
			}
		}
		else
		{
			GUILayout.BeginVertical("box");
			GUILayout.Space (3);
			GUILayout.Label ("Controller Manager", EditorStyles.boldLabel);
			GUILayout.Space (4);
			GUILayout.EndHorizontal();
			if(GUILayout.Button("\nCreate Moga Controller Manager\n"))
			{
				GameObject mogaControllerObject = new GameObject( "MogaControllerManager" );
				mogaControllerObject.AddComponent<Moga_ControllerManager>();
			}
		}

		GUILayout.Space (12);
		GUILayout.BeginVertical("box");
		GUILayout.Space (3);

		GUILayout.Label ("Input Scripts", EditorStyles.boldLabel);
		GUILayout.Space (4);
		GUILayout.EndHorizontal();
		
		if (Selection.activeTransform != null)
		{
			inputScripts = Selection.activeTransform.gameObject.GetComponents<Moga_Input>();
			GUILayout.Label ("Add Input Script to " + Selection.activeTransform.gameObject.name + "?");
			GUILayout.BeginHorizontal();
			
			if (getInputScript() != null)
			{
				if (GUILayout.Button("\nRemove Moga_Input Script\n"))
				{
		            DestroyImmediate(getInputScript());
				}
			}
			else
			{
				if (GUILayout.Button("\nAttach Moga_Input Script\n"))
				{
		            Selection.activeTransform.gameObject.AddComponent<Moga_Input>();
				}
			}

			GUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.HelpBox ("Select a GameObject to attach the Script to...", MessageType.Info);
		}
#endif
#if UNITY_WP8
		if (GameObject.Find("MogaControllerManager") != null)
		{
			mogaControllerManger = GameObject.Find("MogaControllerManager").GetComponent<Moga_ControllerManager>();
		
			// Additional Settings
			GUILayout.BeginVertical("box");
			GUILayout.Space (3);
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Additional Settings", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
			GUILayout.Label ("optional", EditorStyles.miniLabel);
			GUILayout.EndHorizontal();
			GUILayout.Space (4);
			GUILayout.EndVertical();

			if (!mogaControllerManger.wp8poll)
			{
				EditorGUILayout.HelpBox ("Listening Mode Enabled", MessageType.Info);
			}
			else
			{
				EditorGUILayout.HelpBox ("Polling Mode Enabled", MessageType.Info);
			}
			
			mogaControllerManger.wp8poll = EditorGUILayout.Toggle("Polling Mode", mogaControllerManger.wp8poll);
			
			if (!mogaControllerManger.armlib)
			{
				EditorGUILayout.HelpBox ("Building for x86 -  The x86 library should be used when targeting	a 32-bit Windows Emulator", MessageType.Info);
			}
			else
			{
				EditorGUILayout.HelpBox ("Building for ARM - The ARM library should be used when targeting a physical Windows Phone device.", MessageType.Info);
			}
			
			mogaControllerManger.armlib = EditorGUILayout.Toggle("Build Architecture", mogaControllerManger.armlib);
		}
#endif
#if UNITY_WP8 || UNITY_ANDROID
	}

	public Moga_Input getInputScript()
	{
		inputScripts = Selection.activeTransform.gameObject.GetComponents<Moga_Input>();

		if (inputScripts != null)	// GameObject has one or more Input Script already attached
		{
			foreach (object obj in inputScripts)
  			{
				Moga_Input inputScript = (Moga_Input) obj;	
				return inputScript;
			}
		}
		return null;
	}

	public void OnInspectorUpdate()
	{
    	// This will only get called 10 times per second.
		Repaint();
	}
}
#endif