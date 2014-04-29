using UnityEngine;
using System.Collections;

[AddComponentMenu("EndItems/Scale To")]
public class EndItemScaleTo : EndItem {
	
	public Vector3 newScale;
	public float duration;
	public float delay;
	
	private bool saved=false;
	private Vector3 saveScale;
	
	public override void Reset ()
	{
		iTween.Stop (gameObject, false);
		StopAllCoroutines ();
		if(!saved)
		{
			saveScale = transform.localScale;
			saved=true;
		}
		transform.localScale = saveScale;
	}
	
	public override void Work ()
	{
		Reset ();
		StartCoroutine ("Working");
	}	

	IEnumerator Working()
	{
		yield return new WaitForSeconds(delay);
		iTween.ScaleTo(gameObject,iTween.Hash("scale",newScale,"time",duration,"islocal",true));
	}
}
