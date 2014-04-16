using UnityEngine;
using System.Collections;

public class HUBButton : MonoBehaviour {

	void Update()
	{
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf && 
						!PlayerController.Instance.GamePaused;
	}
}
