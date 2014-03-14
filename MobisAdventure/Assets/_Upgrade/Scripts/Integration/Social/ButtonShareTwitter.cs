using UnityEngine;
using System.Collections;

public class ButtonShareTwitter : MonoBehaviour {

	public UILabel message;
	public GameObject loading;
	
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
