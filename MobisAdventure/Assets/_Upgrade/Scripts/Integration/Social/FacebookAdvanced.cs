using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;


#if UNITY_ANDROID
using FacebookAccess = FacebookAndroid;
#elif UNITY_IPHONE
using FacebookAccess = FacebookBinding;
#endif

public class FacebookAdvanced : MonoBehaviour {

	public bool showDebug = true;

	private const string FB_APP_ID = "1470865819839370";

	public int userScore { get; private set; }
	public string userName { get; private set; }
	public string userId { get; private set; }

	public int leaderboardTotal { get; private set; }
	public List<Texture2D> userListAvatars {get;private set;}
	public List<string> userListNames { get; private set;}
	public List<int> userListScores { get; private set;}

	#region Initializing Plugin for Use

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	void Start(){
		// Initializing the plugin on start
		FacebookAccess.init ();

		// Reseting the local score
		userScore = -1;
		userId = "";
		userName = "";
		leaderboardTotal = 0;

		// Add Events
		FacebookManager.sessionOpenedEvent += () => {
			if(showDebug)
				Debug.Log("Facebook: Session Opened");
			if(!HasPublishPermission()){
				RequestPublishPermission();
			}
			else{
				GetUserScore();
				GetLeaderboard();
			}
		};

		FacebookManager.reauthorizationSucceededEvent += () => {
			if(showDebug)
				Debug.Log("Facebook: Reauthorization Successed!");
			GetUserScore();
			GetLeaderboard();
		};

		FacebookManager.loginFailedEvent += (obj) => {
			if(showDebug)
				Debug.Log("Facebook: Session Opened Event Fired");
		};

		FacebookManager.reauthorizationFailedEvent += (obj) => {
			if(showDebug)
				Debug.Log("Facebook: Reauth Failed Event: " + obj);
		};
	}

	#endregion

	#region Login, Get Permissions

	public void Login(){
		var permissions = new string[] { "email", "user_friends" };
		FacebookAccess.loginWithReadPermissions( permissions );
	}

	public void Logout(){
		FacebookAccess.logout ();
	}

	public bool IsLoggedIn(){
		return FacebookAccess.isSessionValid ();
	}

	public void RequestPublishPermission(){
		if(HasPublishPermission()) return;
		var permissions = new string[] { "publish_actions" };
		FacebookAccess.reauthorizeWithPublishPermissions( permissions, FacebookSessionDefaultAudience.Everyone );
	}

	public bool HasPublishPermission(){
		if(!FacebookAccess.isSessionValid()) return false;
		var currentPermissions = FacebookAccess.getSessionPermissions ();

		bool hasPublishActions = false;
		foreach(string p in currentPermissions)
			if(p == "publish_actions"){
				hasPublishActions = true;
				break;
			}

		return hasPublishActions;
	}

	#endregion

	#region Player Scores

	public void GetUserScore(){
		if(!HasPublishPermission()) return;

		string request = "me/scores";
		Facebook.instance.graphRequest( request, HTTPVerb.GET, OnGetScoreComplete );
	}

	private void OnGetScoreComplete( string error, object result ) {
		Debug.Log("Facebook >> OnGetScoreComplete >> error: " + error + ",result: " + result);
		
		var dict = result as Dictionary<string, object>;
		var data = dict ["data"] as List<object>;
		var info = data [0] as Dictionary<string, object>;
		var user = info ["user"] as Dictionary<string, object>;
		
		long score = (long)info ["score"];
		string userid = (string)user ["id"];
		string username = (string)user ["name"];
		
		if (score > userScore) userScore = (int) score;
		this.userId = userid;
		this.userName = userName;
		//Debug.Log("Best Score is: " + bestScore + " , currentId: " + id);
		
	}

	public void SubmitScore(int _score){
		if(!HasPublishPermission()) return;
		if(userId == "") { GetUserScore(); return;}
		if(_score > userScore){
			userScore = _score;
			string request = "me/scores"; // save score to FB
			var parameters = new Dictionary<string,object>(){{  "score", _score.ToString() }};
			Facebook.instance.graphRequest( request, HTTPVerb.POST, parameters, OnPostGraphComplete );
		}
	}

	void OnPostGraphComplete( string error, object result ) {
		Debug.Log("Facebook >> OnPostGraphComplete: " + result);
	}

