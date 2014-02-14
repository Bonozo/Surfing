using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

	public GameObject tableOfContests;

	void Awake()
	{
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
