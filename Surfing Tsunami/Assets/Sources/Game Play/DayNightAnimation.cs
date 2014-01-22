using UnityEngine;
using System.Collections;

public class DayNightAnimation : DayNightItem {

	public bool startTrack=true;
	public string animName;
	private tk2dAnimatedSprite sprite;

	void Awake()
	{
		sprite = GetComponent<tk2dAnimatedSprite> ();
	}

	void Start()
	{
		if(startTrack)
			Go (LevelInfo.State.day);
	}

	public override void Go (bool day)
	{
		var currentTime = sprite.ClipTimeSeconds;
		sprite.Play(animName + (day?"":"_Night"),currentTime);
	} 

}
