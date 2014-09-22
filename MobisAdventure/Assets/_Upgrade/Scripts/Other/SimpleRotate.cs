/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {
	
	public bool ignoreTimeScale = true;
	public Vector3 speed = Vector3.zero;
	
	void Update(){
		transform.Rotate(speed*(ignoreTimeScale?RealTime.deltaTime:Time.deltaTime));
	}
}
