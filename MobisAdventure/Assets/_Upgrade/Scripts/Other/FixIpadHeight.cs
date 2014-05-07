using UnityEngine;
using System.Collections;

public class FixIpadHeight : MonoBehaviour {

	void Awake()
	{
		float aspect = (float)Screen.width / (float)Screen.height;
		if( aspect < 1.34f )
			this.GetComponent<UIRoot>().manualHeight = 1200;
	}
}
