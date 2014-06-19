﻿using UnityEngine;
using System.Collections;

public class DeathScreen : MonoBehaviour {

	public TweenAlpha bgAlphaTween;
	public GameObject stats;
	public UILabel labelCause;
	public UILabel labelStates;
	public UILabel labelCoins;
	public UILabel labelLevel;
	public LevelController levelController;
	public MessageBox messageBox;
	
	public int LastDistanceTravelled{ get; private set;}
	public string LastLevel{ get { return levelName.Substring (6); } }
	
	public void Show(bool monstercause,int distance,int best,int score)
	{
		LastDistanceTravelled = distance;

		if(monstercause) labelCause.text = "the monster\ncaught you!";
		else labelCause.text = "you\ncrashed!";

		string[] sll = GameManager.m_chosenLevel.ToString().Split('_');
		labelStates.text = "" + distance + "m\n" + best + "m (best)\n in " + sll[1].ToLower();
		labelCoins.text = "+" + score;
		labelLevel.text = "Level: " + levelController.CurrentLevel;

		StartCoroutine(ShowThread());
	}

	IEnumerator ShowThread()
	{
		MusicLoop.Instance.DropVolume (0.4f, 1f);
		yield return new WaitForSeconds(1f);
		bgAlphaTween.PlayForward();
		yield return new WaitForSeconds(bgAlphaTween.duration);
		stats.SetActive(true);

		#if UNITY_IPHONE
		// for iPhone platform disabling fb ad twitter buttons
		var buttonFacebook =  GameObject.FindObjectOfType<ButtonShareFacebook>().gameObject;
		var buttonTwitter =  GameObject.FindObjectOfType<ButtonShareTwitter>().gameObject;
		var buttonGamecenter =  GameObject.FindObjectOfType<GameServicesSubmit>().gameObject;
		buttonGamecenter.transform.localPosition = buttonFacebook.transform.localPosition;
		buttonFacebook.SetActive(false);
		buttonTwitter.SetActive(false);
		buttonGamecenter.transform.parent.FindChild("label facebook coins").gameObject.SetActive(false);
		buttonGamecenter.transform.parent.FindChild("label twitter coins").gameObject.SetActive(false);
		#endif

		// Destroy Loaded Assets
		/*Destroy (PlayerController.Instance.bike);
		Destroy (PlayerController.Instance.ragdoll);

		Resources.UnloadUnusedAssets ();*/

		#if !UNITY_EDITOR
		if(!CheckRated.Instance.RateButtonClicked)
			UniRate.Instance.PromptIfNetworkAvailable();
		#endif
	}

	public GameObject pauseScreen;
	public void ShowPauseScreen(bool active)
	{
		pauseScreen.SetActive(active);
	}

	public string levelName{ get { return levelController.uiParams.levelName; } }

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile DeathScreen _staticInstance;	
	public static DeathScreen Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(DeathScreen)) as DeathScreen;
					if (_staticInstance == null) {
						Debug.LogError("The DeathScreen instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
