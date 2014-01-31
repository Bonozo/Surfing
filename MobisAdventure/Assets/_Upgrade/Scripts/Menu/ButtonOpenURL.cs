using UnityEngine;
using System.Collections;

public class ButtonOpenURL : MonoBehaviour {

	public string url;
	void OnClick()
	{
		Application.OpenURL(url);
	}
}
