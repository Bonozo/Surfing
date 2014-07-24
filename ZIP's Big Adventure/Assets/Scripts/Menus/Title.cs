using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	public GameObject title;
	public GameObject tableOfContests;
	public GameObject endGame;

	public static bool firstLaunch=true;

	void OnEnable()
	{
		if(firstLaunch)
		{
			PlayerPrefs.DeleteKey("completed_games");
			PlayerPrefs.DeleteKey("letters");
			PlayerPrefs.DeleteKey("numbers");
			PlayerPrefs.DeleteKey("patterns");
			PlayerPrefs.DeleteKey("shapes");
			PlayerPrefs.Save();
			firstLaunch = false;
			return;
		}
		int completed = PlayerPrefs.GetInt ("completed_games", 0);
		if(completed == 4)
		{
			endGame.SetActive(true);
			title.SetActive(false);
			return;
		}

		if(GameController.gameLevel != GameLevel.None)
		{
			tableOfContests.SetActive(true);
			title.SetActive(false);
		}
	}
}
