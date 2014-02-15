using UnityEngine;
using System.Collections;

public class FinalScreen : MonoBehaviour {

	public GameObject mainMenu;

	void OnClick()
	{
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();
		GameController.gameLevel = GameLevel.None;
		mainMenu.SetActive (true);
		gameObject.SetActive (false);
	}
}
