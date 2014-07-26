using UnityEngine;
using System.Collections;

// There is an error here
[AddComponentMenu("EndItems/Move To")]
public class EndItemMoveTo : EndItem {

	public float duration;
	public float delay;
	public bool resetBefore = true;

	public bool defaultUp = true;
	public bool linear = false;

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
		StopAllCoroutines ();
		if(!saved)
		{
			savePosition = transform.localPosition;
			saved = true;
		}
		transform.localPosition = savePosition;
	}
	
	public override void Work ()
	{
		if(resetBefore)
			Reset ();
		StartCoroutine ("Working");
	}

	private IEnumerator Working()
	{
		yield return new WaitForSeconds (delay);
		if(linear)
			iTween.MoveTo(gameObject,iTween.Hash("position",to,"time",duration,"islocal",true,"easetype",iTween.EaseType.linear));
		else
			iTween.MoveTo(gameObject,iTween.Hash("position",to,"time",duration,"islocal",true));
	}

	#if UNITY_EDITOR
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			Work();
	}
	#endif
}
