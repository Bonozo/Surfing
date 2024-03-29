﻿using UnityEngine;
using System.Collections;

public class LevelRightAnswerButton : MonoBehaviour {

	public LevelRightAnswer level;
	
	public void Reset()
	{
		collider.enabled = true;
	}
	
	public void DisableCollider()
	{
		collider.enabled = false;
	}
	
	void OnClick()
	{
		level.Answered (this);
	}
}
