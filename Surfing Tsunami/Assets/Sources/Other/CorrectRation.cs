using UnityEngine;
using System.Collections;

public class CorrectRation : MonoBehaviour {
	
	void Awake () 
	{
		float asp1 = (float)LevelInfo.Camera.Width / (float)LevelInfo.Camera.Height;
		float asp2 = (float)Screen.width / (float)Screen.height;

		float y = (float)LevelInfo.Camera.Width * (float)Screen.height / (float)Screen.width / (float)LevelInfo.Camera.Height;
		transform.localScale = new Vector3 (1f, y, 1f);
	}
}
