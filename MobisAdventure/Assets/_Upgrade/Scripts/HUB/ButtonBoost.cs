using UnityEngine;
using System.Collections;

public class ButtonBoost : MonoBehaviour {

	void OnPress(bool isDown){
		PlayerController.Instance.controlBoost  = isDown;
	}
}
