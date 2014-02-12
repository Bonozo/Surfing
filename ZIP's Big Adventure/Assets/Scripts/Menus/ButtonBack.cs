using UnityEngine;
using System.Collections;

public class ButtonBack : MonoBehaviour {

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
		Fireworks[] fw = GameObject.FindObjectsOfType<Fireworks> ();
		foreach(var ff in fw) Destroy(ff.gameObject);
		
		yield return new WaitForSeconds (0.25f);
		Loader.sceneName = "title";
		Application.LoadLevel ("loader");
	}
}
