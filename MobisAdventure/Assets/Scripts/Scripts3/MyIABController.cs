using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MyIABController : MonoBehaviour
{
#if UNITY_ANDROID
	//public GameObject loadImage;

	//public GameObject labelAlertGameObject;
	private string[] skus = new string[] {@"150k_coins", @"250k_coins", @"2m_coins", @"500k_coins", @"750k_coins", @"99c_noads"};
	private int[] coinsOfProducts = new int[] {150000, 250000, 2000000, 500000, 750000, 0};
	private int currentLevel;
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
		GoogleIAB.queryInventory( skus );
		
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
		GameObject.Find("LoadingText").SetActive( false );
		GameObject.Find("LoadingImage").SetActive( false );
		if( purchase.productId.ToString() == skus[0] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 150000);
			Coin_Counter.GetUpBalance();
			return;
		}
		else if ( purchase.productId.ToString() == skus[1] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 250000);
			Coin_Counter.GetUpBalance();
			return;
		}
		else if (purchase.productId.ToString() == skus[2] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 2000000);
			Coin_Counter.GetUpBalance();
			return;
		}
		else if (purchase.productId.ToString() == skus[3] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 500000);
			Coin_Counter.GetUpBalance();
			return;
		}
		else if (purchase.productId.ToString() == skus[4] )
		{
			int bal = PlayerPrefs.GetInt("pp_coins",0);
			PlayerPrefs.SetInt("pp_coins",bal + 750000);
			Coin_Counter.GetUpBalance();
			return;
		}
		else if (purchase.productId.ToString() == skus[5] )
		{
			PlayerPrefs.SetInt("RevmobStatus",1);
			return;
		}
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

	void Start(){
		var key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgkocxhCWWAmIhGHo2MwzBk0JYEiYEeHeLU4BOcw0Z0IulbkN8yi3Y/wMVM4EKswRiHEWDBrKDvxyo2PSEsxI8CyuI6HUZyBQHckMIe+Z7Nlr4bbSRvBKmCgWVmanBTN5gdtlPFaZCRgTaPAHV0fL1uey/tPTBi/Y9LobuciBjNKQtWnJCEZDjJW7Tl8Z4LIaAHxy67aKfzU4KjjPElNSpDbhLeN/pC/RifxKMGYH/uwh9bkp8lP/jN4xkY5g6U8E7DcePTFrjo/Mow4p9ogcn4XXh2OA4HF/fJvmy3Q1W7aeGgclPwE5Bo6M4LRXu4/0oU77cb6AId9LZMigt15FjwIDAQAB";
		GoogleIAB.init( key );
	}
	
	void setCurrentLevel( int level)
	{
		currentLevel = level;	
	}
	 
	public void OnAddCoinFromProduct (GameObject button) {
		//int index = int.Parse(button.name.Substring(3)) - 1;
		//ProgressView.SetText("Retrieving product...");
		//ProgressView.StartProgress(true);
		
		//AlertView.ShowAlert( "Start purchase" + skus[index],true);
		//loadImage.SetActive(true);
		GoogleIAB.purchaseProduct(skus[0]);
	}
	

	
	public void OnAddCoin150k()
	{
		GoogleIAB.purchaseProduct(skus[0]);
	}
	
	public void OnAddCoin250k()
	{
		GoogleIAB.purchaseProduct(skus[1]);
	}
	
	
	public void OnAddCoin2m()
	{
		GoogleIAB.purchaseProduct(skus[2]);
	}
	public void OnAddCoin500k()
	{
		GoogleIAB.purchaseProduct(skus[3]);
	}
	public void OnAddCoin750k()
	{
		GoogleIAB.purchaseProduct(skus[4]);
	}
	public void OnNoAds()
	{
		GoogleIAB.purchaseProduct(skus[5]);
	}
	
#endif
}
