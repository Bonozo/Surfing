using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class Menu_0 : MonoBehaviour {
	
	public GameObject n_menu_0;
	public GameObject n_menu_1;
	public GameObject n_menu_2;
	public GameObject obj_menu_0;
	public GameObject obj_menu_1;
	public GameObject obj_menu_2;
	
	public GameObject b_facebook;
	public GameObject b_store;
	public GameObject b_home;
	public Transform storeMenu;
	
	public GameObject loadingText;
	public GameObject loadingImage;
	public GameObject errorFacebook;
	public GameObject errorFacebook1;
	// Subscribe to events
	
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
				
		//	SpawnText("662294140452463");
	}

	void OnDisable(){
		UnsubscribeEvent();
		//FacebookManager.sessionOpenedEvent -= facebookLoginSuccessful;

	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}
	
	void billingSupportedEvent()
	{
		Debug.Log( "billingSupportedEvent" );
	
		
	}
	
	void billingNotSupportedEvent( string error )
	{
		Debug.Log( "billingNotSupportedEvent: " + error );
	}
	
	void Start(){
		//facebook
		//GameObject.Find("MyFacebookController").SendMessage("StartFaceBookLogIn",SendMessageOptions.DontRequireReceiver);
		//--------------- End facebook

		//MONEY

		//--------------- End money
		
		Ride_menu.m_swipeHoldR = true;
//		RotateIt(obj_menu_1, 90);
//		RotateIt(obj_menu_2, 180);
//		obj_menu_1.SetActive (false);
		obj_menu_2.SetActive (false);
		
		
	}
	
	void Update(){
		CallStore();	
		if ( errorFacebook.activeSelf && errorFacebook.animation.isPlaying == false )
			errorFacebook.SetActive ( false );
		
		// Quit App when pressing back button of the device and main title scrren is active
		if( obj_menu_0.activeSelf && Input.GetKeyUp(KeyCode.Escape) )
			Application.Quit();
	}
	
	// Simple tap
	private void On_SimpleTap( Gesture gesture){
		
		if (gesture.pickObject == n_menu_0){
//			RotateIt(obj_menu_0, -90);
//			RotateIt(obj_menu_1, 0);
			obj_menu_0.SetActive (false);
			b_home.SetActive(true);
//			obj_menu_1.SetActive (true);
			
			Ride_menu.m_swipeHoldR = false;
		} else if (gesture.pickObject == n_menu_1 && Ride_menu.ClearForLevel()){
//			RotateIt(obj_menu_1, -90);
//			RotateIt(obj_menu_2, 0);
//			obj_menu_1.SetActive (false);
			obj_menu_2.SetActive (true);
			
			Ride_menu.m_swipeHoldR = true;
			Level_menu.m_swipeHoldL = false;
		} else if (gesture.pickObject == n_menu_2 && Level_menu.ClearForGame()){
			//play game
			Application.LoadLevel(GameManager.m_chosenLevel.ToString());
		}
		if (gesture.pickObject == b_facebook){
			//facebook
			
			//facebookLogin();
			
				//MessageBox.Show(Callback, "Hello World!", "Hello", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
			
			/*if ( PlayerPrefs.GetInt("facebookStatus_mobisrun",0) == 0 )
			{
				loadingText.SetActive( true );
				loadingImage.SetActive( true );
				GameObject.Find("MyFacebookController").SendMessage("StartFaceBookLogIn",SendMessageOptions.DontRequireReceiver);
				
				//yield return new WaitForSeconds(2f);
				//GameObject.Find("MyFacebookController").SendMessage("ShareScreenShotToFaceBook",SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				errorFacebook.SetActive( true);
			}*/
			
			//Application.OpenURL("fb://page/171118409705449");
			//Application.OpenURL("https://www.facebook.com/MobitekGames");
			/*if (PlayerPrefs.GetInt("facebookStatus_mobisrun", 0) == 1)
			{
				errorFacebook.SetActive( true );
			}
			else
			{*/
				loadingText.SetActive( true );
				loadingImage.SetActive( true );
				Like("171118409705449");
				//Application.OpenURL("fb://page/171118409705449");
			//}
			//PlayerIsLoggedIn_PostToTheirWall();	
			//--------------- End facebook
		}
		if (gesture.pickObject == b_store){
			storeOnScreen = !storeOnScreen;
		}
		if (gesture.pickObject == b_home){
			Ride_menu.m_swipeHoldR = true;
//			RotateIt(obj_menu_0, 0);
//			RotateIt(obj_menu_1, 90);
//			RotateIt(obj_menu_2, 180);
			if (obj_menu_2.activeSelf){
				
				obj_menu_2.SetActive(false);
			}
			else
			{
				b_home.SetActive( false );
				obj_menu_0.SetActive (true);
			}
//			obj_menu_1.SetActive (false);
//			obj_menu_2.SetActive (false);
		}
	}
	
	public void RotateIt(GameObject obj, float targt){
		var target = Quaternion.Euler (0, targt, 0);
		obj.transform.rotation = target;
	}
	
	public void PlayerIsLoggedIn_PostToTheirWall(){
	Debug.Log("OK I WILL POST!!");
#if UNITY_IPHONE
		// parameters are optional. See Facebook's documentation for all the dialogs and paramters that they support
		Dictionary<string, string> parameters  = new Dictionary<string,string>();
			
				parameters.Add( "link", "http://mobitekgames.com" );
				parameters.Add( "name", "Mobi's Run" );
				parameters.Add( "picture", "http://mobitekgames.com/sites/all/files/mobirun_promo.jpg" );
				parameters.Add( "caption", "Mobi's Run!" );
			
			FacebookBinding.showDialog( "stream.publish", parameters );
#endif
	}
	
	
	public void facebookLogin()
	{
		Debug.Log("request!!");
		// Note: requesting publish permissions here will result in a crash. Only read permissions are permitted.
		var permissions = new string[] { "user_games_activity" };
#if UNITY_IPHONE
//		FacebookBinding.loginWithRequestedReadPermissions( permissions );
#endif
	}
	
	public void reauthPub(){
		Debug.Log("reAUTH!!!!!!!");
#if UNITY_IPHONE
		FacebookBinding.reauthorizeWithPublishPermissions( new string[] { "publish_actions" }, FacebookSessionDefaultAudience.Everyone );
#endif
	}
	public void facebookLoginSuccessful(){	
		PlayerIsLoggedIn_PostToTheirWall();
	}

	public void facebookPostSuccessful(string duck){
		//tell the game to reward the player with tie dye hammock for supporting the game on facebook.
		Coin_Counter.AddCoins(100);
	}
	
	private bool storeOnScreen = false;
	public void CallStore(){
		float step = 500.0f * Time.deltaTime;
		if(storeOnScreen){
			storeMenu.localPosition = Vector3.MoveTowards(storeMenu.localPosition, new Vector3(storeMenu.localPosition.x,-100,storeMenu.localPosition.z), step);
		} else {
			storeMenu.localPosition = Vector3.MoveTowards(storeMenu.localPosition, new Vector3(storeMenu.localPosition.x,-350,storeMenu.localPosition.z), step);
		}
	}
	
	
	
	//////////////////////////////////////
	///Facebook like
	public void Like(string likeID)
    {
    	Facebook.instance.graphRequest("https://www.facebook.com/pages/"+likeID+"/likes", HTTPVerb.POST,
		    ( error, obj ) =>
		    {
		    	if (obj != null)
		    	{
		    		Prime31.Utils.logObject( obj );
					loadingText.SetActive( false );
					loadingImage.SetActive( false );
				
		    	}
		    	if( error != null )
		    	{
		    		Debug.Log("Error liking:"+error);
					errorFacebook1.SetActive( true );
					loadingText.SetActive( false );
					loadingImage.SetActive( false );
					return;
		    	}
				PlayerPrefs.SetInt("facebookStatus_mobisrun",1);
		    	Debug.Log( "like finished: " );
		    });
    }
	
	
	
	
}


