using UnityEngine;
using System.Collections;

public class ButtonShareTwitter : MonoBehaviour {

	public UILabel message;
	public GameObject loading;

	#if UNITY_ANDROID

	private bool posted=false;
	private bool working=false;
	
	void Update()
	{
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}
	
	void OnClick()
	{
		if(!working && !posted)
			StartCoroutine (Post ());
	}
	
	IEnumerator Post()
	{
		working = true;
		loading.SetActive (true);

		yield return StartCoroutine(MobiTwitter.Instance.Init());
		if(!MobiTwitter.Instance.Result)
		{
			yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("There was an error sharing with Twitter.\n Please try again later!"));
		}
		else
		{
			yield return StartCoroutine(MobiTwitter.Instance.Login());
			if(!MobiTwitter.Instance.Result)
			{
				yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("There was an error sharing with Twitter.\n Please try again later!"));
			}
			else
			{
				yield return StartCoroutine(MobiTwitter.Instance.Post(DeathScreen.Instance.LastDistanceTravelled,DeathScreen.Instance.LastLevel));
				if(!MobiTwitter.Instance.Result)
				{
					yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("There was an error sharing with Twitter.\n Please try again later!"));
				}
				else
				{
					posted = true;
					message.text = "DONE!";

					int coins = PlayerPrefs.GetInt("pp_coins");
					coins += 2500;
					PlayerPrefs.SetInt("pp_coins",coins);
					PlayerPrefs.Save();
					yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("Twitter Sharing Successful! Reward 2500 coins !"));
				}
			}
		}

		loading.SetActive (false);
		working = false;
	}
	#endif
}
