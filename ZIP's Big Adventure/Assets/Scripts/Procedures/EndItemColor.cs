using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class EndItemColor : EndItem {

	public Color to;
	public float duration;

	private bool saved=false;
	private Color saveColor;
	private UISprite sprite;
	
	public override void Reset ()
	{
		iTween.Stop (gameObject, false);
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
		Reset ();
		TweenColor.Begin (gameObject, duration, to);
	}

}
