using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class IABUIManager : MonoBehaviourGUI
{
#if UNITY_ANDROID
	void OnGUI()
	{
		beginColumn();

		if( GUILayout.Button( "Initialize IAB" ) )
		{
			var key = "your public key from the Android developer portal here";
			key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlKs9XEn5clhFXKswADyNBBwB1B5YrMhEt2mwCrjQSF+tZgF7CWemIn3+McSfPZF/Jb/20R5sQCFZHDdDRIS/uwxMVVQn0ZIKLbJMJDCF5B55Pu6Sgk3VDzeTmrXou/VsxaY9JcQEe68RchTZzdpQoG74u9rgkPsHkavtrlgC5TUBOlvvemjlWvzvxxunZ2C/i5Vixxq+aUrHYD/TaR4rM0TM991JbOAY3Ro6Rhj3qooMGK++2NxM0x25SEyu3AsGdJVPPRpXSv8/oYWPXCstDbeDRVqmox4X0w+T2ThHSMXiqD6DSlprOTut7aNwvMVvpfjIDtxuxisaJ3FBPCvPxwIDAQAB";
		  //key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAmffbbQPr/zqRjP3vkxr1601/eKsXm5kO2NzQge8m7PeUj5V+saeounyL34U8WoZ3BvCRKbw6DrRLs2DMoVuCLq7QtJggBHT/bBSHGczEXGIPjWpw6OQb24EWM0PaTRTH2x2mC/X6RwIKcPLJFmy68T38Eh0DXnF4jjiIoaD0W8AYLjLzv0WvbIfgtJlvmmwvI2/Kta1LRnW3/Ggi5jb9UmXZAUIBz8kQtSH5FUCmFOQHMzekfg8rQ4VO1nlWhnB58UPwsxWt/DNyDfqv2VMeA2+VJG0fkiMl/6vWA7+ianVTU3owXcvxJHseEDUVYo1wEKfhK7ErGB7sxDJx5wHXAwIDAQAB";
			GoogleIAB.init( key );
		}


		if( GUILayout.Button( "Query Inventory" ) )
		{
			// enter all the available skus from the Play Developer Console in this array so that item information can be fetched for them
			var skus = new string[] { @"hkr_99c_noads",@"hkr_150k_coins", @"hkr_250k_coins",
				@"hkr_500k_coins", @"hkr_750k_coins",@"hkr_2m_coins" ,@"hkr_5m_coins" ,@"hkr_10m_coins",
				@"hkr_offer1",@"hkr_offer2" };
			GoogleIAB.queryInventory( skus );
		}


		if( GUILayout.Button( "Are subscriptions supported?" ) )
		{
			Debug.Log( "subscriptions supported: " + GoogleIAB.areSubscriptionsSupported() );
		}


		if( GUILayout.Button( "Purchase Test Product" ) )
		{
			GoogleIAB.purchaseProduct( "android.test.purchased" );
		}


		if( GUILayout.Button( "Consume Test Purchase" ) )
		{
			GoogleIAB.consumeProduct( "android.test.purchased" );
		}


		if( GUILayout.Button( "Test Unavailable Item" ) )
		{
			GoogleIAB.purchaseProduct( "android.test.item_unavailable" );
		}


		endColumn( true );


		if( GUILayout.Button( "Purchase Real Product" ) )
		{
			GoogleIAB.purchaseProduct( "com.prime31.testproduct", "payload that gets stored and returned" );
		}


		if( GUILayout.Button( "Purchase Real Subscription" ) )
		{
			GoogleIAB.purchaseProduct( "com.prime31.testsubscription", "subscription payload" );
		}


		if( GUILayout.Button( "Consume Real Purchase" ) )
		{
			GoogleIAB.consumeProduct( "com.prime31.testproduct" );
		}


		if( GUILayout.Button( "Enable High Details Logs" ) )
		{
			GoogleIAB.enableLogging( true );
		}


		if( GUILayout.Button( "Consume Multiple Purchases" ) )
		{
			var skus = new string[] { "com.prime31.testproduct", "android.test.purchased" };
			GoogleIAB.consumeProducts( skus );
		}

		endColumn();
	}
#endif
}
