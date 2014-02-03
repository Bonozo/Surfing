using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public UILabel labelText;
	public TextMesh textMeshLevel;

	private int reachDelta = 500;
	private float tweenScaleTime = 0.6f;

	private PlayerController player;
	private string id;
	int nextLevel;

	void Awake()
	{
		player = GameObject.FindObjectOfType (typeof(PlayerController)) as PlayerController;
		id = "level_" + Application.loadedLevelName;
		nextLevel = PlayerPrefs.GetInt (id, 1);
		textMeshLevel.text = "LEVEL: " + nextLevel;
		labelText.gameObject.SetActive (false);
	}

	IEnumerator Start()
	{
		yield return new WaitForSeconds (1f);
		StartCoroutine("ShowTarget");
	}

	void Update()
	{
		var dist = player.p_distTraveled;
		if( dist >= nextLevel*reachDelta )
		{		
			nextLevel++;
			PlayerPrefs.SetInt(id,nextLevel);
			PlayerPrefs.Save ();
			StartCoroutine("Complete");
		}
	}

	IEnumerator ShowTarget()
	{
		labelText.text = "Level " + nextLevel + "\n" +
			"Reach " + nextLevel*reachDelta + " meters";
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

	IEnumerator Complete()
	{
		int lvl = nextLevel - 1;
		int bonusCoins = 500 * lvl;
		int current = 0;
		int delta = lvl==1?50:100;
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
			yield return new WaitForSeconds(0.05f);
		}

		yield return new WaitForSeconds (1f);

		StartCoroutine ("ShowTarget");
		textMeshLevel.text = "LEVEL: " + nextLevel;

	}
}
