using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class RandomSprite : MonoBehaviour {

	public string[] spriteName;
	public bool pixelPerfect = true;
	private UISprite sprite;

	void Awake()
	{
		sprite = GetComponent<UISprite> ();
	}

	void OnEnable()
	{
		sprite.spriteName = spriteName [Random.Range (0, spriteName.Length)];
		if(pixelPerfect)
			sprite.MakePixelPerfect ();
	}
}
