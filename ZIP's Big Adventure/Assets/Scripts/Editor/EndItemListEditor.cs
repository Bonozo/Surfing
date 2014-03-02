using UnityEditor;
using UnityEngine;
using System.Collections;


[CustomEditor(typeof(EndItemList))]

class EndItemListEditor : Editor {
	
	public override void OnInspectorGUI() {
		
		base.OnInspectorGUI();
		if(GUILayout.Button("Add childs"))
			((EndItemList)target).EditorAddChilds();
	}
	
}