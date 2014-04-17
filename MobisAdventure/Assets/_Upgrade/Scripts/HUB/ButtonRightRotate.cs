using UnityEngine;
using System.Collections;

public class ButtonRightRotate : MonoBehaviour {

	void OnPress(bool isDown){
		PlayerController.Instance.controlRightRotate  = isDown;
	}
}
