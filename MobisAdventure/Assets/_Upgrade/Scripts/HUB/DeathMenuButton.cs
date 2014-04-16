using UnityEngine;
using System.Collections;

public class DeathMenuButton : MonoBehaviour {

	void Update()
	{
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}

	void OnClick()
	{
		Loader.sceneName = "menu";
		Loader.destroyme = false;
		Application.LoadLevel("Loader");
	}
}
