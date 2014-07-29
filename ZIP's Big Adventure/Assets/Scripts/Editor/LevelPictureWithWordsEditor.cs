using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(LevelPictureWithWords))]
public class LevelPictureWithWordsEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Initialize"))
			((LevelPictureWithWords)target).Initialize();
	}
}
