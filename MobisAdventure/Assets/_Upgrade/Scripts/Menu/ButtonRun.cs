using UnityEngine;
using System.Collections;

public class ButtonRun : MonoBehaviour {

	void Update()
	{
		collider.enabled = !MainMenu.Instance.isPopupActive;
	}

	void OnClick()
	{
		GameManager.m_chosenSled = (GameManager.ChosenSled)(PlayerPrefs.GetInt("mobisled",1)-1);
		GameManager.m_chosenMobi = (GameManager.ChosenMobi)(PlayerPrefs.GetInt("mobicharacter",1)-1);
		GameManager.m_chosenLevel = (GameManager.ChosenLevel)(PlayerPrefs.GetInt("mobilevel",1)-1);
		Loader.sceneName = GameManager.m_chosenLevel.ToString();
		Loader.destroyme = true;
		Application.LoadLevel("Loader");
	}
}
