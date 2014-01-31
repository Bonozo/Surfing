using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class Money_purchase : MonoBehaviour {
#if UNITY_ANDROID
	public string skuID;
	public int coins;
	public bool responsible;
	public GameObject loadingText;
	public GameObject loadingImage;
//	private List<StoreKitProduct> _products;
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
		//StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessful;
	}
	
	void OnDisable(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
		//StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
	}
	
	// Use this for initialization
	void Start()
	{
		// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
		// we will store the products locally so that we will know what is purchaseable and when we can purchase the product
		//if(responsible) {
		//	GetProductData();
		//}
	}
	
	private void On_SimpleTap( Gesture gesture){
		if (gesture.pickObject == this.gameObject){
			//Debug.Log(this.gameObject.name);
			//GoogleIAB.consumeProduct( skuID );
			//StoreKitBinding.purchaseProduct( skuID, 1 );
			loadingText.SetActive( true );
			loadingImage.SetActive( true );
			GameObject.Find("MyIABController").SendMessage("On" + this.gameObject.name,SendMessageOptions.DontRequireReceiver);
			
		}
	}
	
	/*void GetProductData()
	{
		// array of product ID's from iTunesConnect.  MUST match exactly what you have there!
		var productIdentifiers = new string[] { "150k_coins", "250k_coins", "500k_coins", "750k_coins", "2m_coins", "99c_noads" };
		StoreKitBinding.requestProductData( productIdentifiers );
		Debug.Log( "Get Product Data");
	}
	
	void purchaseSuccessful( StoreKitTransaction transaction )
	{
		Debug.Log( "purchased product: " + transaction.productIdentifier );
		if(transaction.productIdentifier == skuID){
			if(responsible){
				//disable ads
				Coin_Counter.AddSaveCoins(coins);
			} else {
				Coin_Counter.AddSaveCoins(coins);
			}
		}
	}*/
	
#endif
}
