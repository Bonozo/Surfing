using UnityEngine;
using System.Collections;

public class ButtonLeftRotate : MonoBehaviour {

	void OnPress(bool isDown){
		PlayerController.Instance.controlLeftRotate  = isDown;
	}
}
