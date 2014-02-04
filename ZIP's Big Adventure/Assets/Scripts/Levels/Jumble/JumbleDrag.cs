using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class JumbleDrag : MonoBehaviour {

	public LevelJumble level;
	public Collider[] targetCollider;
	
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
		foreach(var col in targetCollider)
		{
			col.enabled = true;
			col.GetComponent<UISprite> ().spriteName = "gray_box";
		}
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
		dragTime -= Time.deltaTime;
		if(dragTime <= 0f)
		{
			dragTime = 0.5f;

			foreach(var col in targetCollider)
			{
				if( other.collider == col)
				{
					StartCoroutine(HappyEnd(col));
					GameController.Instance.PlayCorrectAnswer();
					return;
				}
			}

			// Not a correct collider
			GameController.Instance.PlayWrongAnswer();
			StartCoroutine(BackToInitialPlace());
		}
	}

	IEnumerator HappyEnd(Collider col)
	{
		DisableCollider ();
		col.enabled = false;
		iTween.MoveTo(gameObject,iTween.Hash("position",col.transform.localPosition
		                                     ,"time",1f,"islocal",true));
		yield return new WaitForSeconds(0.1f);
		col.GetComponent<UISprite> ().spriteName = "yellow_box";
		yield return new WaitForSeconds(0.9f);
		level.Answered (this);
	}

	IEnumerator BackToInitialPlace()
	{
		collider.enabled = false;
		iTween.MoveTo(gameObject,iTween.Hash("position",initialPosition
		                                     ,"time",0.5f,"islocal",true));
		yield return new WaitForSeconds(0.51f);
		collider.enabled = true;
	}
}
