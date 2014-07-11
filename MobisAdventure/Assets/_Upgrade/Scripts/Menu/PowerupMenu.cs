using UnityEngine;
using System.Collections;

public class PowerupMenu : MonoBehaviour {

	public OptionMenu sleds;
	public GameObject[] sledSprite;
	public Transform sledSpriteTransform;

	public string upgradeName = "";
	public UILabel labelTitle;
	public UILabel labelLevel;
	public UISprite spriteIcon;
	public UILabel labelInfo;
	public GameObject buttonUpgrade;

	void OnEnable(){
		
		Destroy(sledSpriteTransform.GetChild(0).gameObject);
		var sd = Instantiate(sledSprite[sleds.SelectedIndex]) as GameObject;
		sd.transform.parent = sledSpriteTransform;
		sd.transform.localPosition = Vector3.zero;
		sd.transform.localScale = new Vector3(1,1,1);

		RefreshWindow ();
	}

	public void RefreshWindow(){
		labelTitle.text = upgradeName.ToUpper ();
		spriteIcon.spriteName = upgradeName.ToLower () + "_button";
		int currentLevel = PlayerPrefs.GetInt (CurrentSledName + "_" + upgradeName, 0) + 1;
		labelLevel.text = "" + currentLevel + "/8";
		
		int coins = PlayerPrefs.GetInt("pp_coins");
		int newlevelcost = currentLevel*25000;
		buttonUpgrade.SetActive (false);
		if(currentLevel == 8)
			labelInfo.text = "Fully Upgraded!";
		else if(newlevelcost > coins)
			labelInfo.text = "You need " + MainMenu.PutCommas(newlevelcost-coins) + " more Coins!";
		else{
			labelInfo.text = "";
			buttonUpgrade.SetActive(true);
		}
	}

	public string CurrentSledName{
		get{
			return sleds.SelectedName;
		}
	}
}
