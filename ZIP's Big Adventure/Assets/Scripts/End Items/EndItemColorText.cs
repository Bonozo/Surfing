using UnityEngine;
using System.Collections;

[AddComponentMenu("EndItems/Color")]
[RequireComponent(typeof(UILabel))]
public class EndItemColorText : EndItem {

	public Color to;
	public float duration;
	public float delay;

	private bool saved=false;
	private Color saveColor;
	private UILabel sprite;
	
	public override void Reset ()
	{
		StopCoroutine ("Working");
		if(!saved)
		{
			sprite = GetComponent<UILabel>();
			saveColor = sprite.color;
			saved = true;
		}
		sprite.color = saveColor;
	}
	
	public override void Work ()
	{
		Reset ();
		StartCoroutine ("Working");
	}

	IEnumerator Working()
	{
		yield return new WaitForSeconds (delay);
		TweenColor.Begin (gameObject, duration, to);
	}

}