	#endregion

	#region Leaderboard

	private int loadedAvatars = 0;

	public void GetLeaderboard(){
		if(!HasPublishPermission()) return;
		string request = "/" + FB_APP_ID + "/scores"; // get friend scores
		Facebook.instance.graphRequest( request, OnLeaderboardComplete );
	}

	void OnLeaderboardComplete( string error, object result ) {
		Debug.Log("Facebook >> OnLeaderboardComplete >> error: " + error + ",result: " + result);
		//Prime31.Utils.logObject( result );
		
		var scoreResults = result as Dictionary<string,object>;
		var list = scoreResults["data"] as List<object>;
		
		userListAvatars = new List<Texture2D>();
		userListNames = new List<string>();
		userListScores =  new List<int>();
		
		leaderboardTotal = list.Count;
		loadedAvatars = 0;
		
		for(int i = 0; i < list.Count; i++)
		{
			Dictionary<string,object> ht = list[i] as Dictionary<string,object>;
			Dictionary<string,object> user = ht["user"] as Dictionary<string,object>;
			
			long score = (long) ht["score"];
			string id = (string)user["id"];
			string name = (string)user["name"];
			name = name.Split(' ')[0];
			
			if (userId.ToString() == id) name += " (me)";
			
			//Debug.Log("id: " + id + " , name: " + name + " , score: " + score);
			
			string avatarURL = "http://graph.facebook.com/" + id + "/picture";
			
			// store stuff
			userListNames.Add(name);
			userListScores.Add((int)score);
			
			userListAvatars.Add(null); // load an avatar into this...
			StartCoroutine( LoadAvatar(i,avatarURL) );
			
		}
		
	}

	private IEnumerator LoadAvatar(int userCount, string url) {
		
		//Debug.Log("GameFacebook >> LoadAvatar >>  count: " + userCount + " , url: " + url );
		
		WWW www = new WWW(url);
		yield return www;
		
		float elapsedTime = 0.0f;
		
		while (!www.isDone)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= 4.0f) break;
			yield return null;
		}
		
		userListAvatars[userCount] = www.texture as Texture2D;
		
		if (++loadedAvatars >= leaderboardTotal) {
			/*
			 // after checking to see if all avatars are loaded, they're displayed in my highscore table
			 // leaderboard is displayed using public userListAvatars, userListNames, & userListScores
			*/
		}
		
	}

	#endregion

	#region Post To Wall

	/*public void ShowPostDialog( int score ) {
		string url = "http://www.mobitekgames.com";
		string title = "Beat my score of " + String.Format("{0:n0}", score) + " metres in Hill Climb Racing!";
		string icon_url = @"http://mobitekgames.com/sites/default/files/imagepicker/2/browser/Mobi'sRun_Icon2_1024x1024.jpg";
		string message = "";
		string imageCaption = "Hill Climb Racing.";
		
		var parameters = new Dictionary<string,string>
		{
			{ "link", url },
			{ "name", title },
			{ "picture", icon_url },
			{ "caption", imageCaption }
		};
		FacebookAccess.showDialog( "stream.publish", parameters);
	}*/

	public void PostOnWall(int scores,string level,Action<string,object> completionHandler){
		Facebook.instance.postMessageWithLinkAndLinkToImage(
			"I reached " + scores + " meters in " + level + " level.",
			"http://www.mobitekgames.com/games",
			"Hill Climb Racing",
			"http://mobitekgames.com/sites/default/files/imagepicker/2/browser/Mobi'sRun_Icon2_1024x1024.jpg",
			"", 
			 completionHandler);
	}
	
	#endregion

	#region Invite Friends

	public void InvitePlayer() {
		string title = "Invite a Friend to Play!";
		string message = "Join me in playing a new game!";
		
		Dictionary<string, string> lParam = new Dictionary<string, string>();
		lParam["message"] = message;
		lParam["title"] = title;
		FacebookAccess.showDialog("apprequests", lParam);
	}

	#endregion

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly  UnityEngine.Object _syncRoot = new UnityEngine.Object();
	private static volatile FacebookAdvanced _staticInstance;	
	public static FacebookAdvanced Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(FacebookAdvanced)) as FacebookAdvanced;
					if (_staticInstance == null) {
						Debug.LogError("The FacebookAdvanced instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
