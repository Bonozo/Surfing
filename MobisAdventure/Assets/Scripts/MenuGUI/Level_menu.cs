using UnityEngine;
using System.Collections;

public class Level_menu : MonoBehaviour {

	//all the sleds
	public Maps[] c_maps;
	public GameObject c_mapGroup;
	public float m_slideSpeed = 10.0f;
	private int m_slideCount = 0;
	public static bool m_swipeHoldL = true;
	
	public static Level_menu m_level = null;
	
	void Awake()
	{
		m_level = this;
	}
	
	// Subscribe to events
	void OnEnable(){
//		EasyTouch.On_SimpleTap += On_SimpleTap;
//		EasyTouch.On_SwipeStart += On_SwipeStart;
//		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;	
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
		
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
//		EasyTouch.On_SimpleTap -= On_SimpleTap;
//		EasyTouch.On_SwipeStart -= On_SwipeStart;
//		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;	
	}

	void Start () {
		StartMapPos();
	}
	
	void Update () {
		SlideMaps();
	}
	
	private void On_SimpleTap( Gesture gesture ){
		
		if (gesture.pickObject == c_maps[m_slideCount].locked){
			ConfirmUnlock(m_slideCount);
			Debug.Log ("MapLockClicked" + m_slideCount);
		}
	}

	
	// During the swipe
	private void On_SwipeEnd(Gesture gesture){
		//if ok to bike change
		if(!m_swipeHoldL){
			//slide bikes the direction of the swipe
			if(gesture.swipe == EasyTouch.SwipeType.Left && m_slideCount < c_maps.Length-1){
				m_slideCount++;	
			}else if(gesture.swipe == EasyTouch.SwipeType.Right && m_slideCount > 0){
				m_slideCount--;	
			}
			//set chosen sled for game
			GameManager.m_chosenLevel = (GameManager.ChosenLevel)(m_slideCount % c_maps.Length);
		}
	}
	
	void StartMapPos(){
		//start placing the maps in local space	
		for(int i = 0; i < c_maps.Length; i++){
			//c_maps[i].geo.transform.localPosition = new Vector3(i,0,0);
			if(PlayerPrefs.GetInt("Map_" + c_maps[i].name) > 0 || c_maps[i].available == true){
				c_maps[i].locked.SetActive(false);
				c_maps[i].costText.text = "";
			} else {
				c_maps[i].locked.SetActive(true);
				c_maps[i].costText.text = c_maps[i].cost.ToString();
			}
		}
	}
	
	void SlideMaps(){
		//move the c_bikeGroup 1 unit at m_slideSpeed
		float step = m_slideSpeed * Time.deltaTime;
		c_mapGroup.transform.localPosition = Vector3.MoveTowards(c_mapGroup.transform.localPosition, new Vector3(-m_slideCount,0,0), step);

	}
	

	
	public bool ConfirmUnlock(int i){
		if(1 > PlayerPrefs.GetInt("Map_" + c_maps[i].name.ToString(),0) && c_maps[i].available == false){
			Confirm_menu.PurchaseInit(c_maps[i].name, c_maps[i].cost, new ConfirmDelegate(AddMap), i);
			return false;
		} else 
			return true;
	}
	
	void AddMap(int i){
		PlayerPrefs.SetInt("Map_" + c_maps[i].name, 1);
		c_maps[i].locked.SetActive(false);
		c_maps[i].costText.text = "";
	}
	
	public static bool ClearForGame(){
		//store chosen bike
		if(m_level.ConfirmUnlock(m_level.m_slideCount)){
			GameManager.m_chosenLevel = (GameManager.ChosenLevel)(m_level.m_slideCount % m_level.c_maps.Length);
			return true;
		} else { //bike not unlocked
			return false;
		}
	}
			
	public static Level_menu Instance
    {
        get { return m_level; }
    }
}

[System.Serializable]
public class Maps {
	public string name;
	public GameObject geo;
	public bool available;
	public int cost;
	public TextMesh costText;
	public GameObject locked;
}


