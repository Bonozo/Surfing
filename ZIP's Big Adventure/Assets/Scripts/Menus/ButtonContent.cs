using UnityEngine;
using System.Collections;

public class ButtonContent : MonoBehaviour {

	public string levelName;

	void Awake()
	{
		if (PlayerPrefs.HasKey (levelName))
						this.GetComponent<UIButton> ().isEnabled = false;
	}

	void OnClick()
	{
		Loader.sceneName = levelName;
		Application.LoadLevel ("loader");
	}
}
