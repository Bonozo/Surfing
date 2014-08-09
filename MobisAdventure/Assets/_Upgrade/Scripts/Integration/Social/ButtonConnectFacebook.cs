using UnityEngine;
using System.Collections;

public class ButtonConnectFacebook : MonoBehaviour {

	void OnClick(){
		Debug.Log ("FB Login Called");
		MobiFacebook.Instance.Login ();
	}
}
