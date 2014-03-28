using UnityEngine;
using System.Collections;

public class OpenLinkWithReward : MonoBehaviour {

	public string url;
	public int reward = 5000;
	public bool disableButton = false;
	public GameObject disableGameObject = null;
	private bool dowork = false;
	private bool blocked = false;

	void Awake()
	{
		if( PlayerPrefs.GetInt("opened"+url,0)==1)
			Block();
	}

	void OnClick()
	{
		if(dowork) return;
		if(blocked) return;
		Application.OpenURL(url);
		if( PlayerPrefs.GetInt("opened"+url,0)==0)
			StartCoroutine(DoWork());
	}

	void Block()
	{
		blocked = true;
		if(disableGameObject!=null) disableGameObject.SetActive(false);
		if(disableButton)
		{
			transform.FindChild("Name").GetComponent<UILabel>().color = new Color(0.85f,0.85f,0.85f,1f);
			transform.FindChild("Background").GetComponent<UISprite>().color = new Color(0.6f,0.6f,0.6f,1f);
		}
	}

	IEnumerator DoWork()
	{
		dowork = true;

		yield return new WaitForSeconds (2f);
		Block ();
		int coins = PlayerPrefs.GetInt("pp_coins");
		coins += reward;
		PlayerPrefs.SetInt("pp_coins",coins);
		PlayerPrefs.SetInt ("opened" + url, 1);
		PlayerPrefs.Save();

		dowork = false;
	}
}

