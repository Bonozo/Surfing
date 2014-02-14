using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {

	//bool started = false;
	bool ended = false;
	/*void OnClick()
	{
		if(!started)
		{
			started = true;
			StartCoroutine (EndGame ());
		}
	}*/

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
		
		/*yield return new WaitForSeconds (8f);
		foreach(Transform t in transform)
		{
			if(t.gameObject.GetComponent<Fireworks>() != null)
				Destroy(t.gameObject.GetComponent<Fireworks>());
		}
*/
		yield return new WaitForSeconds (5f);
		ended = true;
	}
}
