using UnityEngine;
using System.Collections;

public class FinalScreen : MonoBehaviour {

	public GameObject mainMenu;

	void OnClick()
	{
		Title.firstLaunch = true;
		Application.LoadLevel ("title");
	}
}
