using UnityEngine;
using System.Collections;

public class ButtonShareTwitter : MonoBehaviour {

	bool started = false;
	void OnClick()
	{
		if(!started)
		{
			started = true;
			StartCoroutine (ShareOnFacebook ());
		}
	}

	IEnumerator ShareOnFacebook()
	{
		yield return null;
	}
}
