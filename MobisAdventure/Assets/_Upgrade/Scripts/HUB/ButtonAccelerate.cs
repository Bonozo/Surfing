/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class ButtonAccelerate : MonoBehaviour {

	void OnPress(bool isDown){
	   PlayerController.Instance.controlAcceleration  = isDown;
	}
}
