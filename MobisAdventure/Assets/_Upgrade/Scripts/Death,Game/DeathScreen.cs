using UnityEngine;
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
		yield return new WaitForSeconds(1f);
		bgAlphaTween.PlayForward();
		yield return new WaitForSeconds(bgAlphaTween.duration);
		stats.SetActive(true);
		if(!CheckRated.Instance.RateButtonClicked)
			UniRate.Instance.PromptIfNetworkAvailable();
	}

	public GameObject pauseScreen;
	public void ShowPauseScreen(bool active)
	{
		pauseScreen.SetActive(active);
	}

	public string levelName{ get { return levelController.levelName; } }

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
