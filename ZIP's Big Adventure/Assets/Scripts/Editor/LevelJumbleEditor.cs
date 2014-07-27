using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(LevelJumble))]
public class LevelJumbleEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Initialize"))
			((LevelJumble)target).Initialize();
	}
}
