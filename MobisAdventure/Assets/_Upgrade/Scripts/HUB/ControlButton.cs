/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class ControlButton : MonoBehaviour {

	public bool tilt = false;
	public GameObject root;

	void OnClick(){
		PlayerPrefs.SetInt("options_control",tilt?1:0);
		PlayerPrefs.Save ();

		PlayerController.Instance.tilt = tilt;
		root.SetActive (false);

		ButtonLeftRotate.Instance.gameObject.SetActive (!tilt);
		ButtonRightRotate.Instance.gameObject.SetActive (!tilt);

		if (tilt)
			DeathScreen.Instance.ShowMessageAndUnPause("You have activated Tilt Controls; locate the options icon on the menu screens to activate Button Controls.");
		else
			DeathScreen.Instance.ShowMessageAndUnPause("You have activated Button Controls; locate the options icon on the menu screens to activate Tilt Controls.");

	}
	
}
