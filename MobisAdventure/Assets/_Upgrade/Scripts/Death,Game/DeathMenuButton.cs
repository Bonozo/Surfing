using UnityEngine;
using System.Collections;

public class DeathMenuButton : MonoBehaviour {

	void OnClick()
	{
		Loader.sceneName = "menu";
		Loader.destroyme = false;
		Application.LoadLevel("Loader");
	}
}
