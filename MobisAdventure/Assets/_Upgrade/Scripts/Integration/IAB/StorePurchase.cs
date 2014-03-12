using UnityEngine;
using System.Collections;

public class StorePurchase : MonoBehaviour {

	public string iapMethod;

	void OnClick()
	{
		StartCoroutine (MakePurchase ());
	}

	IEnumerator MakePurchase()
	{
		yield return StartCoroutine(MainMenu.Instance.messagebox.ConnectToIAP());
		if( MobiIAB.Instance.Connected)
			MobiIAB.Instance.SendMessage(iapMethod);
	}
}
