using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {

	private ButtonBack backButton;
	private bool ended = false;

	void Awake()
	{
		backButton = GameObject.FindObjectOfType<ButtonBack> ();
	}

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
			backButton.gameObject.SetActive(false);
		yield return new WaitForSeconds (8f);
		var fw = transform.Find ("Fireworks");
		foreach(Transform t in fw)
		{
			if(t.gameObject.GetComponent<Fireworks>() != null)
				Destroy(t.gameObject.GetComponent<Fireworks>());
		}

		yield return new WaitForSeconds (3.1f);
		ended = true;
	}
}
