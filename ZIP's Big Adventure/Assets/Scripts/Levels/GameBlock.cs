using UnityEngine;
using System.Collections;

public class GameBlock : MonoBehaviour {

	public ZIPLevel[] level;
	public LevelPath path;
	public bool showIntro = true;
	public GameBlockIntro intro;
	public GameObject winToggle;

	private int gameIndex=0;

	void OnEnable()
	{
		StartCoroutine(StartGame());
	}

	IEnumerator StartGame()
	{
		yield return new WaitForEndOfFrame();
		collider.enabled = false;
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
	}

	void OnClick()
	{
		collider.enabled = false;
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
			GameController.Instance.Toggle(gameObject,winToggle);
		}
		else
		{
			level[gameIndex].StartGame();
		}
	}

}
