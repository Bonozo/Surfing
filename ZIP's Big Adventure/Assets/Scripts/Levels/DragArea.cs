using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIPanel))]
public class DragArea : MonoBehaviour {

	void Awake()
	{
		// Setting up drag area parameters
		var panel = GetComponent<UIPanel> ();
		float aspectRation = (float)Screen.width / (float)Screen.height;
		Vector4 clipRange = panel.clipRange;
		float currentRation = clipRange.z / clipRange.w;
		clipRange.z *= aspectRation / currentRation;
		
		// adding margins to the borders
		/*clipRange.z *= 0.95f;
		clipRange.w *= 0.95f;*/
		
		panel.clipRange = clipRange;
	}

}
