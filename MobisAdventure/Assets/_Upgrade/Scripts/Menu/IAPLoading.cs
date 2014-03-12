using UnityEngine;
using System.Collections;

public class IAPLoading : MonoBehaviour {

	public IEnumerator ConnectToIAP()
	{
		gameObject.SetActive (true);
		yield return new WaitForSeconds(1f);
		float tm = 5f;
		while(tm>0f && !MobiIAB.Instance.Connected)
		{
			tm-=Time.deltaTime;
			yield return null;
		}
		gameObject.SetActive (false);
	}
}
