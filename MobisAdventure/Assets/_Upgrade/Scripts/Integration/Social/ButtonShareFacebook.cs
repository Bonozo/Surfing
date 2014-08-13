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

		if(!FacebookAdvanced.Instance.IsLoggedIn())
			FacebookAdvanced.Instance.Login();
		float time = RealTime.time + 10f;
		while(!FacebookAdvanced.Instance.HasPublishPermission() && time>RealTime.time)
			yield return new WaitForEndOfFrame();

		if(FacebookAdvanced.Instance.HasPublishPermission())
			FacebookAdvanced.Instance.PostOnWall(DeathScreen.Instance.LastDistanceTravelled,DeathScreen.Instance.LastLevel,
			  (error,result) => {
				if(error == null){
					posted = true;
					message.text = "done!";
					
					int coins = PlayerPrefs.GetInt("pp_coins");
					coins += 2500;
					PlayerPrefs.SetInt("pp_coins",coins);
					PlayerPrefs.Save();
				}
			});

		loading.SetActive (false);
		working = false;
	}
}
