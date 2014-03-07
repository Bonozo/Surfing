using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class EndItemColor : EndItem {

	public Color to;
	public float duration;
	public float delay;

	private bool saved=false;
	private Color saveColor;
	private UISprite sprite;
	
	public override void Reset ()
	{
		StopCoroutine ("Working");
		if(!saved)
		{
			sprite = GetComponent<UISprite>();
			saveColor = sprite.color;
			saved = true;
		}
		sprite.color = saveColor;
	}
	
	public override void Work ()
	{
		//Reset ();
		StartCoroutine ("Working");
	}

	IEnumerator Working()
	{
		yield return new WaitForSeconds (delay);
		TweenColor.Begin (gameObject, duration, to);
	}

}
