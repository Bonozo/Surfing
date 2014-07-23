using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_IPHONE
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
#endif


public class LoadHighScores : MonoBehaviour {

	#if UNITY_ANDROID
	private string leaderboardName = "CgkIu_XOtq8QEAIQAQ";
	#elif UNITY_IPHONE
	private string leaderboardName = "monsterhillracing_best";
	#endif

	public UILabel labelLoadingText;
	public Transform grid;
	public Texture defaultAvatar;

	void OnEnable(){
		GPGManager.loadScoresFailedEvent += loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent += loadScoresSucceededEvent;
		GPGManager.profileImageLoadedAtPathEvent += profileImageLoadedAtPathEvent;

		foreach(Transform profile in grid)
			profile.gameObject.SetActive(false);

		labelLoadingText.text = "";

		StartCoroutine ("ShowLoading");
		StartCoroutine ("ShowLeaderboard");
	}

	void OnDisable(){
		GPGManager.loadScoresFailedEvent -= loadScoresFailedEvent;
		GPGManager.loadScoresSucceededEvent -= loadScoresSucceededEvent;
		GPGManager.profileImageLoadedAtPathEvent -= profileImageLoadedAtPathEvent;
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
	
	IEnumerator ShowLeaderboard(){
		yield return null;
		if(!PlayGameServices.isSignedIn()){
			StopCoroutine("ShowLoading");
			labelLoadingText.text = "Not signed in.";	
		}
		else{
			PlayGameServices.loadScoresForLeaderboard(leaderboardName,GPGLeaderboardTimeScope.AllTime,false,false);
		}
	}

	void loadScoresFailedEvent( string leaderboardId, string error )
	{
		StopCoroutine("ShowLoading");
		labelLoadingText.text = "Failed with error: " + error;
	}
	
	
	void loadScoresSucceededEvent( List<GPGScore> scores )
	{
		Debug.Log( "loadScoresSucceededEvent" );
		Prime31.Utils.logObject( scores );

		StopCoroutine("ShowLoading");
		labelLoadingText.text = "";

		if( scores.Count == 0){
			labelLoadingText.text = "No data to show";
		}else{
			// Only first 30 is showed, fix this
			for(int i=0;i<scores.Count&&i<grid.childCount;i++){
				var profile = grid.GetChild(i);
				profile.FindChild("Rank").GetComponent<UILabel>().text = scores[i].formattedRank + ".";
				profile.FindChild("Score").GetComponent<UILabel>().text = scores[i].formattedScore;
				profile.gameObject.SetActive(true);
			}

			StartCoroutine(LoadProfilePictures(scores));
		}
	}

	bool imageLoaded = false;
	string nextImagePath;
	IEnumerator LoadProfilePictures(List<GPGScore> scores){
		for(int i=0;i<scores.Count&&i<grid.childCount;i++){
			if( scores[i].avatarUrl == null){
				grid.GetChild(i).FindChild("Avatar").GetComponent<UITexture>().mainTexture = defaultAvatar;
			}
			else{
				//Debug.Log("Loading image: " + i);
				imageLoaded = false;
				PlayGameServices.loadProfileImageForUri(scores[i].avatarUrl);
				while(!imageLoaded) yield return null;
				//Debug.Log("Putting Image: " + i);
				yield return StartCoroutine(PutImage(grid.GetChild(i).FindChild("Avatar").GetComponent<UITexture>(),
				                                     "file://" + nextImagePath));
			}
		}
	}

	void profileImageLoadedAtPathEvent( string path )
	{
		//Debug.Log( "profileImageLoadedAtPathEvent: " + path );
		nextImagePath = path;
		imageLoaded = true;
	}
	
	IEnumerator PutImage(UITexture tex,string url){
		WWW www = new WWW (url);
		//Debug.Log ("Put image started. url: " + url);
		yield return www;
		if(www.error == null){
			//Debug.Log("Loading Texture");
			tex.mainTexture = www.texture;
		}
		else{
			//Debug.Log("Error: " + www.error + " url: " + url);
		}
	}
	/*#if UNITY_ANDROID || UNITY_IPHONE

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

	#endif*/
}
