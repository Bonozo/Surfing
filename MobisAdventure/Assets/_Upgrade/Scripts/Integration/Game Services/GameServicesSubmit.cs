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

	void Update()
	{
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}

	#if UNITY_ANDROID

	void OnClick()
	{
		int score = DeathScreen.Instance.LastDistanceTravelled;
		Social.localUser.Authenticate((bool success) => {
			if(success)
			{
				Social.ReportScore(score, "CgkIu_XOtq8QEAIQAQ", (bool submit) => {
					if(submit)
					{
						Social.ShowLeaderboardUI();
					}
				});
			}
		});
	}

	#endif

	#if UNITY_IPHONE
	
		void OnClick()
		{
			DoLeaderboard ();
		}

		void DoLeaderboard () {
		
			Social.localUser.Authenticate (success => {
				
				if (success) {

					int score = DeathScreen.Instance.LastDistanceTravelled;

					Debug.Log ("Authentication successful");
					string userInfo = "Username: " + Social.localUser.userName + 
						"\nUser ID: " + Social.localUser.id + 
							"\nIsUnderage: " + Social.localUser.underage;
					Debug.Log (userInfo);
					
					Social.CreateLeaderboard();

						Social.CreateLeaderboard().id = "mobisRunLeaderboard";
						ReportScore(score, "mobisRunLeaderboard");

					Social.ShowLeaderboardUI();
				}
				else
					Debug.Log ("Authentication failed");
			} );
		}
	
	void ReportScore (long score, string leaderboardID) {
		Debug.Log ("Reporting score " + score + " on leaderboard " + leaderboardID);
		Social.ReportScore (score, leaderboardID, success => {
			Debug.Log(success ? "Reported score successfully" : "Failed to report score");
		});
	}
	
	#endif
}
