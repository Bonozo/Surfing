using UnityEngine;
using System.Collections;

[AddComponentMenu("EndItems/Change Text")]
public class EndItemChangeText : EndItem {

	public string newText;
	public float duration;
	public float delay;
	
	private bool saved=false;
	private UILabel label;
	private string oldText;
	private Vector3 saveScale;

	public override void Reset ()
	{
		iTween.Stop (gameObject, false);
		StopAllCoroutines ();
		if(!saved)
		{
			label = GetComponent<UILabel>();
			oldText = label.text;
			saveScale = transform.localScale;
			saved=true;
		}
		label.text = oldText;
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
		iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(1f,1f,1f) ,"time",duration,"islocal",true));
		yield return new WaitForSeconds(duration+0.2f);
		label.text = newText;
		iTween.ScaleTo(gameObject,iTween.Hash("scale",saveScale ,"time",duration,"islocal",true));
		yield return new WaitForSeconds(duration);
	}

	#if UNITY_EDITOR
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			Work();
	}
	#endif
}
