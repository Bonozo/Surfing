using UnityEngine;
using System.Collections;

public class DeathScreen : MonoBehaviour {

	public TweenAlpha bgAlphaTween;
	public GameObject stats;
	public GameObject controlsScreen;
	public FailedScreen failedScreen;
	public UILabel labelCause;
	public UILabel labelStates;
	public UILabel labelCoins;
	public UILabel labelLevel;
	public LevelController levelController;
	public MessageBox messageBox;
	public UILabel countDown;
	
	public int LastDistanceTravelled{ get; private set;}
	public string LastLevel{ get { return levelName.Substring (6); } }

	[HideInInspector]public bool resultMonsterCause;
	[HideInInspector]public int resultDistance;
	[HideInInspector]public int resultBest;
	[HideInInspector]public int resultScore;

	
	public void Show(bool monstercause,int distance,int best,int score)
	{
		/*if(monstercause) labelCause.text = "the monster\ncaught you!";
		else labelCause.text = "you\ncrashed!";

		string[] sll = GameManager.m_chosenLevel.ToString().Split('_');
		labelStates.text = "" + distance + "m\n" + best + "m (best)\n in " + sll[1].ToLower();
		labelCoins.text = "+" + score;
		labelLevel.text = "Level: " + levelController.CurrentLevel;*/

		resultMonsterCause = monstercause;
		resultDistance = distance;
		resultBest = best;
		resultScore = score;

		StartCoroutine(ShowThread(monstercause, distance, best, score));
	}

	IEnumerator ShowThread(bool monstercause,int distance,int best,int score)
	{
		LastDistanceTravelled = distance;

		// Drop the volume and fade out
		MusicLoop.Instance.DropVolume (0.4f, 1f);
		PlayerController.Instance.StartCoroutine(PlayerController.Instance.FadeOut ());

		float time = RealTime.time + 0.5f;
		while(time > RealTime.time) yield return new WaitForEndOfFrame();
		Time.timeScale = 0.0f;
		
		failedScreen.Show (monstercause, distance, best, score);

		/*yield return new WaitForSeconds(1f);
		bgAlphaTween.PlayForward();
		yield return new WaitForSeconds(bgAlphaTween.duration);
		stats.SetActive(true);*/

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
	}

	public GameObject pauseScreen;
	public void ShowPauseScreen(bool active)
	{
		pauseScreen.SetActive(active);
	}

	public string levelName{ get { return levelController.uiParams.levelName; } }

	public void ShowMessageAndDoNothingMore(string message){
		StartCoroutine (ShowMessageAndUnpause (message,false));
	}

	public void ShowMessageAndUnPause(string message){
		StartCoroutine (ShowMessageAndUnpause (message,true));
	}

	private IEnumerator ShowMessageAndUnpause(string message,bool unpause){
		yield return StartCoroutine(messageBox.Show(message));
		if(unpause)
			Time.timeScale = 1f;
	}

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
