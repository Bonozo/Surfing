using UnityEngine;
using System.Collections;

public class EndItemGoRootIfCollider : EndItem {

	public Transform root;

	private bool saved=false;
	private Transform parent;

	public override void Reset ()
	{
		if(!saved)
		{
			parent = transform.parent;
		}
		transform.parent = parent;
	}

	public override void Work ()
	{
		if(collider.enabled)
			transform.parent = root;
	}
}
