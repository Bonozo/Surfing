using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public class Coin_Counter : MonoBehaviour {
	
	public int coin_balance;
	public TextMesh coinText;
	public int start_balance;
	
	public AudioClip a_coinPing;
	
	public static Coin_Counter m_ccounter = null;
	
	void Awake()
	{
		m_ccounter = this;
		GetStartUpBalance();
	}
	
	// Use this for initialization
	void Start () {
//		GetStartUpBalance();
	}
	
	void UpdateText () {
		coinText.text = coin_balance.ToString();
		//do save here too?
	}
	
	public static bool Spend(int spend){
		int balance = PlayerPrefs.GetInt("pp_coins");
		balance = balance - spend;
		m_ccounter.SetAndSaveBalance(balance);
		return true;
	}
	
	public void SetBalance(int balance){
		coin_balance = balance;
		UpdateText();
	}
	
	public void SetAndSaveBalance(int balance){
		PlayerPrefs.SetInt("pp_coins", balance);
		SetBalance(balance);
	}
	
	public static void GetUpBalance(){
		if(!PlayerPrefs.HasKey("pp_coins")){
			//give player 50 coins
			m_ccounter.SetAndSaveBalance(0);
			Debug.Log("no key");
		}
		if(PlayerPrefs.GetInt("pp_coins") < 50){
			//give player 50 coins
			m_ccounter.SetAndSaveBalance(0);
			Debug.Log("shot doe");
		}
		
//		int dummyValue = 100000;
//		PlayerPrefs.SetInt("pp_coins", dummyValue);
		m_ccounter.SetBalance(PlayerPrefs.GetInt("pp_coins"));
	}
	public void GetStartUpBalance(){
		GetUpBalance();
		start_balance = coin_balance;
	}
	
	public static void AddCoins(int addCoins){
		m_ccounter.coin_balance = m_ccounter.coin_balance + addCoins;
		m_ccounter.SetBalance(m_ccounter.coin_balance);
		if(m_ccounter.a_coinPing){
			m_ccounter.audio.PlayOneShot(m_ccounter.a_coinPing);
		}
		m_ccounter.audio.Play();
	}
	
	public static void AddSaveCoins(int addCoins){
		Debug.Log ("coins added: " + addCoins );
		m_ccounter.coin_balance = m_ccounter.coin_balance + addCoins;
		m_ccounter.SetAndSaveBalance(m_ccounter.coin_balance);
	}
	
	public static int GetBalance(){
		return m_ccounter.coin_balance;
	}
	
	public static Coin_Counter Instance
    {
        get { return m_ccounter; }
    }
}
