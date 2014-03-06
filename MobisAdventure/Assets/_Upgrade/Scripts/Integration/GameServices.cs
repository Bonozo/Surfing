using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GameServices : MonoBehaviour {

	#if UNITY_ANDROID

	private static bool initialized = false;
	
	void Start()
	{
		if(!initialized)
		{
			PlayGamesPlatform.DebugLogEnabled = true;
		
			// Activate the Google Play Games platform
			PlayGamesPlatform.Activate();

			initialized = true;
		}
	}

	#endif
}
