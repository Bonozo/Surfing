using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(LevelRightSequenceButton))]
public class LevelRightSequenceButtonEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Initialize"))
			((LevelRightSequenceButton)target).Initialize();
	}
}
