using UnityEngine;
using System.Collections;

public class PowerupMenu : MonoBehaviour {

	public OptionMenu sleds;
	public OptionMenu levels;
	public GameObject[] sledSprite;
	public Transform sledSpriteTransform;

	public GameObject screenNormal;
	public GameObject screenLocked;

	public string upgradeName = "";
	public UILabel labelTitle;
	public UILabel labelLevel;
	public UISprite spriteIcon;
	public UILabel labelInfo;
	public UILabel labelPowerupInfo;
	public GameObject buttonUpgrade;

	int neededCoins = 0;

	void OnEnable(){
		
		Destroy(sledSpriteTransform.GetChild(0).gameObject);
		var sd = Instantiate(sledSprite[sleds.SelectedIndex]) as GameObject;
		sd.transform.parent = sledSpriteTransform;
		sd.transform.localPosition = Vector3.zero;
		sd.transform.localScale = new Vector3(1,1,1);

		int lvl = levels.SelectedItem.GetComponent<LevelNameText> ().currentLevel;
		bool lockscreen = (upgradeName == "Suspension" && lvl < 3);
		screenNormal.SetActive (!lockscreen);
		screenLocked.SetActive (lockscreen);
		RefreshWindow ();
	}
	
	public void RefreshWindow(){
		spriteIcon.spriteName = upgradeName.ToLower () + "_button";
		int currentLevel = PlayerPrefs.GetInt (CurrentSledName + "_" + upgradeName, 0) + 1;
		labelLevel.text = "" + currentLevel + "/8";
		
		int coins = PlayerPrefs.GetInt("pp_coins");
		int newlevelcost = currentLevel*25000;
	
		neededCoins = newlevelcost - coins;
		if(neededCoins<=0) neededCoins=0;

		//buttonUpgrade.SetActive (false);
		if(currentLevel == 8)
			labelInfo.text = "Fully Upgraded!";
		else if(newlevelcost > coins)
			labelInfo.text = "Need " + MainMenu.PutCommas(neededCoins) + " more Coins!";
		else{
			labelInfo.text = "Upgrade to next level!";
			//buttonUpgrade.SetActive(true);
		}

		// powerup name and info
		switch (upgradeName) {
		case "Engine":
			labelPowerupInfo.text = "Adds more power to your engine, which increases your ability to do tricks and earn bonuses.";
			labelTitle.text = "ENGINE POWERUP";
			break;
		case "Tread":
			labelPowerupInfo.text = "Improves traction, which increases your ability to obtain greater distances.";
			labelTitle.text = "TREAD POWERUP";
			break;
		case "Suspension":
			labelPowerupInfo.text = "Improves shock absorption, which increases sled stability on bumps.";
			labelTitle.text = "SHOCKS POWERUP";
			break;
		}

		// upgrade button
		var buttonLabel = buttonUpgrade.transform.FindChild("Label").GetComponent<UILabel>();
		if(neededCoins>0){
			if(neededCoins>25000)
				buttonLabel.text = "$1.99";
			else
				buttonLabel.text = "$0.99";
		}
		else
			buttonLabel.text = "UPGRADE";
	}

	public string CurrentSledName{
		get{
			return sleds.SelectedName;
		}
	}

	public void OnDownButtonClick(){
		if(neededCoins>0)
			StartCoroutine(BuySpecialOffer());
		else
			StartCoroutine(UpgradeThread());
	}

	IEnumerator UpgradeThread()
	{
		int currentLevel = PlayerPrefs.GetInt(CurrentSledName+"_"+upgradeName,0);
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
				PlayerPrefs.SetInt(CurrentSledName+"_"+upgradeName,newlevel);
				PlayerPrefs.Save();
				RefreshWindow();
			}
			else
			{
				MainMenu.Instance.GetMoreCoinsToggle.OnClick();
			}
		}
	}
	
	IEnumerator BuySpecialOffer(){
		string purchaseName = neededCoins>25000?"mobisrun_offer199":"mobisrun_offer099";

		if(MobiIAB.debug){
			MainMenu.Instance.messagebox.ShowLoading(false);
			int coins = PlayerPrefs.GetInt("pp_coins");
			coins += neededCoins;
			PlayerPrefs.SetInt("pp_coins",coins);
			PlayerPrefs.Save();
			RefreshWindow();
		}
		else{
			yield return StartCoroutine (MainMenu.Instance.messagebox.ConnectToIAP ());
			if(MobiIAB.Instance.Connected){
				MainMenu.Instance.messagebox.ShowLoading (true);
				
				IAP.purchaseConsumableProduct( purchaseName, ( didSucceed, error ) => {
					if( !didSucceed ){
						MainMenu.Instance.messagebox.ShowLoading(false);
						MainMenu.Instance.messagebox.Show("Failed to purchase!");
					} else{
						MainMenu.Instance.messagebox.ShowLoading(false);
						int coins = PlayerPrefs.GetInt("pp_coins");
						coins += neededCoins;
						PlayerPrefs.SetInt("pp_coins",coins);
						PlayerPrefs.Save();
						RefreshWindow();
					}
				});
			}
		}
	}
}
