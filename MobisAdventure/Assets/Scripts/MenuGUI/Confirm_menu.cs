using UnityEngine;
using System.Collections;

public delegate void ConfirmDelegate(int i);

public class Confirm_menu : MonoBehaviour {
	
	static ConfirmDelegate confDelegate = null;
	static int confDelInt;
	
	public GameObject u_yes;
	public GameObject u_no;
	public int u_cost;
	public string u_name;
	public TextMesh u_costText;
	public TextMesh u_nameText;
	protected static int u_coins = 0;
	private bool onScreen = false;
	public float m_slideSpeed = 10.0f;
	
	
	

	
	public static Confirm_menu m_confirm = null;
	
	void Awake()
	{
		m_confirm = this;
		
		//set u_coins
		u_coins = Coins();
	}

	void Start () {
		u_coins = Coins();
	}
	
	void Update(){
		MoveOnScreen();
		u_coins = Coins();
	}
	
	public static int Coins(){
		//check for coins in Player.Pref, if yes
		/*if(!PlayerPrefs.HasKey("pp_coins")){
			//maybe should check an online bank?
			PlayerPrefs.SetInt("pp_coins", 10);
			return 10;
		} else
			return PlayerPrefs.GetInt("pp_coins");*/
		return PlayerPrefs.GetInt("pp_coins",10);
	}
	
	public static void PurchaseInit (string f_name, int f_cost, ConfirmDelegate f_method, int f_methInt) {
		confDelegate = f_method;
		confDelInt = f_methInt;
		//do we even have enough money?
		int tttt = PlayerPrefs.GetInt("CurrentBikeLock",0);
		Debug.Log("TTTTTTT" + tttt);
		
		if(f_cost > u_coins /*|| tttt == 1*/){
			//turn cost red
			m_confirm.u_costText.gameObject.renderer.material.color = Color.red;
			//hide ok button
			m_confirm.u_yes.SetActive(false);
		} else { //they have the funds
			//turn cost white
			m_confirm.u_costText.gameObject.renderer.material.color = Color.white;
			//show ok button
			m_confirm.u_yes.SetActive(true);	
		}
		m_confirm.u_cost = f_cost;
		m_confirm.u_name = f_name;
		m_confirm.u_costText.text = m_confirm.u_cost.ToString();
		m_confirm.u_nameText.text = m_confirm.u_name.ToString();
		//drop menu onto screen
		m_confirm.onScreen = true;
		Ride_menu.m_swipeHoldC = m_confirm.onScreen;
		Level_menu.m_swipeHoldL = m_confirm.onScreen;
	}

	public void MoveOnScreen(){
		float step = m_slideSpeed * Time.deltaTime;
		if(onScreen){
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x,-100,transform.localPosition.z), step);
		} else {
			transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x,100,transform.localPosition.z), step);
		}
	}
	
	public static Confirm_menu Instance
    {
        get { return m_confirm; }
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
	

	
	// Simple tap
	private void On_SimpleTap( Gesture gesture){
		
		// Yes
		if (gesture.pickObject == u_yes){
			//purchase coins
			if(Coin_Counter.Spend(u_cost)){
				//do the shit
				confDelegate(confDelInt);
				confDelegate = null;
				confDelInt = 0;
				Debug.Log("fuuuck");
				//put away menu
				onScreen = false;
				Ride_menu.m_swipeHoldC = onScreen;
				Level_menu.m_swipeHoldL = onScreen;
				//Debug.Log("DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
				//GameObject.Find("_UpgradeBack").SendMessage("UpgradeInit",SendMessageOptions.DontRequireReceiver);
			}
		} else if (gesture.pickObject == u_no){
			//cancel
			onScreen = false;
			Ride_menu.m_swipeHoldC = onScreen;
			Level_menu.m_swipeHoldL = onScreen;
		}
	}
		
	public bool SpendMoney(){
		//deduct u_cost from pp_coins
		int balance = PlayerPrefs.GetInt("pp_coins");
		balance = balance - u_cost;
		PlayerPrefs.SetInt("pp_coins", balance);
		return true;
	}
	
}

