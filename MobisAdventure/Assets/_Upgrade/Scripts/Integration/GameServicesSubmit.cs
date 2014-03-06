using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GameServicesSubmit : MonoBehaviour {

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
}
