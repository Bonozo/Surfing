using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif
#if UNITY_ANDROID
using GooglePlayGames;
#endif

public class GameServices : MonoBehaviour {

	#if UNITY_ANDROID
	private static bool playInitialized = false;
	#endif

	void Start()
	{
		#if UNITY_ANDROID
		if(!playInitialized)
		{
			// Activate the Google Play Games platform
			//PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
			playInitialized = true;
		}
		#endif
		Social.localUser.Authenticate(success => {
			Debug.Log("Game Center Authentication Status: " + success); });
	}

}
