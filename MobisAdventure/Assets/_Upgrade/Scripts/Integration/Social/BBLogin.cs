using UnityEngine;
using System.Collections;

public class BBLogin : MonoBehaviour {
	
	void OnClick()
	{
		FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
	}
}
