using UnityEngine;
using System.Collections;

public class TraceElement : MonoBehaviour {

	public int index;
	public bool isElem;
	public AudioClip clip;

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
				if(clip!=null) AudioManager.Instance.PlayClip(clip);
				if(isElem) StartCoroutine(Highlight());
				trace.Progress(this);
			}
		}
	}

	IEnumerator Highlight(){
		var sc = transform.localScale;
		float tm = 0.1f;
		float by = 1.5f;
		float by2 = 1.25f;
		TweenScale.Begin (gameObject, tm, sc * by);
		yield return new WaitForSeconds(tm);
		TweenScale.Begin (gameObject, tm, sc * by2);
		yield return new WaitForSeconds(tm);
		transform.localScale = sc * by2;
	}
}
