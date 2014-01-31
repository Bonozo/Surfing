using UnityEngine;
using System.Collections;

public class Clothes_menu : MonoBehaviour {
	
	public Upgrade_clothes[] clothes;
	public GameObject[] mobis;
	private bool onScreen = false;
//	public GameObject clothingButton;
//	public GameObject doneUp;
	public float m_slideSpeed = 10.0f;
	
	public static Clothes_menu m_clothes = null;
	
	void Awake()
	{
		m_clothes = this;
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
	
	void Start(){
		CheckClothes();	
	}
	
	void Update(){
//		MoveOnScreen();
	}
	
	// Simple tap
	private void On_SimpleTap( Gesture gesture){
		
		
//		if (gesture.pickObject == clothingButton){
//			onScreen = true;
//		} 
//		
//		// Verification that the action on the object
//		if (gesture.pickObject == doneUp){
//			onScreen = false;
//		} 
		if(Ride_menu.m_swipeHoldR != true){
			for(int i = 0; i < clothes.Length; i++){
				if (gesture.pickObject == clothes[i].button){
					//if not locked set clothing
					if(0 < PlayerPrefs.GetInt(clothes[i].name.ToString()) || clothes[0].button == gesture.pickObject){
						SetMobis(i);
					} else {//if locked
						int clothingCost = clothes[i].cost;
						Confirm_menu.PurchaseInit(clothes[i].name, clothingCost, new ConfirmDelegate(BuyClothes), i);
					}
				} 
			}	
		}
	}
	
	public void BuyClothes(int i){
		PlayerPrefs.SetInt(clothes[i].name.ToString(), 1);
		Debug.Log("HEREHIOJHASDF");
		//remove the lock
		clothes[i].locked.SetActive(false);
		SetMobis(i);	
	}
	
	public void UpdateCost(){
		//update all the text meshes with the update objects current cost
		for(int i = 0; i < clothes.Length; i++){
			if(clothes[i].costText)
			clothes[i].costText.text = clothes[i].cost.ToString();
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
	
	public static void ClothesInit () {
		for(int i = 0; i < m_clothes.clothes.Length; i++){
			if(!PlayerPrefs.HasKey(m_clothes.clothes[i].ToString())){
				PlayerPrefs.SetInt(m_clothes.clothes[i].ToString(), 0);
			}
			m_clothes.clothes[i].costText.text = m_clothes.clothes[i].cost.ToString();
		}
		m_clothes.onScreen = true;
	}
	
	public void CheckClothes(){
		for(int i = 0; i < clothes.Length; i++){
			if(PlayerPrefs.HasKey(clothes[i].name.ToString())){
				//if it's 1 remove the lock
				if(0 < PlayerPrefs.GetInt(clothes[i].name.ToString())){
					clothes[i].locked.SetActive(false);
				}
			} else {
				PlayerPrefs.SetInt(clothes[i].ToString(), 0);
				clothes[i].locked.SetActive(true);
			}
			//set text mesh
			if(clothes[i].costText)
				clothes[i].costText.text = clothes[i].cost.ToString();
		}
		clothes[0].locked.SetActive(false);
	}
	
	public void SetMobis(int clothNum){
		for(int j = 0; j < mobis.Length; j++){
			mobis[j].renderer.material = clothes[clothNum].mat;
		}
		GameManager.m_chosenMobi = (GameManager.ChosenMobi)(clothNum);
	}
	
	public static Clothes_menu Instance
    {
        get { return m_clothes; }
    }
}

[System.Serializable]
public class Upgrade_clothes {
	public string name;
	public bool available;
	public int cost;
	public TextMesh costText;
	public GameObject button;
	public GameObject locked;
	public Material mat;
}
