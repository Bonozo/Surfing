using UnityEngine;
using System.Collections;

public class ButtonConnectFacebook : MonoBehaviour {

	void Start(){

		bool connect_status = (PlayerPrefs.GetInt ("facebook_connected", 0) == 1);
		transform.FindChild ("Background").gameObject.SetActive (!connect_status);
		GetComponent<UIButton> ().isEnabled = !connect_status;

		// anyway
		if(FacebookAdvanced.Instance.IsLoggedIn() && !FacebookAdvanced.Instance.HasPublishPermission())
			FacebookAdvanced.Instance.RequestPublishPermission();
		FacebookAdvanced.Instance.GetLeaderboard ();

		// Check if out saved status is not true
		bool connected = FacebookAdvanced.Instance.IsLoggedIn ();
		transform.FindChild ("Background").gameObject.SetActive (!connected);
		GetComponent<UIButton> ().isEnabled = !connected;
		PlayerPrefs.SetInt ("facebook_connected", connected?1:0);
		PlayerPrefs.Save ();
	}

	void OnClick(){
		StartCoroutine (FBConnectThread ());
	}

	IEnumerator FBConnectThread(){

		if(!FacebookAdvanced.Instance.IsLoggedIn()){
			string message = "CONNECT WITH FACEBOOK AND PLAY WITH YOUR FRIENDS!";
			string okbm = "CONNECT";
			string backbm = "BACK";
			
			yield return StartCoroutine(MainMenu.Instance.confirmationPopup.ShowPopupThread(
				message,okbm,backbm));
			bool status = MainMenu.Instance.confirmationPopup.status;
			if(status)
				FacebookAdvanced.Instance.Login();
		}
		else if(!FacebookAdvanced.Instance.HasPublishPermission())
			FacebookAdvanced.Instance.RequestPublishPermission();
	}

	void Update()
	{
		collider.enabled = !MainMenu.Instance.isPopupActive;
	}
}
