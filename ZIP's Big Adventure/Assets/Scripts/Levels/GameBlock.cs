using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameBlock : MonoBehaviour {

	public bool readyToPublish = false;
	public ZIPLevel[] level;
	public LevelPath path;
	public bool showIntro = true;
	public GameBlockIntro intro;
	public GameObject winToggle;

	private int gameIndex=0;

	void Awake()
	{
		if(readyToPublish)
		{
			// Load games into the memory
			var path = GameController.gameType + "/" + GameController.gameLevel;
			GameObject allgames = Instantiate(Resources.Load<GameObject>(path)) as GameObject;
			allgames.transform.parent = transform.FindChild("Levels");
			allgames.transform.localPosition = new Vector3(0f,0f,0f);
			allgames.transform.localScale = new Vector3(1f,1f,1f);
		
			// Prepare and choose random games
			List<ZIPLevel> levels = new List<ZIPLevel>();
			foreach(Transform group in allgames.transform)
				foreach(Transform single in group)
					if(single.GetComponent<ZIPLevel>() != null)
					{
						single.GetComponent<ZIPLevel>().gameBlock = this;
						levels.Add(single.GetComponent<ZIPLevel>());
					}

			if(levels.Count < level.Length){
				Debug.Log("There is no enought levels.");
				return;
			}

			for (int i = 0; i < levels.Count; i++) {
				var temp = levels[i];
				int randomIndex = Random.Range(i, levels.Count);
				levels[i] = levels[randomIndex];
				levels[randomIndex] = temp;
			}

			level = new ZIPLevel[7];
			for(int i=0;i<level.Length;i++)
				level[i] = levels[i];

			// Start game
			StartCoroutine(StartGame());
			return;
		}

		StartCoroutine(StartGame());
	}

	IEnumerator StartGame()
	{
		yield return new WaitForEndOfFrame();

		path.Reset();
		gameIndex=0;
		path.PlayFirstAnims ();
		if(showIntro){
			yield return intro.StartCoroutine (intro.ComeAround ());
		}
		else{
			if(intro != null)
				intro.Reset();
			yield return new WaitForSeconds(3f);
			path.PauseAnims();
			ShowNextLevel();
		}
	}

	public void Play()
	{
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

	#region Editor

	public Transform editorLevelsRoot;
	public int editorStartIndex;

	public void InitLevels()
	{
		for(int i=0;i<7;i++)
		{
			if(editorStartIndex+i < editorLevelsRoot.childCount)
			{
				level[i] = editorLevelsRoot.GetChild(editorStartIndex+i).GetComponent<ZIPLevel>();
			}
		}
	}

	#endregion
}
