using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	public GameObject tableOfContests;
	public GameObject endGame;

	void Awake()
	{
		int completed = PlayerPrefs.GetInt ("completed_games", 0);
		if(completed == 4)
		{
			endGame.SetActive(true);
			gameObject.SetActive(false);
			return;
		}

		if(GameController.gameLevel != GameLevel.None)
		{
			tableOfContests.SetActive(true);
			gameObject.SetActive(false);
		}
		else
		{
			PlayerPrefs.DeleteAll();
		}
	}
}
