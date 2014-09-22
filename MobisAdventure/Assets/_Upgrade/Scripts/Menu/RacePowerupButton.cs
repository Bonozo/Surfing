/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class RacePowerupButton : MonoBehaviour {

	public PowerupMenu menu;
	public string powerupName;

	void OnClick(){
		menu.upgradeName = powerupName;
		menu.gameObject.SetActive (true);
	}
}
