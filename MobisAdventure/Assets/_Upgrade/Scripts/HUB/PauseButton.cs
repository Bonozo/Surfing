/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(isDown)
			PlayerController.Instance.PauseGame ();
	}
}
