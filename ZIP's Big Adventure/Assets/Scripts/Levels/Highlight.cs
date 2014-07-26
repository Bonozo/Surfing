using UnityEngine;
using System.Collections;

public class Highlight : MonoBehaviour {
	
	public Vector3 delta;
	public float delay;
	public float duration = 0.1f;
	public iTween.EaseType type;
	void OnEnable(){
		transform.localPosition += delta;
		StartCoroutine ("DoWork");
	}

	IEnumerator DoWork(){
		/*iTween.RotateBy (gameObject, new Vector3 (0f, 0f, by), time);
		yield return new WaitForSeconds (0.1f);
		iTween.RotateBy (gameObject, new Vector3 (0f, 0f, -by), time);*/
		yield return new WaitForSeconds (delay);
		iTween.MoveTo(gameObject,iTween.Hash("position",transform.localPosition-delta,"time",duration,"islocal",true
		                                     ,"easetype",type));

	}
}
