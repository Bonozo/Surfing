using UnityEngine;
using System.Collections;

// There is an error here
public class EndItemMoveTo : EndItem {

	public float duration;

	public bool defaultUp = true;
	public Vector3 to;
	private bool saved=false;
	private Vector3 savePosition;

	void Awake()
	{
		if(defaultUp)
			to = transform.localPosition + new Vector3 (0f, 2000f, 0f);
	}

	public override void Reset ()
	{
		iTween.Stop (gameObject, false);
		if(!saved)
		{
			savePosition = transform.localPosition;
			saved = true;
		}
		transform.localPosition = savePosition;
	}
	
	public override void Work ()
	{
		Reset ();
		iTween.MoveTo(gameObject,iTween.Hash("position",to,"time",duration,"islocal",true));
	}
}
