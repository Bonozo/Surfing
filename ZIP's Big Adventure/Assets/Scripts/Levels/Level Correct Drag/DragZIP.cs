﻿using UnityEngine;
using System.Collections;

public class DragZIP : MonoBehaviour {

	public LevelCorrectDrag level;

	private bool inited = false;
	private Vector3 initialPosition;
	private float dragTime;

	void Awake()
	{
		initialPosition = transform.localPosition;
	}

	public void Reset()
	{
		if(!inited)
		{
			initialPosition = transform.localPosition;
			inited = true;
		}
		transform.localPosition = initialPosition;
		dragTime = 0.5f;
		collider.enabled = true;
	}

	public void DisableCollider()
	{
		collider.enabled = false;
	}

	void OnDrag()
	{
		dragTime = 0.5f;
	}

	void Update()
	{
		if(dragTime>0f) dragTime -= Time.deltaTime;
	}

	void OnTriggerStay(Collider other)
	{
		if(dragTime<=0f)
		{
			level.Answered(other.gameObject);
		}
	}
}
