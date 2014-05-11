using UnityEngine;
using System.Collections;

public class ButtonRightRotate : MonoBehaviour {

	void Awake()
	{
		if(PlayerPrefs.GetInt("options_control")==1) // tilt control
			gameObject.SetActive(false);
	}

	void OnPress(bool isDown){
		PlayerController.Instance.controlRightRotate  = isDown;
	}
}
