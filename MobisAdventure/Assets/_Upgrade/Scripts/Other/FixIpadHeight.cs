/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class FixIpadHeight : MonoBehaviour {
	
	void Awake()
	{
		float aspect = (float)Screen.width / (float)Screen.height;
		if( aspect < 1.34f )
			this.GetComponent<UIRoot>().manualHeight = 1200;
	}

	private static bool calc = false;
	private static bool res = false;
	public static bool isWideScreen{
		get{
			if(!calc){
				res = ( (float)Screen.width / (float)Screen.height ) < 1.34f;
				calc = true;
			}
			return res;
		}
	}
}
