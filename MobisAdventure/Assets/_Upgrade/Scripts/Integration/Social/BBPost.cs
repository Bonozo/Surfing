using UnityEngine;
using System.Collections;

public class BBPost : MonoBehaviour {

	
	void OnClick()
	{
		Facebook.instance.postMessage ("Hello", PostedHandler);
	}

	private void PostedHandler( string error, object result )
	{
		Debug.Log ("Post responde: " + error);
	}
}
