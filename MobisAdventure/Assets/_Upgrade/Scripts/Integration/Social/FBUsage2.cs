using UnityEngine;
using System.Collections;

public class FBUsage2 : MonoBehaviour {

	public FacebookAdvanced fb;
	int localscore = 1;

	void OnGUI(){
		GUILayout.BeginVertical ();

		// Login
		GUILayout.Label ("--Login--");
		GUILayout.Label ("Is logged in?: " + fb.IsLoggedIn());
		if(GUILayout.Button("Login")) fb.Login();
		if(GUILayout.Button("Logout")) fb.Logout();

		// Permissions
		GUILayout.Label ("--Permissions--");
		GUILayout.Label ("Has Permissions?: " + fb.HasPublishPermission());
		if(GUILayout.Button("Request Permissions")) fb.RequestPublishPermission();

		// Scores
		GUILayout.Label ("--Scores--");
		GUILayout.Label ("User score: " + fb.userScore);
		GUILayout.Label ("User name: " + fb.userName);
		if(GUILayout.Button("Get my score")) fb.GetUserScore();

		// Post Score
		GUILayout.Label ("--Post Score--");
		GUILayout.Label ("Local score: " + localscore);
		if(GUILayout.Button("Random local score")) localscore = Random.Range(1,1000);
		if(GUILayout.Button("Post My Score")) fb.SubmitScore(localscore);

		// Leaderboard
		GUILayout.Label ("--Leaderboard");

		if(GUILayout.Button("Get Leadeboard")) fb.GetLeaderboard();

		GUILayout.Label ("High Scores Count: " + fb.leaderboardTotal);
		for (int i=0; i<fb.leaderboardTotal; i++) {
			if( fb.userListAvatars[i] != null)
				GUI.DrawTexture(new Rect(20f+i*100f,Screen.height-100f,80f,80f),fb.userListAvatars[i]);
			GUILayout.Label(""+(i+1)+fb.userListNames[i] + " " + fb.userListScores[i]);
		}	


		
	//	if(GUILayout.Button("Post To Wall")) fb.PostScoreToWall(123);
		
		if(GUILayout.Button("Invite friends")) fb.InvitePlayer();

		GUILayout.EndHorizontal ();
	}
}
