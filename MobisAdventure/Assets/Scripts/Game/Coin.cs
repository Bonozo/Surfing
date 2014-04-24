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

	float initialY;

	void OnEnable()
	{
		initialY = transform.localPosition.y;
		collider.enabled = true;
	}

	float a=0.5f,b=4f;
	void Update()
	{
		var pos = transform.localPosition;
		pos.y = initialY + 0.5f + a*Mathf.Sin (b*Time.time);
		transform.localPosition = pos;
		if(transform.position.x < PlayerController.Instance.transform.position.x)
			collider.enabled = false;
	}
}
