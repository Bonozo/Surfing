using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MobiIAB : MonoBehaviour
{
	#if UNITY_ANDROID
	private string key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAymWgRnh1BakrbqiGqrF6LBNLMQY/4gHI0KlOc9u6A3kYtMN3ghqQzeIje+WzP7G6wUPeTmLUrI0WUrcQFuvsMm0xwnj9rEY66RhgvwpwC6MO/dOH0NmsMBTll3g8WgT4GHyeBojH21ZZAi5bXS49qAHh/A4wQxVudzazLoMXe9s8/4mFwJUHkByOnMDN9cnVV5wl4nf/zO4i2mZU4A2j9CLBwzVDUK6e6giLPLwpGmTtqEdfX4hfLJ7okFV8rPGXW8aZjwsSclRb4oTuRshpqqONRHvBFcN7GJUT2h43HqDYY1Ul+uTtYQIsdQFDdnXKi2G44KZnNcZrnDhtRUaWLwIDAQAB";
	private string[] skus = new string[] {@"mobi_99c_noads",@"mobi_150k_coins", @"mobi_250k_coins",
		@"mobi_500k_coins", @"mobi_750k_coins",@"mobi_2m_coins" ,@"mobi_5m_coins" ,@"mobi_10m_coins"  };
	private int[] coinsOfProducts = new int[] {0,150000, 250000, 500000,750000,2000000,5000000,10000000};
	private bool billingSupported = false;
	private bool queryInventorySucceeded = false;

	void OnEnable()
	{
		// Listen to all events for illustration purposes
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	}
	//661762773850900
	//538336272885272
	
	void OnDisable()
	{
		// Remove all event handlers
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}
	
	
	
	void billingSupportedEvent()
	{
		Debug.Log( "billingSupportedEvent" );
		billingSupported = true;
		//GoogleIAB.queryInventory( skus );
		
		//ProgressView.StopProgress(true);
		//AlertView.ShowAlert( "billingSupportedEvent: success " ,true);
	}

	void billingNotSupportedEvent( string error )
	{
		Debug.Log( "billingNotSupportedEvent: " + error );
		
		//ProgressView.StopProgress(true);
		//		AlertView.ShowAlert( "billingNotSupportedEvent:\n " + error,true);
	}
	
	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		queryInventorySucceeded = true;
		Debug.Log( "queryInventorySucceededEvent" );
		Prime31.Utils.logObject( purchases );
		Prime31.Utils.logObject( skus );
		GoogleSkuInfo[] list=skus.ToArray();
		string str="";
		for(int i=0;i<list.Length;i++)
			str+=","+list[i].ToString();
		
		GooglePurchase[] list1=purchases.ToArray();
		string str1="";
		for(int i=0;i<list1.Length;i++)
			str1+=","+list1[i].ToString();
		
		//ProgressView.StopProgress(true);
		//		AlertView.ShowAlert( "queryInventorySucceededEvent\n"+str+"\n"+str1,true);
	}

	void queryInventoryFailedEvent( string error )
	{
		Debug.Log( "queryInventoryFailedEvent: " + error );
		
		//ProgressView.StopProgress(true);
		//AlertView.ShowAlert(  "queryInventoryFailedEvent:\n " + error ,true);
		//labelAlertGameObject.SetActive(true);
		//labelAlert.text = "queryInventoryFailedEvent:" + error;
	}
	
	void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
	{
		Debug.Log( "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
		
		//		ProgressView.StopProgress(true);
		//		AlertView.ShowAlert("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature,true);
	}

	void purchaseSucceededEvent( GooglePurchase purchase )
	{
		Debug.Log( "purchaseSucceededEvent: " + purchase );
		
		/*for (int i=0; i<skus.Length; i++) {
			if (purchase.productId.ToString () == skus[i]) {
				ProgressView.StopProgress (true);
				UserSettings.AddCoins(coinsOfProducts[i]);
				UserSettings.Save();
				ServerController.SharedInstance.SaveSettings();
				AlertView.ShowAlert("Success in purchasing of coins.\nU have got "+coinsOfProducts[i]+"coins.",true);
				return;
			}
		}*/
		
		//loadImage.SetActive(false);
		//GameObject.Find ("Panel_UnlockLevel").SetActive(false);
		//labelAlertGameObject.SetActive(true);
		//labelAlert.text = "Success in purchasing of Unlock Level";

		//GameObject.Find("LoadingText").SetActive( false );
		//GameObject.Find("LoadingImage").SetActive( false );

		
		if (purchase.productId.ToString() == skus[0] )
		{
			// no ads implement here
		}
		else if( purchase.productId.ToString() == skus[1] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 150000);
			Coin_Counter.GetUpBalance();
		}
		else if ( purchase.productId.ToString() == skus[2] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 250000);
			Coin_Counter.GetUpBalance();
		}
		else if (purchase.productId.ToString() == skus[3] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 500000);
			Coin_Counter.GetUpBalance();
		}
		else if (purchase.productId.ToString() == skus[4] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 750000);
			Coin_Counter.GetUpBalance();
		}
		else if (purchase.productId.ToString() == skus[5] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 2000000);
			Coin_Counter.GetUpBalance();
		}
		else if (purchase.productId.ToString() == skus[6] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 5000000);
			Coin_Counter.GetUpBalance();
		}
		else if (purchase.productId.ToString() == skus[7] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 10000000);
			Coin_Counter.GetUpBalance();
		}

		PlayerPrefs.Save ();
	}
	
	void purchaseFailedEvent( string error )
	{
		Debug.Log( "purchaseFailedEvent: " + error );
		//ProgressView.StopProgress(true);
		//AlertView.ShowAlert( "purchaseFailedEvent: \n" + error,true);
		//labelAlertGameObject.SetActive(true);
		//labelAlert.text = "purchaseFailedEvent:" + error;
	}
	
	void consumePurchaseSucceededEvent( GooglePurchase purchase )
	{
		Debug.Log( "consumePurchaseSucceededEvent: " + purchase );
		
		//ProgressView.StopProgress(true);
		//AlertView.ShowAlert( "consumePurchaseSucceededEvent: " + purchase,true);
	}
	
	void consumePurchaseFailedEvent( string error )
	{
		Debug.Log( "consumePurchaseFailedEvent: " + error );
		//ProgressView.StopProgress(true);
		//AlertView.ShowAlert( "consumePurchaseFailedEvent: " + error,true);
	}

	void Init()
	{
		GoogleIAB.init( key );
	}
	
	public void OnAddCoinFromProduct (GameObject button) {
		//int index = int.Parse(button.name.Substring(3)) - 1;
		//ProgressView.SetText("Retrieving product...");
		//ProgressView.StartProgress(true);
		
		//AlertView.ShowAlert( "Start purchase" + skus[index],true);
		//loadImage.SetActive(true);
		GoogleIAB.purchaseProduct(skus[0]);
	}
	
	IEnumerator Start()
	{
		Init ();
		while(!billingSupported) yield return null;
		while(!queryInventorySucceeded)
		{
			GoogleIAB.queryInventory( skus );
			yield return new WaitForSeconds(1f);
		}
	}

	private void PurchaseProduct(int skuIndex)
	{
		if(!queryInventorySucceeded)
		{
			Debug.Log("queryInventory is not succedded yet");
		}
		else
		{
			Debug.Log ("purchasing: " + skus [skuIndex]);
			GoogleIAB.purchaseProduct (skus [skuIndex]);
		}
	}

	public void OnNoAds()
	{
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

	public bool Connected { get { return queryInventorySucceeded; } }

	#endif

	#if UNITY_IPHONE
	public bool Connected { get { return false; } } // to do
	// ... ???????
	#endif

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
