using UnityEngine;
using System.Collections;

public class ButtonBoost : MonoBehaviour {
	
	private TweenScale tweener;

	void Awake()
	{
		tweener = GetComponentInChildren<TweenScale> ();
	}

	void OnPress(bool isDown){
		PlayerController.Instance.controlBoost  = isDown;
	}

	public void Animate(bool value)
	{
		if(tweener.enabled && value) return;
		tweener.enabled = value;
		if(value) tweener.PlayForward();
		else tweener.transform.localScale = new Vector3(1f,1f,1f);
	}
}
