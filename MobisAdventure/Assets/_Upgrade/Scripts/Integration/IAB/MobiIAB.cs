﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class MobiIAB : MonoBehaviour
{
	#if UNITY_ANDROID || UNITY_IPHONE
	private string key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAymWgRnh1BakrbqiGqrF6LBNLMQY/4gHI0KlOc9u6A3kYtMN3ghqQzeIje+WzP7G6wUPeTmLUrI0WUrcQFuvsMm0xwnj9rEY66RhgvwpwC6MO/dOH0NmsMBTll3g8WgT4GHyeBojH21ZZAi5bXS49qAHh/A4wQxVudzazLoMXe9s8/4mFwJUHkByOnMDN9cnVV5wl4nf/zO4i2mZU4A2j9CLBwzVDUK6e6giLPLwpGmTtqEdfX4hfLJ7okFV8rPGXW8aZjwsSclRb4oTuRshpqqONRHvBFcN7GJUT2h43HqDYY1Ul+uTtYQIsdQFDdnXKi2G44KZnNcZrnDhtRUaWLwIDAQAB";
	private string[] skus = new string[] {@"mobisrun_99c_noads",@"mobisrun_150k_coins", @"mobisrun_250k_coins",
		@"mobisrun_500k_coins", @"mobisrun_750k_coins",@"mobisrun_2m_coins" ,@"mobisrun_5m_coins" ,@"mobisrun_10m_coins",
		@"mobisrun_offer099",@"mobisrun_offer199"  };
	private bool queryInventorySucceeded = false;

	void Start(){
		IAP.init (key);
		RequestProductData ();
	}

	private void RequestProductData(){
		IAP.requestProductData(skus,skus,productList => {
			Debug.Log( "***Important***:::::Product list received" );
			Utils.logObject( productList );

			if(productList.Count == 0){
				Debug.Log("Product Count is 0: Why this happens when no connection");
				return;
			}
			queryInventorySucceeded = true;
			for(int i=0;i<productList.Count;i++)
				Debug.Log("Product: " + productList[i].title + " ::: " + productList[i].price);
		});
	}

	public void RestoreTransactions(){
		#if UNITY_IPHONE
		IAP.restoreCompletedTransactions( productId => {
			Debug.Log( "restored purchased product: " + productId );
			if(productId == skus[0] && PlayerPrefs.GetInt("RevmobStatus",0) != 1){
				PlayerPrefs.SetInt("RevmobStatus",1);
				PlayerPrefs.Save();
				ShowMessage("Great !\nAds will never appear !");
			}
		});
		#endif
	}

	private void PurchaseProduct(int skuIndex)
	{
		if(!queryInventorySucceeded){
			Debug.Log("queryInventory is not succedded yet");
			RequestProductData();
		}
		else{
			if(skuIndex==0){ // no ads
				IAP.purchaseNonconsumableProduct( skus [skuIndex], ( didSucceed, error ) =>
				                                 {
					Debug.Log( "purchasing product " + skus [skuIndex] + " result: " + didSucceed );
					
					if( !didSucceed ){
						Debug.Log( "purchase error: " + error );
					} else{
						// Purchased "no ads"
						PlayerPrefs.SetInt("RevmobStatus",1);
						ShowMessage("Great !\nAds will never appear !");
					}
				});
			} else{ // coins
				IAP.purchaseConsumableProduct( skus [skuIndex], ( didSucceed, error ) =>
				                              {
					Debug.Log( "purchasing product " + skus [skuIndex] + " result: " + didSucceed );
					
					if( !didSucceed ){
						Debug.Log( "purchase error: " + error );
					} else{
						switch(skuIndex){
						case 1: AddCoinBalance  (150000); break;
						case 2: AddCoinBalance  (250000); break;
						case 3: AddCoinBalance  (500000); break;
						case 4: AddCoinBalance  (750000); break;
						case 5: AddCoinBalance  (2000000); break;
						case 6: AddCoinBalance  (5000000); break;
						case 7: AddCoinBalance  (10000000); break;

						}
					}
				});
			}
		}
	}

	public void OnNoAds()
	{
		if(PlayerPrefs.GetInt("RevmobStatus",0) == 1)
			ShowMessage("You have already bought this item!");
		else
			PurchaseProduct (0);
	}
	public void OnAddCoin150k()
	{
		PurchaseProduct (1);
	}
	public void OnAddCoin250k()
	{
		PurchaseProduct (2);
	}
	public void OnAddCoin500k()
	{
		PurchaseProduct (3);
	}
	public void OnAddCoin750k()
	{
		PurchaseProduct (4);
	}
	public void OnAddCoin2m()
	{
		PurchaseProduct (5);
	}
	public void OnAddCoin5m()
	{
		PurchaseProduct (6);
	}
	public void OnAddCoin10m()
	{
		PurchaseProduct (7);
	}

	#endif

	void AddCoinBalance(int count)
	{
		int coins = PlayerPrefs.GetInt("pp_coins");
		coins += count;
		PlayerPrefs.SetInt("pp_coins",coins);
		PlayerPrefs.Save();
	}

	void ShowMessage(string message)
	{
		StartCoroutine (MainMenu.Instance.messagebox.Show (message));
	}

	public bool Connected{
		get{
#if UNITY_ANDROID || UNITY_IPHONE
			return queryInventorySucceeded;
#else
			return false;
#endif
		}
	}

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly  UnityEngine.Object _syncRoot = new UnityEngine.Object();
	private static volatile MobiIAB _staticInstance;	
	public static MobiIAB Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(MobiIAB)) as MobiIAB;
					if (_staticInstance == null) {
						Debug.LogError("The MobiIAB instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
