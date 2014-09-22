/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class MessageBox : MonoBehaviour {

	public UILabel labelMessage;
	public GameObject loading;
	public GameObject box;

	public IEnumerator ConnectToIAP()
	{
		box.SetActive (false);
		loading.SetActive (true);

		gameObject.SetActive (true);
		float tm = 5f;
		if(!MobiIAB.Instance.Connected) MobiIAB.Instance.RequestProductData();
		while(tm>0f && !MobiIAB.Instance.Connected)
		{
			tm-=Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(1f);

		if(!MobiIAB.Instance.Connected)
			StartCoroutine(Show("Unable to connect to the Server."));
		else
			gameObject.SetActive (false);
	}

	public IEnumerator Show(string message)
	{
		gotOk = false;
		loading.SetActive (false);
		box.SetActive (true);
		labelMessage.text = message;
		gameObject.SetActive (true);
		while(!gotOk) yield return null;
		gameObject.SetActive(false);
	}

	public void ShowLoading(bool status){
		box.SetActive (false);
		loading.SetActive (status);
		gameObject.SetActive (status);
	}

	private bool gotOk = false;
	public void OK()
	{
		gotOk = true;
	}
}
