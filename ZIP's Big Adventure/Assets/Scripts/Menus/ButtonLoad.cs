using UnityEngine;
using System.Collections;

public class ButtonLoad : MonoBehaviour {

	public string sceneName;

	void OnClick()
	{
		Loader.sceneName = sceneName;
		Application.LoadLevel ("loader");
	}
}
