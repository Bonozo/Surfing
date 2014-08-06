using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	public UILabel labelLevelName;
	public UILabel labelScore;
	public UILabel labelLevel;
	public UILabel labelCoins;

	void OnEnable(){
		var lvlName = GameManager.m_chosenLevel.ToString ().Split ('_') [1];
		if(lvlName == "IcyTundra") lvlName = "Icy Tundra";
		labelLevelName.text = lvlName.ToUpper ();
		labelScore.text = DeathScreen.Instance.resultDistance + "m (" + DeathScreen.Instance.resultBest + " best)";
		labelLevel.text = "Level: " + DeathScreen.Instance.levelController.CurrentLevel;
		labelCoins.text = "+" + DeathScreen.Instance.resultScore;
	}
}
