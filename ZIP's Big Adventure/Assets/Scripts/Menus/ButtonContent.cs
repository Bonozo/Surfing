using UnityEngine;
using System.Collections;

public class ButtonContent : MonoBehaviour {

	public GameType gameType;

	void Awake()
	{
		if (PlayerPrefs.HasKey (gameType.ToString()))
			this.GetComponent<UIButton> ().isEnabled = false;
	}

	void OnClick()
	{
		GameController.gameType = gameType;
		Loader.sceneName = gameType.ToString();
		Application.LoadLevel ("loader");
	}
}
