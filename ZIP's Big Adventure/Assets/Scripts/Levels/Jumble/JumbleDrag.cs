using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class JumbleDrag : MonoBehaviour {

	public LevelJumble level;
	public Collider[] targetCollider;
	public bool convertTargetToBox = true;
	public bool distanceMode = false;
	public bool fixCollider = true;

	private float distanceModeDistance =125f;
	private Vector3 initialPosition;
	private float dragTime;
	
	void Awake()
	{
		// Adjust collider size: minimum 150x150
		if(fixCollider)
		{
			BoxCollider box = collider as BoxCollider;
			Vector3 sz = new Vector3 (150f, 150f, box.size.z);
			sz.x = Mathf.Max (1f, sz.x / transform.localScale.x);
			sz.y = Mathf.Max (1f, sz.y / transform.localScale.y);
			box.size = sz;
		}
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
			if(convertTargetToBox && col.GetComponent<UISprite>() != null)
				col.GetComponent<UISprite> ().spriteName = "gray_box";
		}
		this.GetComponent<UIDragObject> ().enabled = true;
	}
	
	public void DisableCollider()
	{
		collider.enabled = false;
	}

	// Give attention to this.
	void OnPress(bool isDown)
	{
		if(GetComponent<UISprite>() == null) return;
		if(isDown)
			GetComponent<UISprite>().depth++;
		else
			GetComponent<UISprite>().depth--;
	}
	
	void OnDrag()
	{
		dragTime = 0.5f;
	}
	
	void OnTriggerStay(Collider other)
	{		
		if(other.GetComponent<JumbleDrag>() != null) return;
		dragTime -= Time.deltaTime;
		if(dragTime <= 0f)
		{
			dragTime = 0.5f;

			foreach(var col in targetCollider)
			{
				bool firstcond = (!distanceMode && other.collider == col);
				bool secondcond = (distanceMode && 
				     Vector2.Distance(other.transform.localPosition,transform.localPosition) < distanceModeDistance);
				if( firstcond || secondcond )
				{
					StartCoroutine(HappyEnd(col));
					return;
				}
			}

			// Not a correct collider
			if(!distanceMode)
			{
				GameController.Instance.PlayWrongAnswer();
				StartCoroutine(BackToInitialPlace());
			}
		}
	}

	IEnumerator HappyEnd(Collider col)
	{
		GameController.Instance.PlayCorrectAnswer();
		level.DragComplete (this);
		DisableCollider ();
		dragTime = 0.5f;
		this.GetComponent<UIDragObject> ().enabled = false;
		col.enabled = false;
		iTween.MoveTo(gameObject,iTween.Hash("position",col.transform.localPosition
		                                     ,"time",1f,"islocal",true));
		yield return new WaitForSeconds(0.1f);
		if(convertTargetToBox && col.GetComponent<UISprite>() != null)
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
