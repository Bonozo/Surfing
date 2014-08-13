using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadHighScores : MonoBehaviour {

	public GameObject guiConnected;
	public GameObject guiNotConnected;
	public Transform grid;
	public UILabel labelMessage;
	private int total=0;

	void OnEnable(){
		foreach(Transform profile in grid)
			profile.gameObject.SetActive(false);
		bool connected = FacebookAdvanced.Instance.HasPublishPermission ();
		guiConnected.SetActive (connected);
		guiNotConnected.SetActive (!connected);
		ShowLeaderboard ();
	}

	void Update(){

		if (FacebookAdvanced.Instance.leaderboardTotal != total){
			// We compare these values for knowing that the leaderboard memeber's count has been changed.
			total = FacebookAdvanced.Instance.leaderboardTotal;
			ShowLeaderboard();
		}
	}

	void ShowLeaderboard(){

		if(FacebookAdvanced.Instance.leaderboardTotal==0){
			labelMessage.text = "No data to show";
		}
		else{
			labelMessage.text = "";
			// Only first 30 is showed, fix this
			for(int i=0;i<FacebookAdvanced.Instance.leaderboardTotal&&i<grid.childCount;i++){
				var profile = grid.GetChild(i);
				profile.FindChild("Rank").GetComponent<UILabel>().text = "" + (i+1) + ".";
				profile.FindChild("Score").GetComponent<UILabel>().text = "" + FacebookAdvanced.Instance.userListScores[i];
				profile.FindChild("Name").GetComponent<UILabel>().text = FacebookAdvanced.Instance.userListNames[i];
				StartCoroutine(WaitForPictureReady(grid.GetChild(i).FindChild("Avatar").GetComponent<UITexture>(),FacebookAdvanced.Instance.userListAvatars[i]));
				profile.gameObject.SetActive(true);
			}
			for(int i=FacebookAdvanced.Instance.leaderboardTotal;i<grid.childCount;i++)
				grid.GetChild(i).gameObject.SetActive(false);
		}
	}

	IEnumerator WaitForPictureReady(UITexture dest,Texture2D tex){
		float time = Time.time + 10f;
		while(tex == null && time > Time.time)
			yield return new WaitForEndOfFrame();

		if(tex == null)
			Debug.Log("Avatar is null");
		else{
			dest.mainTexture = tex;
			Debug.Log("Avatar loaded Successfully");
		}
	}
}
