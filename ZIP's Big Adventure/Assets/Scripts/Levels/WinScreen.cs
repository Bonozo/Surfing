using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {

	public GameObject backButton;
	bool ended = false;

	void OnEnable()
	{
		StartCoroutine (EndGame ());
	}

	void Update()
	{
		if(ended)
		{
			ended = false;
			Loader.sceneName = "title";
			Application.LoadLevel ("loader");
		}
	}

	IEnumerator EndGame()
	{
		PlayerPrefs.SetInt (Application.loadedLevelName, 1);

		int cmpt = PlayerPrefs.GetInt ("completed_games", 0);
		PlayerPrefs.SetInt ("completed_games", cmpt + 1);

		PlayerPrefs.Save ();
		if(backButton != null)
			backButton.SetActive(false);
		yield return new WaitForSeconds (8f);
		foreach(Transform t in transform)
		{
			if(t.gameObject.GetComponent<Fireworks>() != null)
				Destroy(t.gameObject.GetComponent<Fireworks>());
		}

		yield return new WaitForSeconds (5f);
		ended = true;
	}
}
