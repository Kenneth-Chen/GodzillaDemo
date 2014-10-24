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

#if UNITY_WP8 || UNITY_ANDROID
public class Moga_Mapping : EditorWindow {
	
	Texture btnTexture;
	
	private bool controllerManagerExists = false;
	
    private static Moga_ControllerManager mogaControllerManger;
	
	[MenuItem ("MOGA/MOGA Controller Mappings")]
	
    static void Init ()
	{
        // Get existing open window or if none, make a new one:
        Moga_Mapping window = (Moga_Mapping)EditorWindow.GetWindow (typeof (Moga_Mapping), true, "MOGA Controller Mappings");

        window.position = new Rect(100, 100, 350, 620);
		
		window.minSize = new Vector2(350, 610);
		window.maxSize = new Vector2(350, 610);
		
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
		
		EditorGUILayout.HelpBox ("Controller Mappings are automatically assigned by Default, you can re-assign the KeyCodes here if required.", MessageType.Info);
		
		if (GameObject.Find("MogaControllerManager") != null)
		{
			controllerManagerExists = true;
			mogaControllerManger = GameObject.Find("MogaControllerManager").GetComponent<Moga_ControllerManager>();
		}
		
		if (controllerManagerExists)
		{
			GUILayout.Label ("Controller Mappings", EditorStyles.boldLabel);
			
			mogaControllerManger.p1ButtonA 				= (KeyCode)EditorGUILayout.EnumPopup ("Button A ", 			mogaControllerManger.p1ButtonA);
			mogaControllerManger.p1ButtonB 				= (KeyCode)EditorGUILayout.EnumPopup ("Button B ", 			mogaControllerManger.p1ButtonB);
			mogaControllerManger.p1ButtonX 				= (KeyCode)EditorGUILayout.EnumPopup ("Button X ", 			mogaControllerManger.p1ButtonX);
			mogaControllerManger.p1ButtonY 				= (KeyCode)EditorGUILayout.EnumPopup ("Button Y ", 			mogaControllerManger.p1ButtonY);
			mogaControllerManger.p1ButtonL1 			= (KeyCode)EditorGUILayout.EnumPopup ("Button L1 ", 		mogaControllerManger.p1ButtonL1);
			mogaControllerManger.p1ButtonL2 			= (KeyCode)EditorGUILayout.EnumPopup ("Button L2 ", 		mogaControllerManger.p1ButtonL2);
			mogaControllerManger.p1ButtonL3 			= (KeyCode)EditorGUILayout.EnumPopup ("Button L3 ", 		mogaControllerManger.p1ButtonL3);
			mogaControllerManger.p1ButtonR1 			= (KeyCode)EditorGUILayout.EnumPopup ("Button R1 ", 		mogaControllerManger.p1ButtonR1);
			mogaControllerManger.p1ButtonR2 			= (KeyCode)EditorGUILayout.EnumPopup ("Button R2 ", 		mogaControllerManger.p1ButtonR2);			
			mogaControllerManger.p1ButtonR3 			= (KeyCode)EditorGUILayout.EnumPopup ("Button R3 ", 		mogaControllerManger.p1ButtonR3);			
			mogaControllerManger.p1ButtonStart 			= (KeyCode)EditorGUILayout.EnumPopup ("Button Start ", 		mogaControllerManger.p1ButtonStart);				
			mogaControllerManger.p1ButtonSelect 		= (KeyCode)EditorGUILayout.EnumPopup ("Button Select ", 	mogaControllerManger.p1ButtonSelect);
			mogaControllerManger.p1ButtonDPadUp 		= (KeyCode)EditorGUILayout.EnumPopup ("Button DPad Up ", 	mogaControllerManger.p1ButtonDPadUp);
			mogaControllerManger.p1ButtonDPadDown 		= (KeyCode)EditorGUILayout.EnumPopup ("Button DPad Down ", 	mogaControllerManger.p1ButtonDPadDown);
			mogaControllerManger.p1ButtonDPadLeft 		= (KeyCode)EditorGUILayout.EnumPopup ("Button DPad Left ", 	mogaControllerManger.p1ButtonDPadLeft);
			mogaControllerManger.p1ButtonDPadRight 		= (KeyCode)EditorGUILayout.EnumPopup ("Button DPad Right ", mogaControllerManger.p1ButtonDPadRight);
			mogaControllerManger.p1AxisHorizontal 		= EditorGUILayout.TextField("Left Nub Horizontal: ", 		mogaControllerManger.p1AxisHorizontal);
			mogaControllerManger.p1AxisVertical 		= EditorGUILayout.TextField("Left Nub Vertical: ", 			mogaControllerManger.p1AxisVertical);
			mogaControllerManger.p1AxisLookHorizontal 	= EditorGUILayout.TextField("Right Nub Horizontal: ", 		mogaControllerManger.p1AxisLookHorizontal);
			mogaControllerManger.p1AxisLookVertical 	= EditorGUILayout.TextField("Right Nub Vertical: ", 		mogaControllerManger.p1AxisLookVertical);
			mogaControllerManger.p1AxisL2 				= EditorGUILayout.TextField("Left Trigger: ", 				mogaControllerManger.p1AxisL2);
			mogaControllerManger.p1AxisR2 				= EditorGUILayout.TextField("Right Trigger: ", 				mogaControllerManger.p1AxisR2);
		}
		else
		{
			EditorGUILayout.HelpBox ("You need to create the Controller Manager first!", MessageType.Warning);
		}
    }
		
	public void OnInspectorUpdate()
	{
    	// This will only get called 10 times per second.
		Repaint();
		
		if (GameObject.Find("MogaControllerManager") == null)
			controllerManagerExists = false;
	}
}
#endif