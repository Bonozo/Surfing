using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISprite))]
public class MenuTitle : MonoBehaviour {

	public string[] spriteName;
	public AudioClip[] clips;
	public bool pixelPerfect = true;
	private UISprite sprite;

	void OnEnable()
	{
		int completed = PlayerPrefs.GetInt ("completed_games", 0);
		completed = Mathf.Min (completed, 3);
		sprite = GetComponent<UISprite> ();
		sprite.spriteName = spriteName [completed];
		if(pixelPerfect)
			sprite.MakePixelPerfect ();
		AudioManager.Instance.PlayClip(clips[completed]);
	}
}
