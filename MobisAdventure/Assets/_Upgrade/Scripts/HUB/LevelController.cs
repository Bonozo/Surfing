using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public UIParams uiParams;
	private int[] levelDist = new int[]{0,0,1500,3000,4500,6000,7500,10000};
	private int[] levelBonus = new int[]{0,0,1000,1500,2000,2500,3000,4500};
	
	public UILabel labelText;
	public UILabel labelBackflip;
	public UILabel labelFrontflip;
	public TextMesh textMeshLevel;
	
	private float tweenScaleTime = 0.6f;

	private PlayerController player;
	private string id;
	int nextLevel;

	public int CurrentLevel{
		get{
			id = "level_" + uiParams.levelName;
			var currentLevel = PlayerPrefs.GetInt (id, 1);
			return currentLevel;
		}
	}
	
	void Awake()
	{
		player = GameObject.FindObjectOfType (typeof(PlayerController)) as PlayerController;		
		id = "level_" + uiParams.levelName;
		textMeshLevel.text = "LEVEL: " + CurrentLevel;
		nextLevel = CurrentLevel + 1;
		labelText.gameObject.SetActive (false);
		labelBackflip.gameObject.SetActive (false);
		labelFrontflip.gameObject.SetActive (false);

		labelText.color = uiParams.uiColor;
		labelBackflip.color = uiParams.uiColor;
		labelFrontflip.color = uiParams.uiColor;
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds (1f);
		StartCoroutine("ShowTarget");
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			ShowBackflipBonus();
		if(nextLevel>=levelDist.Length) return;
		var dist = player.p_distTraveled;
		if( dist >= levelDist[nextLevel] )
		{		
			PlayerPrefs.SetInt(id,nextLevel);
			PlayerPrefs.Save ();
			nextLevel++;
			StartCoroutine("Complete");
		}
	}

	IEnumerator ShowTarget()
	{
		if(nextLevel<levelDist.Length){
			labelText.text = "Level " + nextLevel + "\n" +
				"Reach " + levelDist[nextLevel] + " meters";
			labelText.transform.localScale = new Vector3(0f,0f,1f);

			labelText.gameObject.SetActive (true);
			iTween.ScaleTo (labelText.gameObject, iTween.Hash("scale", new Vector3(1.5f,1.5f,1f),
				"time", tweenScaleTime, "easeType", iTween.EaseType.spring));
			//TweenScale.Begin(labelText.gameObject,tweenScaleTime,new Vector3(1.5f,1.5f,1f));
			yield return new WaitForSeconds (4f);
			iTween.ScaleTo (labelText.gameObject, iTween.Hash("scale", new Vector3(0f,0f,1f),
				"time", 0.4f*tweenScaleTime, "easeType", iTween.EaseType.linear));
			//TweenScale.Begin(labelText.gameObject,tweenScaleTime,new Vector3(0f,0f,1f));
			yield return new WaitForSeconds (tweenScaleTime);
			
			labelText.gameObject.SetActive (false);

			yield return null;
		}
	}

	IEnumerator Complete()
	{
		int lvl = nextLevel - 1;
		int bonusCoins = levelBonus [lvl];
		int current = 0;
		int delta = 100;
		float yd = 1f / ((float)bonusCoins / (float)delta);

		labelText.text = "Level " + lvl + " reached\n" +
			"Bonus 0 pt";
		labelText.gameObject.SetActive (true);
		iTween.ScaleTo (labelText.gameObject, iTween.Hash("scale", new Vector3(1.5f,1.5f,1f),
			"time", tweenScaleTime, "easeType", iTween.EaseType.spring));
		yield return new WaitForSeconds (1f);

		while(bonusCoins>0)
		{
			int ddx = Mathf.Min(delta,bonusCoins);	
			bonusCoins -= ddx;
			current += ddx;
			Coin_Counter.AddCoins(ddx);
			labelText.text = "Level " + lvl + " reached\n" +
				"Bonus " + current + " pt";
			yield return new WaitForSeconds(yd);
		}

		yield return new WaitForSeconds (1f);

		labelText.gameObject.SetActive (false);
		StartCoroutine ("ShowTarget");
		textMeshLevel.text = "LEVEL: " + lvl;

	}

	#region Flips

	public void ShowBackflipBonus()
	{
		StopCoroutine("ShowBackflipThread");
		iTween.Stop (labelBackflip.gameObject, false);
		StartCoroutine ("ShowBackflipThread");
	}

	public void ShowFrontflipBonus()
	{
		StopCoroutine("ShowFrontflipThread");
		iTween.Stop (labelFrontflip.gameObject, false);
		StartCoroutine ("ShowFrontflipThread");
	}

	private IEnumerator ShowFrontflipThread()
	{
		yield return new WaitForEndOfFrame ();

		PlayerController.Instance.audio.PlayOneShot(MobiAssets.Instance.clipBoostPowerup);
		labelFrontflip.transform.localScale = new Vector3 (0f, 0f, 1f);
		labelFrontflip.text = "Front flip\n+1000 pt";
		NGUITools.SetActive (labelFrontflip.gameObject, true);
		iTween.ScaleTo (labelFrontflip.gameObject, iTween.Hash("scale", new Vector3(1.5f,1.5f,1f),
		                                                      "time", 0.3f, "easeType", iTween.EaseType.spring));
		yield return new WaitForSeconds (2f);
		iTween.ScaleTo (labelFrontflip.gameObject, iTween.Hash("scale", new Vector3(0f,0f,1f),
		                                                  "time", 0.1f, "easeType", iTween.EaseType.linear));
		yield return new WaitForSeconds (0.1f);
		NGUITools.SetActive (labelFrontflip.gameObject, false);
	}

	private IEnumerator ShowBackflipThread()
	{
		yield return new WaitForEndOfFrame ();
		
		PlayerController.Instance.audio.PlayOneShot(MobiAssets.Instance.clipBoostPowerup);
		labelBackflip.transform.localScale = new Vector3 (0f, 0f, 1f);
		labelBackflip.text = "Back flip\n+1000 pt";
		NGUITools.SetActive (labelBackflip.gameObject, true);
		iTween.ScaleTo (labelBackflip.gameObject, iTween.Hash("scale", new Vector3(1.5f,1.5f,1f),
		                                                      "time", 0.3f, "easeType", iTween.EaseType.spring));
		yield return new WaitForSeconds (2f);
		iTween.ScaleTo (labelBackflip.gameObject, iTween.Hash("scale", new Vector3(0f,0f,1f),
		                                                  "time", 0.1f, "easeType", iTween.EaseType.linear));
		yield return new WaitForSeconds (0.1f);
		NGUITools.SetActive (labelBackflip.gameObject, false);

		/*int delta = 100;
		int bonusCoins = 1000;
		labelBackflip.text = "Back flip\nBonus 0 pt";

		labelBackflip.transform.localScale = new Vector3 (0f, 0f, 1f);
		NGUITools.SetActive (labelBackflip.gameObject, true);
		iTween.ScaleTo (labelBackflip.gameObject, iTween.Hash("scale", new Vector3(1.5f,1.5f,1f),
		                                                  "time", 0.3f, "easeType", iTween.EaseType.spring));
		yield return new WaitForSeconds (0.3f);
		while(bonusCoins>0)
		{
			int ddx = Mathf.Min(delta,bonusCoins);	
			bonusCoins -= ddx;
			Coin_Counter.AddCoins(ddx);
			labelBackflip.text = "Back flip\nBonus " + (1000-bonusCoins) + " pt";
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds (0.5f);
		NGUITools.SetActive (labelBackflip.gameObject, false);*/
	}

	#endregion
}
