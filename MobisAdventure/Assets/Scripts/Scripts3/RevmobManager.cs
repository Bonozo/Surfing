using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RevmobManager : MonoBehaviour ,IRevMobListener {
	
	private static readonly Dictionary<String, String> REVMOB_APP_IDS = new Dictionary<String, String>() {
        { "Android", "51b1cbaa1eb3ebaabd00011e"},
        { "IOS", "51b1cbaa1eb3ebaabd00011e" }
    };
    private RevMob revmob;
	private RevMobFullscreen fullscreen;
    void Awake() {
		DontDestroyOnLoad (this.gameObject);
        revmob = RevMob.Start(REVMOB_APP_IDS, "RevmobManager");
		//fullscreen = revmob.CreateFullscreen();
    }
	
	// Use this for initialization
	void Start () {
		//revmob = RevMob.Start(REVMOB_APP_IDS, "RevmobManager");
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AdDidReceive (string revMobAdType) {
        Debug.Log("Ad did receive.");
    }

    public void AdDidFail (string revMobAdType) {
        Debug.Log("Ad did fail.");
    }

    public void AdDisplayed (string revMobAdType) {
        Debug.Log("Ad displayed.");
    }

    public void UserClickedInTheAd (string revMobAdType) {
        Debug.Log("Ad clicked.");
    }

    public void UserClosedTheAd (string revMobAdType) {
        Debug.Log("Ad closed.");
		//fullscreen.Hide();
		//fullscreen.Release();
    }
	
	public void InstallDidReceive (string rebMobAdType) {
		
	}
	
	public void InstallDidFail (string rebMobAdType) {
		
	}

	void showRevmobPopup()
	{
		if (PlayerPrefs.GetInt("RevmobStatus", 0) == 0 )
		{
			revmob.ShowPopup();
		}
	}

	void showRevmobFullScreen()
	{
		if (PlayerPrefs.GetInt("RevmobStatus", 0) == 0 )
		{
/*#if UNITY_ANDROID
			revmob.ShowPopup();
#elif UNITY_IPHONE*/
			revmob.ShowFullscreen();
//#endif
			//fullscreen.Show();
		}
	}
}
