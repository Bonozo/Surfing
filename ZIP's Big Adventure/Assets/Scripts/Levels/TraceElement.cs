using UnityEngine;
using System.Collections;

public class TraceElement : MonoBehaviour {

	public int index;
	private Trace trace;

	void Awake()
	{
		trace = transform.parent.GetComponent<Trace>();
	}

	void Update()
	{
		if(collider.enabled)
		{
			if( UICamera.lastHit.collider == collider)
			{
				trace.Progress(this);
			}
		}
	}

/*	void OnDrop()
	{
		Debug.Log("Clicked " + index);
		trace.Progress(this);
	}*/
}
