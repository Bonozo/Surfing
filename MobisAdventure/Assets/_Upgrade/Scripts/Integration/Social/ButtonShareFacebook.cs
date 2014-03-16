using UnityEngine;
using System.Collections;

public class ButtonShareFacebook : MonoBehaviour {

	public UILabel message;
	public GameObject loading;

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

		//message.text = "Sharing...";
		MobiFacebook.Instance.Init ();
		yield return new WaitForSeconds (1f);

		yield return StartCoroutine(MobiFacebook.Instance.Login());
		if(MobiFacebook.Instance.Result==false)
		{
			yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("There was an error sharing with Facebook.\n Please try again later!"));
		}
		else
		{
			//message.text = "Reout";
			yield return StartCoroutine(MobiFacebook.Instance.ReutorizeWithPublishPermission());
			if(MobiFacebook.Instance.Result==false)
			{
				yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("There was an error sharing with Facebook.\n Please try again later!"));
			}
			else
			{
				//message.text = "posting";
				yield return StartCoroutine(MobiFacebook.Instance.PostOnWall(DeathScreen.Instance.LastDistanceTravelled,DeathScreen.Instance.LastLevel));
				if(MobiFacebook.Instance.Result==false)
				{
					yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("There was an error sharing with Facebook.\n Please try again later!"));
				}
				else
				{
					yield return StartCoroutine(DeathScreen.Instance.messageBox.Show("Facebook Sharing Successful! Reward 2500 coins !"));
					posted = true;
					message.text = "done !";

					int coins = PlayerPrefs.GetInt("pp_coins");
					coins += 2500;
					PlayerPrefs.SetInt("pp_coins",coins);
					PlayerPrefs.Save();
				}
			}
		}

		loading.SetActive (false);
		working = false;
	}
}
