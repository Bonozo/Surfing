using UnityEngine;
using System.Collections;

public class DeathResumeButton : MonoBehaviour {

	PlayerController player;

	void Awake()
	{
		player = GameObject.FindObjectOfType (typeof(PlayerController)) as PlayerController;
	}


	void OnClick()
	{
		player.Resume (); 
	}

	void Update()
	{
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}
}
