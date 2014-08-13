using UnityEngine;
using System.Collections;

public class ButtonConnectTwitter : MonoBehaviour {

	void Update()
	{
		collider.enabled = !MainMenu.Instance.isPopupActive;
	}
}
