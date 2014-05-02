using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(EditorTask))]

class EditorTaskEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Works"))
			((EditorTask)target).Work();
	}
	
}