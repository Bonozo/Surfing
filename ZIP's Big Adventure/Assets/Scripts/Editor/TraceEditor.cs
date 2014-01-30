using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(Trace))]

class TraceEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Index"))
			((Trace)target).IndexEditor();
	}
	
}