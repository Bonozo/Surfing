using UnityEngine;
using System.Collections;

public class StorePurchase : MonoBehaviour {

	public static bool debug = true;

	public int debugCoinsAdd = 0; // used in degug
	public string iapMethod;

	void OnClick()
	{
		if(debug){
			int coins = PlayerPrefs.GetInt("pp_coins");
			coins += debugCoinsAdd;
			PlayerPrefs.SetInt("pp_coins",coins);
			PlayerPrefs.Save();
		}
		else{
			StartCoroutine (MakePurchase ());
		}
	}

	IEnumerator MakePurchase()
	{
		yield return StartCoroutine(MainMenu.Instance.messagebox.ConnectToIAP());
		if( MobiIAB.Instance.Connected)
			MobiIAB.Instance.SendMessage(iapMethod);
	}
}
