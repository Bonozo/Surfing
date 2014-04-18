﻿using UnityEngine;
using System.Collections;

public class ButtonBoost : MonoBehaviour {
	
	private TweenScale tweener;

	void Awake()
	{
		tweener = GetComponentInChildren<TweenScale> ();
	}

	void OnPress(bool isDown){
		PlayerController.Instance.controlBoost  = isDown;
	}

	public void Animate(bool value)
	{
		if(tweener.enabled && value) return;
		tweener.enabled = value;
		if(value) tweener.Play();
		else tweener.transform.localScale = new Vector3(1f,1f,1f);
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.D))
			Animate(true);
		if(Input.GetKeyDown(KeyCode.F))
			Animate(false);
	}
}
