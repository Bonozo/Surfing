using UnityEngine;
using System.Collections;

public class StorePurchase : MonoBehaviour {

	public string iapMethod;

	void OnClick()
	{
		MobiIAB.Instance.SendMessage(iapMethod);
		/*int coins = PlayerPrefs.GetInt("pp_coins");
		coins += addCoins;
		PlayerPrefs.SetInt("pp_coins",coins);
		PlayerPrefs.Save();*/
	}
}
