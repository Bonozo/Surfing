using UnityEngine;
using System.Collections;

public class Bike_menu : MonoBehaviour {
	
	public string bikeName;
	public int bikeCost;
	public GameObject unlockUpgrade_button;
	public Material upgradeMat;
	private int upStatus; 
	
	// Subscribe to events
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}
	
	// Simple tap
	private void On_SimpleTap( Gesture gesture){
		
		// Verification that the action on the object
		if (gesture.pickObject == unlockUpgrade_button){
			UnlockUpgrade();
		}
	}
	
	void Start () {
		//TEMP
//		PlayerPrefs.SetInt(bikeName, 0);
		
		if(bikeName != "" && PlayerPrefs.HasKey(bikeName)){
			//get keys current value.
			upStatus = PlayerPrefs.GetInt(bikeName);
			UpdateMat ();
		}
	}
	
	void UpdateMat () {
		//if the key value is greater than zero it has been unlocked and can be upgr
		if(upStatus > 0){
			//display upgrade button
			unlockUpgrade_button.SetActive (false);
		}
	}
	
	//unlock purchased bikes
	void Unlock(int upGrade){
		PlayerPrefs.SetInt(bikeName, upGrade);
		upStatus = PlayerPrefs.GetInt(bikeName);
		UpdateMat();
	}
	
	delegate void MyDelegate(int i);
	
	//if tap on UnlockUpgrade
	void UnlockUpgrade(){
		//Unlock or Upgrade
		if(upStatus > 0){
			//goto Upgrades Menu
//			Upgrade_menu.UpgradeInit();
		} else {
			//purchase the bike
			Confirm_menu.PurchaseInit(bikeName, bikeCost, new ConfirmDelegate(Unlock), 1);
//			Upgrade_menu.UpgradeInit();
			 
		}	
	}
	
	public int GetUpStatus(){
		Debug.Log("BikeName"+ bikeName +"BikeCost" +bikeCost);
		if (bikeName == "Classic")
			return 1;
		else
			return upStatus;
	}
	//do this on the upgrade page? check select bike, get upgrade status, display upgrade icons?
	//if unlock button pressed prompt unlock purchase
	//if upgrade button pressed 
		//set the upgrade item to reflect the bikes upgrade status bring in the  upgrade items
}
