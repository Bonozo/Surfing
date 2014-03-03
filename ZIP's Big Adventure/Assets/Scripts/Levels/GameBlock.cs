using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBlock : MonoBehaviour {

	public bool randomizedGames = true;
	public bool allLevelsDetermined = false;
	public ZIPLevel[] level;
	public LevelPath path;
	public bool showIntro = true;
	public GameBlockIntro intro;
	public UILabel labelIntro;
	public GameObject winToggle;

	private int gameIndex=0;

	void OnEnable()
	{
		// Set up Levels
		if(randomizedGames)
		{
			List<ZIPLevel> levels = new List<ZIPLevel>();
			GameObject gmb;

			if(GameController.gameLevel == GameLevel.First || !allLevelsDetermined)
			{
				gmb = GameObject.Find("First");
				foreach(Transform g in gmb.transform)
					foreach(Transform gg in g)
						levels.Add(gg.GetComponent<ZIPLevel>());
			}

			if(GameController.gameLevel == GameLevel.Kindergarten || !allLevelsDetermined)
			{
				gmb = GameObject.Find("Kindergarten");
				foreach(Transform g in gmb.transform)
					foreach(Transform gg in g)
						levels.Add(gg.GetComponent<ZIPLevel>());
			}

			if(GameController.gameLevel == GameLevel.PreK || !allLevelsDetermined)
			{
				gmb = GameObject.Find("PreK");
				foreach(Transform g in gmb.transform)
					foreach(Transform gg in g)
						levels.Add(gg.GetComponent<ZIPLevel>());
			}

			if(levels.Count < level.Length)
			{
				collider.enabled = false;
				Debug.Log("There is no enought levels.");
				return;
			}

			for (int i = 0; i < levels.Count; i++) {
				var temp = levels[i];
				int randomIndex = Random.Range(i, levels.Count);
				levels[i] = levels[randomIndex];
				levels[randomIndex] = temp;
			}
			for(int i=0;i<level.Length;i++)
				level[i] = levels[i];
		}

		StartCoroutine(StartGame());
	}

	IEnumerator StartGame()
	{
		yield return new WaitForEndOfFrame();

		collider.enabled = false;
		labelIntro.gameObject.SetActive (false);
		path.Reset();
		gameIndex=0;
		path.PlayFirstAnims ();
		if(showIntro){
			yield return intro.StartCoroutine (intro.ComeAround ());
			collider.enabled = true;
		}
		else{
			if(intro != null)
				intro.Reset();
			yield return new WaitForSeconds(3f);
			path.PauseAnims();
			ShowNextLevel();
		}
		labelIntro.gameObject.SetActive (true);
	}

	void OnClick()
	{
		collider.enabled = false;
		labelIntro.gameObject.SetActive (false);
		StartCoroutine (StartLevels ());
	}

	IEnumerator StartLevels()
	{
		yield return intro.StartCoroutine (intro.GoAway ());
		path.PauseAnims ();
		yield return new WaitForSeconds (1f);
		ShowNextLevel();
	}

	public void LevelComplete()
	{
		level[gameIndex].gameObject.SetActive(false);
		gameIndex++;
		ShowNextLevel();
	}

	private void ShowNextLevel()
	{
		if(gameIndex>=level.Length)
		{
			GameController.Instance.SimpleToggle(gameObject,winToggle);
		}
		else
		{
			level[gameIndex].StartGame();
		}
	}

}
