using UnityEngine;
using System.Collections;

public class SledUpgrade : MonoBehaviour {

	public string upgradeName;
	public UpgradeMenu menu;
	public UILabel labelName;
	public UILabel labelCost;
	public UISprite spriteFillbar;

	void Update()
	{
		collider.enabled = !MainMenu.Instance.confirmationPopup.gameObject.activeSelf;
	}

	void OnClick()
	{
		StartCoroutine(UpgradeThread());
	}

	IEnumerator UpgradeThread()
	{
		int currentLevel = PlayerPrefs.GetInt(menu.CurrentSledName+"_"+upgradeName,0);
		int newlevel = currentLevel+1;
		int coins = PlayerPrefs.GetInt("pp_coins");
		int newlevelcost = newlevel*25000;

		string message,okbm,backbm;

		if(coins>=newlevelcost)
		{
			message = "UPGRADE TO LEVEL " + (newlevel+1) + " WITH " + MainMenu.PutCommas(newlevelcost) + " COINS?";
			okbm = "OK";
			backbm = "BACK";
		}
		else
		{
			message = "YOU NEED " + MainMenu.PutCommas(newlevelcost-coins) + " MORE COINS TO BUY THIS ITEM. WOULD YOU " +
				"LIKE TO BUY MORE COINS?";
			okbm = "BUY";
			backbm = "BACK";
		}
		yield return StartCoroutine(MainMenu.Instance.confirmationPopup.ShowPopupThread(
			message,okbm,backbm));
		bool status = MainMenu.Instance.confirmationPopup.status;
		if(status)
		{
			if(coins>=newlevelcost)
			{
				coins -= newlevelcost;
				PlayerPrefs.SetInt("pp_coins",coins);
				PlayerPrefs.SetInt(menu.CurrentSledName+"_"+upgradeName,newlevel);
				PlayerPrefs.Save();
				Init();
			}
			else
			{
				MainMenu.Instance.GetMoreCoinsToggle.OnClick();
			}
		}
	}

	public void Init()
	{
		int currentLevel = PlayerPrefs.GetInt(menu.CurrentSledName+"_"+upgradeName,0);
		labelCost.text = MainMenu.PutCommas( (1+currentLevel)*25000);
		labelName.text = upgradeName.ToUpper() + " (LEVEL " + (currentLevel+1) + " )";

		int fillbarindex = Mathf.Min(8,currentLevel);
		spriteFillbar.spriteName = "fillbar_"+fillbarindex;
	}
}
