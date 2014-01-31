using UnityEngine;
using System.Collections;

public class Upgrade_menu : MonoBehaviour {
	
	public Upgrade_item[] upgrades;
	private bool onScreen = false;
//	public GameObject doneUpgrade;
	public float m_slideSpeed = 10.0f;
	
	public static Upgrade_menu m_upgrade = null;
	
	void Awake()
	{
		m_upgrade = this;
	}
	
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
	
	void Update(){
//		MoveOnScreen();
	}
	
	// Simple tap
	private void On_SimpleTap( Gesture gesture){
		
		// Verification that the action on the object
//		if (gesture.pickObject == doneUpgrade){
//			onScreen = false;
//			Ride_menu.m_swipeHoldU = onScreen;
//		} 
		
		
		if(Ride_menu.m_swipeHoldR != true){
			for(int i = 0; i < upgrades.Length; i++){
				if (gesture.pickObject == upgrades[i].button){
					//BUY UPGRADE!?
					int bikeCost =  (m_upgrade.upgrades[i].level + 1)*(25000);
					
					Confirm_menu.PurchaseInit("Upgrade" + upgrades[i].level, bikeCost, new ConfirmDelegate(Upgrade), i);
				} 
			}
		}
	}
	
	public void Upgrade(int i){
		//get level
		int level;
		if(PlayerPrefs.HasKey(GameManager.m_chosenSled.ToString() + "_" + m_upgrade.upgrades[i].name)){
			level = PlayerPrefs.GetInt(GameManager.m_chosenSled.ToString() + "_" + m_upgrade.upgrades[i].name);
		} else { 
			PlayerPrefs.SetInt(GameManager.m_chosenSled.ToString() + "_" + m_upgrade.upgrades[i].name, 0);
			level = 0;
		}
		//add level
		level = level + 1;
		//set level
		upgrades[i].level = level;
		//save level
		PlayerPrefs.SetInt(GameManager.m_chosenSled.ToString() + "_" + m_upgrade.upgrades[i].name, level);
		Debug.Log("UpgradeUpgradeUpgradeUpgradeUpgrade");
		UpdateCost();
		//UpgradeInit();
		
	}
	
	public static void UpgradeInit () {
		Debug.Log("UpgradeInitUpgradeInitUpgradeInitUpgradeInitUpgradeInitUpgradeInit");
		for(int i = 0; i < m_upgrade.upgrades.Length; i++){
			if(!PlayerPrefs.HasKey(GameManager.m_chosenSled.ToString() + "_" + m_upgrade.upgrades[i].name)){
				PlayerPrefs.SetInt(GameManager.m_chosenSled.ToString() + "_" + m_upgrade.upgrades[i].name,0);
			}
			Debug.Log("Sled" + GameManager.m_chosenSled.ToString());
			m_upgrade.upgrades[i].level = PlayerPrefs.GetInt(GameManager.m_chosenSled.ToString() + "_" + m_upgrade.upgrades[i].name);
			int bikeCost = (m_upgrade.upgrades[i].level + 1)*(25000);
			
			Debug.Log("Up" + i+ " Level" + m_upgrade.upgrades[i].level);
			m_upgrade.upgrades[i].cost = bikeCost;
			m_upgrade.upgrades[i].costText.text = bikeCost.ToString();
			for(int j = 0; j < m_upgrade.upgrades[i].fillbar.Length; j++){
					
				if(m_upgrade.upgrades[i].level == j){
						m_upgrade.upgrades[i].fillbar[j].SetActive(true);				
					} else {
						m_upgrade.upgrades[i].fillbar[j].SetActive(false);	
					}
			}
		}
//		m_upgrade.UpdateCost();
		
//		m_upgrade.onScreen = true;
//		Ride_menu.m_swipeHoldU = m_upgrade.onScreen;
	}
	
	public void UpdateCost(){
		//update all the text meshes with the update objects current cost
		for(int i = 0; i < upgrades.Length; i++){
			upgrades[i].costText.text = upgrades[i].cost.ToString();
			
			for(int j = 0; j < upgrades[i].fillbar.Length; j++){
					
				if(upgrades[i].level == j){
						upgrades[i].fillbar[j].SetActive(true);				
					} else {
						upgrades[i].fillbar[j].SetActive(false);	
					}
			}
		}
	}
	
	public void ShowFillbar(int show, GameObject[] fillbar){
		if(fillbar != null){
			for (int i = 0; i < fillbar.Length; i++){
				if(i == show){
					fillbar[i].SetActive(true);	
				} else {
					fillbar[i].SetActive(false);
				}
			}
		} else {
			Debug.Log("Empyfillbars");
		}
	}
	
	public void MoveOnScreen(){
		float step = m_slideSpeed * Time.deltaTime;
		if(onScreen){
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x,-100,transform.localPosition.z), step);		
		} else {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x,100,transform.localPosition.z), step);
		}
	}
	
	public static Upgrade_menu Instance
    {
        get { return m_upgrade; }
    }
}

[System.Serializable]
public class Upgrade_item {
	public string name;
	public int level;
	public int cost;
	public TextMesh costText;
	public GameObject button;
	public GameObject[] fillbar; 
}