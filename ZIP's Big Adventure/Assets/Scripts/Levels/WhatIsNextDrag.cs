using UnityEngine;
using System.Collections;

public class WhatIsNextDrag : MonoBehaviour {

	public LevelWhatIsNext level;
	public Collider targetCollider;
	
	private Vector3 initialPosition;
	private float dragTime;

	void Awake()
	{
		// Adjust collider size: minimum 150x150
		BoxCollider box = collider as BoxCollider;
		Vector3 sz = new Vector3 (150f, 150f, box.size.z);
		sz.x = Mathf.Max (1f, sz.x / transform.localScale.x);
		sz.y = Mathf.Max (1f, sz.y / transform.localScale.y);
		box.size = sz;
	}

	public void Reset()
	{
		if(initialPosition == Vector3.zero)
			initialPosition = transform.localPosition;
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

	void OnTriggerStay(Collider other)
	{
		if(other.collider == targetCollider)
		{
			dragTime -= Time.deltaTime;
			if(dragTime <= 0f)
			{
				level.Answered(this);
				dragTime = 0.5f;
			}
		}
	}
}
