/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_IPHONE
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class GameServicesSubmit : MonoBehaviour {

	/*#if UNITY_ANDROID
	private string leaderboardName = "CgkI4azPoZEbEAIQBg";
	#elif UNITY_IPHONE
	private string leaderboardName = "monsterhillracing_best";
	#endif

	void Update(){
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}

	#if UNITY_ANDROID

	void OnEnable(){
		GPGManager.authenticationSucceededEvent += authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent += authenticationFailedEvent;

		GPGManager.submitScoreFailedEvent += submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent += submitScoreSucceededEvent;
	}

	void OnDiable(){
		GPGManager.authenticationSucceededEvent -= authenticationSucceededEvent;
		GPGManager.authenticationFailedEvent -= authenticationFailedEvent;

		GPGManager.submitScoreFailedEvent -= submitScoreFailedEvent;
		GPGManager.submitScoreSucceededEvent -= submitScoreSucceededEvent;
	}

	void authenticationSucceededEvent( string param )
	{
		SumbitScore();
	}

	void authenticationFailedEvent( string error )
	{
		Debug.Log( "authenticationFailedEvent: " + error );
	}

	
	void submitScoreFailedEvent( string leaderboardId, string error )
	{
		Debug.Log( "submitScoreFailedEvent. leaderboardId: " + leaderboardId + ", error: " + error );
	}
	
	
	void submitScoreSucceededEvent( string leaderboardId, Dictionary<string,object> scoreReport )
	{
		Debug.Log( "submitScoreSucceededEvent" );
		Prime31.Utils.logObject( scoreReport );
		PlayGameServices.showLeaderboard (leaderboardName);
	}

	void OnClick(){
		if (!PlayGameServices.isSignedIn ()) {
			PlayGameServices.authenticate();
		} else{
			SumbitScore();
		}
	}

	void SumbitScore(){
		
		int score = DeathScreen.Instance.LastDistanceTravelled;
		PlayGameServices.submitScore(leaderboardName,score);
	}

	#endif

	#if UNITY_IPHONE

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

	#endif*/
}
