using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(LevelPath))]

class LevelPathEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Initialize"))
			((LevelPath)target).Initialize();
	}
	
}