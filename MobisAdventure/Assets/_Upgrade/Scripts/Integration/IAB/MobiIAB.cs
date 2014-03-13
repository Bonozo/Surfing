using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MobiIAB : MonoBehaviour
{
	#if UNITY_ANDROID
	private string key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAymWgRnh1BakrbqiGqrF6LBNLMQY/4gHI0KlOc9u6A3kYtMN3ghqQzeIje+WzP7G6wUPeTmLUrI0WUrcQFuvsMm0xwnj9rEY66RhgvwpwC6MO/dOH0NmsMBTll3g8WgT4GHyeBojH21ZZAi5bXS49qAHh/A4wQxVudzazLoMXe9s8/4mFwJUHkByOnMDN9cnVV5wl4nf/zO4i2mZU4A2j9CLBwzVDUK6e6giLPLwpGmTtqEdfX4hfLJ7okFV8rPGXW8aZjwsSclRb4oTuRshpqqONRHvBFcN7GJUT2h43HqDYY1Ul+uTtYQIsdQFDdnXKi2G44KZnNcZrnDhtRUaWLwIDAQAB";
	private string[] skus = new string[] {@"mobisrun_99c_noads",@"mobisrun_150k_coins", @"mobisrun_250k_coins",
		@"mobisrun_500k_coins", @"mobisrun_750k_coins",@"mobisrun_2m_coins" ,@"mobisrun_5m_coins" ,@"mobisrun_10m_coins"  };
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
		//ShowMessage ("billingSupportedEvent");
	}

	void billingNotSupportedEvent( string error )
	{
		Debug.Log( "billingNotSupportedEvent: " + error );
		//ShowMessage("billingNotSupportedEvent: " + error );
	}
	
	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		queryInventorySucceeded = true;
		Debug.Log( "queryInventorySucceededEvent" );

		int total_coins_purchases = 0;
		for(int i=0;i<purchases.Count;i++)
		{
			if(purchases[i].productId == this.skus[0] && PlayerPrefs.GetInt("RevmobStatus",0) == 0)
			{
				PlayerPrefs.SetInt("RevmobStatus",1);
				PlayerPrefs.Save();
				ShowMessage("Great !\nAds will never appear !");
			}
			else
			{
				//ShowMessage(purchases[i].productId);
				for(int j=1;j<this.skus.Length;j++)
				{
					if( this.skus[j] == purchases[i].productId)
					{
						total_coins_purchases += coinsOfProducts[j];
						break;
					}
				}
			}
		}

		// this is for second safe, but no actually working
		// if the user bought the same unmanaged product multiple time
		// it calculated only once
		int must = PlayerPrefs.GetInt ("mobigoogleiappt", 0);
		if(total_coins_purchases < must)
		{
			int addref = must - total_coins_purchases;
			
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + addref);
			Coin_Counter.GetUpBalance();
			PlayerPrefs.SetInt("mobigoogleiappt",total_coins_purchases);
			PlayerPrefs.Save();
		}

		for(int i=1;i<this.skus.Length;i++)
			GoogleIAB.consumeProduct(this.skus[i]);
	}

	void queryInventoryFailedEvent( string error )
	{
		Debug.Log( "queryInventoryFailedEvent: " + error );
		//ShowMessage("queryInventoryFailedEvent: " + error );
	}
	
	void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
	{
		Debug.Log( "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
		//ShowMessage("purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
	}

	void AddTotalPurchase(int amount)
	{
		int total = PlayerPrefs.GetInt("mobigoogleiappt",0);
		total += amount;
		PlayerPrefs.SetInt("mobigoogleiappt",total);
	}

	void purchaseSucceededEvent( GooglePurchase purchase )
	{
		Debug.Log( "purchaseSucceededEvent: " + purchase );
		//ShowMessage("purchaseSucceededEvent: " + purchase );

		
		if (purchase.productId.ToString() == skus[0] )
		{
			PlayerPrefs.SetInt("RevmobStatus",1);
			ShowMessage("Great !\nAds will never appear !");
		}
		else if( purchase.productId.ToString() == skus[1] )
		{
			AddCoinBalance  (150000);
			AddTotalPurchase(150000);
			GoogleIAB.consumeProduct(skus[1]);
		}
		else if ( purchase.productId.ToString() == skus[2] )
		{
			AddCoinBalance  (250000);
			AddTotalPurchase(250000);
			GoogleIAB.consumeProduct(skus[2]);
		}
		else if (purchase.productId.ToString() == skus[3] )
		{
			AddCoinBalance  (500000);
			AddTotalPurchase(500000);
			GoogleIAB.consumeProduct(skus[3]);
		}
		else if (purchase.productId.ToString() == skus[4] )
		{
			AddCoinBalance  (750000);
			AddTotalPurchase(750000);
			GoogleIAB.consumeProduct(skus[4]);
		}
		else if (purchase.productId.ToString() == skus[5] )
		{
			AddCoinBalance  (2000000);
			AddTotalPurchase(2000000);
			GoogleIAB.consumeProduct(skus[5]);
		}
		else if (purchase.productId.ToString() == skus[6] )
		{
			AddCoinBalance  (5000000);
			AddTotalPurchase(5000000);
			GoogleIAB.consumeProduct(skus[6]);
		}
		else if (purchase.productId.ToString() == skus[7] )
		{
			AddCoinBalance  (10000000);
			AddTotalPurchase(10000000);
			GoogleIAB.consumeProduct(skus[7]);
		}

		PlayerPrefs.Save ();
	}

	void AddCoinBalance(int count)
	{
		int coins = PlayerPrefs.GetInt("pp_coins");
		coins += count;
		PlayerPrefs.SetInt("pp_coins",coins);
		PlayerPrefs.Save();
	}
	
	void purchaseFailedEvent( string error )
	{
		Debug.Log( "purchaseFailedEvent: " + error );
		ShowMessage("Unable to purchase the product.");
	}
	
	void consumePurchaseSucceededEvent( GooglePurchase purchase )
	{
		Debug.Log( "consumePurchaseSucceededEvent: " + purchase );
		//ShowMessage ("Consumed\n" + purchase);
	}
	
	void consumePurchaseFailedEvent( string error )
	{
		Debug.Log( "consumePurchaseFailedEvent: " + error );
		//ShowMessage("Consume Failed: " + error );
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

	void ShowMessage(string message)
	{
		StartCoroutine (MainMenu.Instance.messagebox.Show (message));
	}

	public bool Connected{
		get{
#if UNITY_ANDROID
			return queryInventorySucceeded;
#elif UNITY_IPHONE
			return false; // to do
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
