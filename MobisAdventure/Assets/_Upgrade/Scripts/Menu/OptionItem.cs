using UnityEngine;
using System.Collections;

public class OptionItem : MonoBehaviour {

	public int cost=0;
	public string itemName;
	public OptionMenu optionMenu;
	private UISprite sprite;
	private string spriteName;
	private UILabel labelCost;
	
	void Awake()
	{
		transform.FindChild ("Name").localScale = new Vector3 (1.1f, 1f, 1f);
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
			labelCost = transform.FindChild("Cost").GetComponent<UILabel>();
			labelCost.text = MainMenu.PutCommas(cost);
		}

		labelCost.gameObject.SetActive(Locked);
	}

	void OnClick()
	{
		if(Locked)
			StartCoroutine(GetComfirm());
		else
			optionMenu.Select(this);
	}

	private IEnumerator GetComfirm()
	{
		int coins = PlayerPrefs.GetInt("pp_coins");

		string message,okbm,backbm;
		
		if(coins>=cost)
		{
			message = "WOULD YOU LIKE TO BUY THIS ITEM WITH " + MainMenu.PutCommas(cost) + " COINS?";
			okbm = "BUY";
			backbm = "BACK";
		}
		else
		{
			message = "YOU NEED " + MainMenu.PutCommas(cost-coins) + " MORE COINS TO BUY THIS ITEM. WOULD YOU " +
				"LIKE TO BUY MORE COINS?\n";
			okbm = "BUY";
			backbm = "BACK";
		}

		yield return StartCoroutine(MainMenu.Instance.confirmationPopup.ShowPopupThread(
			message,okbm,backbm));
		bool status = MainMenu.Instance.confirmationPopup.status;
		if(status)
		{
			if(coins>=cost)
			{
				coins -= cost;
				PlayerPrefs.SetInt("pp_coins",coins);
				PlayerPrefs.SetInt(optionMenu.optionName+itemName,1);
				PlayerPrefs.Save();
				optionMenu.Select(this);
			}
			else
			{
				MainMenu.Instance.GetMoreCoinsToggle.OnClick();
			}
		}
	}
	
	public void SetSelected(bool selected)
	{
		Init ();
		sprite.spriteName = spriteName+(selected?"_highlighted":"");
	}

	public bool Locked{ get{ return cost>0&&!PlayerPrefs.HasKey(optionMenu.optionName+itemName); }}
}
