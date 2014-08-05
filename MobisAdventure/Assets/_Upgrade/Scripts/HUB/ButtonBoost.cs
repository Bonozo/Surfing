using UnityEngine;
using System.Collections;

public class ButtonBoost : MonoBehaviour {
	
	private TweenScale tweener;
	private UISprite sprite;
	private Boost boost;

	void Awake()
	{
		tweener = GetComponentInChildren<TweenScale> ();
		sprite = tweener.GetComponent<UISprite> ();
		boost = GameObject.FindObjectOfType<Boost> ();

		// Set alpha to 0
		sprite.color = new Color (1f, 1f, 1f, 0f);
	}

	void OnPress(bool isDown){
		PlayerController.Instance.controlBoost  = isDown;
	}

	void Update(){
		bool canBoost = boost.currentBoosts > 0 || PlayerController.Instance.BoostingTime > 0f;
		collider.enabled = canBoost;
		Color col = sprite.color;
		if( canBoost && sprite.alpha < 1f) col.a = Mathf.Clamp01(col.a+5f*Time.deltaTime);
		if( !canBoost && sprite.alpha > 0f) col.a = Mathf.Clamp01(col.a-5f*Time.deltaTime);
		sprite.color = col;
		Animate (canBoost);
	}

	public void Animate(bool value)
	{
		if(tweener.enabled && value) return;
		tweener.enabled = value;
		if(value) tweener.PlayForward();
		else tweener.transform.localScale = new Vector3(1f,1f,1f);
	}
}
