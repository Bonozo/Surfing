using UnityEngine;
using System.Collections;

[AddComponentMenu("EndItems/Change Texture")]
public class EndItemChangeTexture : EndItem {

	public string newSpriteName;
	public Vector3 newScale;
	public float duration;
	public float delay;
	
	private bool saved=false;
	private UISprite sprite;
	private string oldSpriteName;
	private Vector3 saveScale;

	public override void Reset ()
	{
		iTween.Stop (gameObject, false);
		StopAllCoroutines ();
		if(!saved)
		{
			sprite = GetComponent<UISprite>();
			oldSpriteName = sprite.spriteName;
			saveScale = transform.localScale;
			saved=true;
		}
		sprite.spriteName = oldSpriteName;
		transform.localScale = saveScale;
	}

	public override void Work ()
	{
		Reset ();
		StartCoroutine ("Working");
	}

	IEnumerator Working()
	{
		yield return new WaitForSeconds(delay);
		iTween.ScaleTo(gameObject,iTween.Hash("scale",new Vector3(1f,1f,1f) ,"time",duration,"islocal",true));
		yield return new WaitForSeconds(duration);
		sprite.spriteName = newSpriteName;
		iTween.ScaleTo(gameObject,iTween.Hash("scale",newScale ,"time",duration,"islocal",true));
		yield return new WaitForSeconds(duration);
	}

	#if UNITY_EDITOR
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			Work();
	}
	#endif
}
