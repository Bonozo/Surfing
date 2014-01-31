using UnityEngine;
using System.Collections;

public class GetMoreCoins : MonoBehaviour {

	public float cost;
	public int addCoins;

	void OnClick()
	{
		int coins = PlayerPrefs.GetInt("pp_coins");
		coins += addCoins;
		PlayerPrefs.SetInt("pp_coins",coins);
		PlayerPrefs.Save();
	}
}
