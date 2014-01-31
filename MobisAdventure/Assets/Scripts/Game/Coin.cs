using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
	private int coinValue=0;
	private TextMesh textMesh;
	
	void Awake()
	{
		textMesh = GetComponentInChildren<TextMesh>();
	}
	
	void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "CoinCollider"){
			//send coins to bank
			Coin_Counter.AddCoins(coinValue);
			//disable 
			gameObject.SetActive(false);
		}
	}
	
	public void Reset(int value){
		coinValue=value;
		textMesh.text = ""+coinValue;
		//reenable self
		gameObject.SetActive(false);
		gameObject.SetActive(true);
	}
}
