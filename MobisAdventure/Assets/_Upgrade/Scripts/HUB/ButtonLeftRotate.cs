using UnityEngine;
using System.Collections;

public class ButtonLeftRotate : MonoBehaviour {

	public static ButtonLeftRotate Instance;

	void Awake()
	{
		Instance = this;
		if(PlayerPrefs.GetInt("options_control")==1) // tilt control
			gameObject.SetActive(false);
	}

	void OnPress(bool isDown){
		PlayerController.Instance.controlLeftRotate  = isDown;
	}
}
