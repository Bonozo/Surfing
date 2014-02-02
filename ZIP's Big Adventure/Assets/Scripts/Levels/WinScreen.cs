using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {

	bool started = false;
	void OnClick()
	{
		if(!started)
		{
			started = true;
			StartCoroutine (EndGame ());
		}
	}

	IEnumerator EndGame()
	{
		foreach(Transform t in transform)
		{
			if(t.gameObject.GetComponent<Fireworks>() != null)
				Destroy(t.gameObject);
		}

		yield return new WaitForSeconds (0.25f);
		Loader.sceneName = "title";
		Application.LoadLevel ("loader");
	}
}
