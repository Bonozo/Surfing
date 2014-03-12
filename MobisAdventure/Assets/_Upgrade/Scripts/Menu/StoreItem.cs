using UnityEngine;
using System.Collections;

public class StoreItem : MonoBehaviour {
	
	public StoreMenu optionMenu;
	private UISprite sprite;
	private string spriteName;
	
	void Awake()
	{
		Init ();
	}
	
	void Update()
	{
		collider.enabled = !MainMenu.Instance.isPopupActive;
	}
	
	bool inited=false;
	void Init()
	{
		if(!inited)
		{
			inited=true;
			sprite = transform.GetComponentInChildren<UISprite>();
			spriteName = sprite.spriteName;
		}
	}
	
	void OnClick()
	{
		optionMenu.Select(this);
	}

	public void SetSelected(bool selected)
	{
		Init ();
		sprite.spriteName = spriteName+(selected?"_highlighted":"");
	}
}
