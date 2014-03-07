using UnityEngine;
using System.Collections;

public class EndItemLeaveScreen : EndItem {

	private bool saved=false;
	private Vector3 savePosition;
	private Quaternion saveRotation;
	
	public override void Reset ()
	{
		iTween.Stop (gameObject, false);
		StopAllCoroutines ();
		if(!saved)
		{
			savePosition = transform.localPosition;
			saveRotation = transform.localRotation;
			saved = true;
		}
		transform.localPosition = savePosition;
		transform.localRotation = saveRotation;
	}

	public override void Work ()
	{
		//Reset ();
		StartCoroutine (WorkThread ());
	}

	private IEnumerator WorkThread()
	{
		float radius = 3000f;
		float alpha = Random.Range (0f, 360f);
		Vector3 rand = new Vector3 (radius * Mathf.Sin (alpha), radius * Mathf.Cos (alpha), 0f);
		iTween.MoveTo(gameObject,iTween.Hash("position",rand,"time",12f,"islocal",true));
		float time = 10f;
		float rot = 30f;
		while(time>0f){
			time -= Time.deltaTime;
			transform.Rotate(0f,0f,rot);
			yield return new WaitForEndOfFrame();
		}
	}
}
