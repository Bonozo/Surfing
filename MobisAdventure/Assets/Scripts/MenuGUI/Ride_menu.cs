using UnityEngine;
using System.Collections;

public class Ride_menu : MonoBehaviour {
	
	//all the sleds
	public GameObject[] c_bikes;
	public GameObject c_bikeGroup;
	public float m_slideSpeed = 10.0f;
	private int m_slideCount = 0;
	public static bool m_swipeHoldR = false;
	public static bool m_swipeHoldU = false;
	public static bool m_swipeHoldC = false;
	
	public static Ride_menu m_ride = null;
	
	void Awake()
	{
		m_ride = this;
	}
	
	// Subscribe to events
	void OnEnable(){
//		EasyTouch.On_SwipeStart += On_SwipeStart;
//		EasyTouch.On_Swipe += On_Swipe;
		EasyTouch.On_SwipeEnd += On_SwipeEnd;		
	}

	void OnDisable(){
		UnsubscribeEvent();
		
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
//		EasyTouch.On_SwipeStart -= On_SwipeStart;
//		EasyTouch.On_Swipe -= On_Swipe;
		EasyTouch.On_SwipeEnd -= On_SwipeEnd;	
	}

	void Start () {
		StartBikePos();
		Upgrade_menu.UpgradeInit();
	}
	
	void Update () {
		SlideBikes();
		//Debug.Log("SetCurrentBikeCost");
		PlayerPrefs.SetInt("CurrentBikeLock",m_ride.c_bikes[m_ride.m_slideCount].GetComponent<Bike_menu>().GetUpStatus());
		Debug.Log("Mride_slideCount"+ m_ride.m_slideCount);
		Debug.Log ("SetCurrentBikeCost" + m_ride.c_bikes[m_ride.m_slideCount].GetComponent<Bike_menu>().GetUpStatus());
	}
	
	// During the swipe
	private void On_SwipeEnd(Gesture gesture){
		//if ok to bike change
		if(!m_swipeHoldR){ //!m_swipeHoldU && !m_swipeHoldC && 
			//slide bikes the direction of the swipe
			if(gesture.swipe == EasyTouch.SwipeType.Left && m_slideCount < c_bikes.Length){
				m_slideCount++;
			}else if(gesture.swipe == EasyTouch.SwipeType.Right && m_slideCount > 0){
				m_slideCount--;
			}
			//set chosen sled for game
			GameManager.m_chosenSled = (GameManager.ChosenSled)(m_slideCount % c_bikes.Length);
			Upgrade_menu.UpgradeInit();
		}
	}
	
	void StartBikePos(){
		//start placing the books in local space	
		for(int i = 0; i < c_bikes.Length; i++){
			c_bikes[i].transform.localPosition = new Vector3(i,0,0);
		}
	}
	
	void SlideBikes(){
		//move the c_bikeGroup 1 unit at m_slideSpeed
		float step = m_slideSpeed * Time.deltaTime;
		c_bikeGroup.transform.localPosition = Vector3.MoveTowards(c_bikeGroup.transform.localPosition, new Vector3(-m_slideCount,0,0), step);

	}
	
	public static bool ClearForLevel(){
		//store chosen bike
		if(m_ride.c_bikes[m_ride.m_slideCount].GetComponent<Bike_menu>().GetUpStatus() > 0){
			GameManager.m_chosenSled = (GameManager.ChosenSled)(m_ride.m_slideCount % m_ride.c_bikes.Length);
			return true;
		} else if (m_ride.m_slideCount == 0 ){
			return true;
		}
		else
		{ //bike not unlocked
			return false;
		}
	}
	

	public static Ride_menu Instance
    {
        get { return m_ride; }
    }

	
	void ConfirmUnlock(){
		
	}
	
}
