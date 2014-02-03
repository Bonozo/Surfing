using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	private int[] levelDist = new int[]{0,0,1500,3000,4500,6000,7500,10000};
	private int[] levelBonus = new int[]{0,0,1000,1500,2000,2500,3000,4500};

	public UILabel labelText;
	public TextMesh textMeshLevel;
	
	private float tweenScaleTime = 0.6f;

	private PlayerController player;
	private string id;
	int nextLevel;

	public int CurrentLevel{
		get{
			var id = "level_" + Application.loadedLevelName;
			var currentLevel = PlayerPrefs.GetInt (id, 1);
			return currentLevel;
		}
	}
	
	void Awake()
	{
		player = GameObject.FindObjectOfType (typeof(PlayerController)) as PlayerController;
		id = "level_" + Application.loadedLevelName;
		textMeshLevel.text = "LEVEL: " + CurrentLevel;
		nextLevel = CurrentLevel + 1;
		labelText.gameObject.SetActive (false);
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds (1f);
		StartCoroutine("ShowTarget");
	}

	void Update()
	{
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
}
