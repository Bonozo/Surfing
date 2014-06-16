using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif

#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class GameServicesSubmit : MonoBehaviour {

	#if UNITY_ANDROID
	private string leaderboardName = "CgkIu_XOtq8QEAIQAQ";
	#elif UNITY_IPHONE
	private string leaderboardName = "monsterhillracing_best";
	#endif

	void Update(){
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}

	#if UNITY_ANDROID || UNITY_IPHONE

	void OnClick(){
		int score = DeathScreen.Instance.LastDistanceTravelled;

		if(!Social.localUser.authenticated){
			Social.localUser.Authenticate(success => {
				if(success)
					SubmitScore(score);
				Debug.Log("Game Center Authentication Status: " + success); });
		} else{
			SubmitScore(score);
		}
	}

	void SubmitScore(int score){
		Social.ReportScore(score,  leaderboardName, submit => {
			if(submit)
				Social.ShowLeaderboardUI();
			Debug.Log("Report Score Status: " + submit); });
	}

	#endif
}
