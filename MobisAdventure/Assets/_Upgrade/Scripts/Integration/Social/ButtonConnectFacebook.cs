using UnityEngine;
using System.Collections;

public class ButtonConnectFacebook : MonoBehaviour {

	void Start(){
		// Update Leaderboard anyway 
		FacebookAdvanced.Instance.GetLeaderboard ();
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
