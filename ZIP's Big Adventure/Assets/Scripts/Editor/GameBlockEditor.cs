using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(GameBlock))]

class GameBlockEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Init Levels"))
			((GameBlock)target).InitLevels();
	}
	
}