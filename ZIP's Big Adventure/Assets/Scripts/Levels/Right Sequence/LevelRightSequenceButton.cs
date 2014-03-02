using UnityEngine;
using System.Collections;

public class LevelRightSequenceButton : MonoBehaviour {

	public LevelRightSequence level;
	public int value;

	private UISprite sprite;
	public void Reset()
	{
		sprite = transform.GetChild (0).GetComponent<UISprite> ();
		sprite.color = Color.white;
		StartCoroutine ("Init");
	}

	void OnPress(bool isDown)
	{
		if(isDown)
			level.Answer (this);
	}

	public void PlayAnim(bool correct)
	{
		if(!correct)
			StartCoroutine("WrongAnswer");
		else
			StartCoroutine("RightAnswer");
	}

	private IEnumerator Init()
	{
		collider.enabled = false;
		Vector3 saveScale = sprite.transform.localScale;
		sprite.transform.localScale = new Vector3 (0f, 0f, 1f);
		TweenScale.Begin (sprite.gameObject, 0.4f, saveScale);
		yield return new WaitForSeconds(0.4f);
		collider.enabled = true;
	}

	private IEnumerator WrongAnswer()
	{
		collider.enabled = false;
		Vector3 saveScale = sprite.transform.localScale;
		TweenColor.Begin (sprite.gameObject, 0.2f, new Color (0.5f, 0.5f, 0.5f, 1f));
		TweenScale.Begin (sprite.gameObject, 0.2f, saveScale * 0.9f);
		yield return new WaitForSeconds(0.2f);
		TweenColor.Begin (sprite.gameObject, 0.2f, new Color (1f, 1f, 1f, 1f));
		TweenScale.Begin (sprite.gameObject, 0.2f, saveScale);
		yield return new WaitForSeconds(0.2f);
		collider.enabled = true;
	}

	private IEnumerator RightAnswer()
	{
		collider.enabled = false;
		Vector3 saveScale = sprite.transform.localScale;
		TweenColor.Begin (sprite.gameObject, 0.5f, new Color (1f, 1f, 1f, 0f));
		TweenScale.Begin (sprite.gameObject, 0.5f, saveScale * 1.1f);
		yield return new WaitForSeconds (0.5f);
		sprite.transform.localScale = saveScale;
	}

	public void Initialize()
	{
		var sprite = transform.GetChild (0).GetComponent<UISprite> ();
		name = "" + value;
		sprite.spriteName = "" + value;
		sprite.type = UISprite.Type.Simple;
		sprite.MakePixelPerfect ();
		sprite.type = UISprite.Type.Sliced;

		var pos = transform.localPosition;
		pos.x = Mathf.Round (pos.x);
		pos.y = Mathf.Round (pos.y);
		transform.localPosition = pos;
	}
}
