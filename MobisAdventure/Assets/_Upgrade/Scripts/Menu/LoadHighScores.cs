using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif


public class LoadHighScores : MonoBehaviour {

	public UILabel labelLoadingText;
	public UILabel labelLeaderboard;

	void OnEnable(){
		StartCoroutine ("ShowLoading");
		Load ();
	}

	IEnumerator ShowLoading(){
		float timeout = 0.3f;
		while (true) {
			labelLoadingText.text = "LOADING.";
			yield return new WaitForSeconds(timeout);
			labelLoadingText.text = "LOADING..";
			yield return new WaitForSeconds(timeout);
			labelLoadingText.text = "LOADING...";
			yield return new WaitForSeconds(2.2f*timeout);
		}
	}

	#if UNITY_ANDROID
	private string leaderboardName = "CgkIu_XOtq8QEAIQAQ";
	#elif UNITY_IPHONE
	private string leaderboardName = "monsterhillracing_best";
	#endif

	#if UNITY_ANDROID || UNITY_IPHONE

	void Load(){ // Need to be fixed
		if(Social.localUser.authenticated){
			Social.LoadScores(leaderboardName, scores => {
				if (scores.Length > 0) {
					Debug.Log ("Got " + scores.Length + " scores");
					string myScores = "Leaderboard:\n";
					foreach (IScore score in scores)
						myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
					Debug.Log (myScores);
				}
				else
					Debug.Log ("No scores loaded");

				StopCoroutine("ShowLoading");
				if(scores.Length == 0){
					labelLoadingText.text = "No data found";
					labelLeaderboard.text = "";
				}
				else{
					labelLeaderboard.text = "";
					for(int i=0;i<Mathf.Max(6,scores.Length);i++)
						labelLeaderboard.text += "" + scores[i].rank + ". " + scores[i].userID + " - " + scores[i].formattedValue;
				}

			});
		}


	}

	#endif
}
