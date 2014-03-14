using UnityEngine;
using System.Collections;

public class ButtonShareFacebook : MonoBehaviour {
	
	public UILabel upText;
	public GameObject loading;

	bool posted = false;
	bool working = false;
	void OnClick()
	{
		if(posted) return;
		if(!working)
		{
			StartCoroutine (ShareOnFacebook ());
		}
	}

	IEnumerator ShareOnFacebook()
	{
		working = true;
		loading.SetActive (true);

		if(!MobiFacebook.Instance.LoggedIn)
			yield return StartCoroutine(MobiFacebook.Instance.Login());
		if(MobiFacebook.Instance.LoggedIn)
		{
			yield return StartCoroutine(MobiFacebook.Instance.ReutorizeWithPublishPermission());
			if(MobiFacebook.Instance.Result)
			{
				yield return StartCoroutine(MobiFacebook.Instance.PostOnWall(DeathScreen.Instance.LastDistanceTravelled));
				if(MobiFacebook.Instance.Result)
				{
					upText.text = "Shared!";
					Coin_Counter.AddCoins(2500);
					PlayerPrefs.Save();
					posted = true;
				}
			}
		}

		loading.SetActive (false);
		working = false;
	}
}
