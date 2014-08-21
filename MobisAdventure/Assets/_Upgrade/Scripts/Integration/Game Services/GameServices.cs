using UnityEngine;
using System.Collections;
#if UNITY_IPHONE
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class GameServices : MonoBehaviour {

	void Start()
	{
		#if UNITY_IPHONE
		Social.localUser.Authenticate(success => {
			Debug.Log("Game Center Authentication Status: " + success); });
		#endif
		#if UNITY_ANDROID
		PlayGameServices.authenticate();
		#endif
	}
}
