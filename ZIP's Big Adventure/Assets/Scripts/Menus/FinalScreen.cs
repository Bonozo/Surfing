using UnityEngine;
using System.Collections;

public class FinalScreen : MonoBehaviour {

	public GameObject mainMenu;

	void OnClick()
	{
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.Save ();
		Title.firstLaunch = true;
		mainMenu.SetActive (true);
		gameObject.SetActive (false);
	}
}
