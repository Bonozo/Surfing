using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class MyFacebook : MonoBehaviour
{
	private string social_id="1231231";
	private string social_name="socialname1";
	private string social_picture_url="socialpicture1";
	private string email="FakeEmail@gmail.com";
	public GameObject aaa;
#if UNITY_IPHONE || UNITY_ANDROID
	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;

		FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedEvent;
		FacebookManager.dialogFailedEvent += dialogFailedEvent;

		FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent += restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent += restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;

		FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
	}


	void OnDisable()
	{
		// Remove all the event handlers when disabled
		FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
		FacebookManager.loginFailedEvent -= loginFailedEvent;

		FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedEvent;
		FacebookManager.dialogFailedEvent -= dialogFailedEvent;

		FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent -= restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent -= restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;

		FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
	}



	void sessionOpenedEvent()
	{
		Debug.Log( "Successfully logged in to Facebook" );
		//AlertView.ShowAlert("Successfully logged in to Facebook",true);
		//Facebook.instance.graphRequest( "me/likes", completionHandler );
		//aaa.SetActive( true );
		//GameObject.Find("MyFacebookController").SendMessage("ShareScreenShotToFaceBook",SendMessageOptions.DontRequireReceiver);
		//ShareScreenShotToFaceBook();
	}


	void loginFailedEvent( string error )
	{
		Debug.Log( "Facebook login failed: " + error );
		//AlertView.ShowAlert("login failed: "+error,true);
		//UserSettings.SharedSettings.IsFacebookLogIn=false;
		//ProgressView.StopProgress(true);
		//AlertView.ShowAlert("Your phone can't access to Facebook\n"+error,true);
		
	}


	void dialogCompletedEvent( string url )
	{
		Debug.Log( "dialogCompletedEvent: " + url );
	}


	void dialogFailedEvent( string error )
	{
		Debug.Log( "dialogFailedEvent: " + error );
	}


	void facebokDialogCompleted()
	{
		Debug.Log( "facebokDialogCompleted" );
	}

	
	void graphRequestCompletedEvent( object obj )
	{
		//AlertView.ShowAlert("graphRequest complete event",true);
		
		Debug.Log( "graphRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
		
		if(obj==null){
			Debug.Log("graphRequest Obj is null");
			//AlertView.ShowAlert("graphRequest Obj is null",true);
			return;
		}
		
		var ht=obj as Hashtable;
		social_id=ht["id"].ToString();
		social_name=ht["name"].ToString();
		social_picture_url=ht["picture"].ToString();
		email=ht["email"].ToString();
		
		//AlertView.ShowAlert(social_id +" "+social_name +" "+social_picture_url +" "+email,true);
		
		//UserSettings.SharedSettings.username="";
		//UserSettings.SharedSettings.email = email;
		//UserSettings.SharedSettings.social_id = social_id;
		//UserSettings.SharedSettings.social_name = social_name;
		//UserSettings.SharedSettings.social_picture_url = social_picture_url;
		//ServerController.SharedInstance.Login(email,"");
	}


	void facebookCustomRequestFailed( string error )
	{
		//AlertView.ShowAlert("graphRequest failed event"+error,true);
		Debug.Log( "facebookCustomRequestFailed failed: " + error );
		aaa.SetActive( true );

	}
	
	void facebookReceivedCustomRequest( object obj )
	{
		//GameObject.Find("LoadingText").SetActive( false );
		//GameObject.Find("LoadingImage").SetActive( false );
	}


	void restRequestCompletedEvent( object obj )
	{
		//AlertView.ShowAlert("rest Request compelte",true);
		Debug.Log( "restRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
		
	}


	void restRequestFailedEvent( string error )
	{
		//AlertView.ShowAlert("rest Request fail"+error,true);
		//GameObject.Find("LoadingText").SetActive( false );
		//GameObject.Find("LoadingImage").SetActive( false );
		Debug.Log( "restRequestFailedEvent failed: " + error );
	}


	void facebookComposerCompletedEvent( bool didSucceed )
	{
		Debug.Log( "facebookComposerCompletedEvent did succeed: " + didSucceed );
	}


	void reauthorizationSucceededEvent()
	{
		
		Debug.Log( "reauthorizationSucceededEvent" );
//		Facebook.instance.graphRequest( "me", completionHandler );
	}


	void reauthorizationFailedEvent( string error )
	{
		//GameObject.Find("LoadingText").SetActive( false );
		//GameObject.Find("LoadingImage").SetActive( false );
		Debug.Log( "reauthorizationFailedEvent: " + error );
	}

#endif
	
#if UNITY_ANDROID
	public static string screenshotFilename = "someScreenshot.png";
	
	
	// common event handler used for all Facebook graph requests that logs the data to the console
	void completionHandler( string error, object obj )
	{
		
		if( error != null ){
			Debug.LogError( error );
			//UserSettings.SharedSettings.IsFacebookLogIn=false;
			//AlertView.ShowAlert("|||| "+error+" ||||",true);
		}
		else{
			
			//Debug.Log( "graphRequestCompletedEvent" );
			//Prime31.Utils.logObject( obj );
			var ht=obj as Hashtable;
			
			if(PostImageFlag==true){
				PostImageFlag=false;
				//AlertView.ShowAlert("Shared your game to Facebook sucessfully!",true);
				//ProgressView.StopProgress(true);
				//GameObject.Find("LoadingText").SetActive( false );
				//GameObject.Find("LoadingImage").SetActive( false );
				int bal = PlayerPrefs.GetInt("pp_coins",0);
				PlayerPrefs.SetInt("pp_coins",bal + 5000);
				PlayerPrefs.SetInt("facebookStatus_mobisrun",1);
				Coin_Counter.GetUpBalance();
				return;
			}
			if(obj==null){
				Debug.Log("graphRequest Obj is null");
				//AlertView.ShowAlert("graphRequest Obj is null",true);
				return;
			}
			
			if(ht["id"]!=null)
				social_id=ht["id"].ToString();
			if(ht["name"]!=null)
				social_name=ht["name"].ToString();
		
			if(ht["email"]!=null)
				email=ht["email"].ToString();
		
			if(ht["picture"]!=null)
				social_picture_url=ht["picture"].ToString();
			
//			AlertView.ShowAlert("id:"+social_id+"   name:"+social_name+"\nemail:"+email+"  Picture:"+social_picture_url,true);
			
			//UserSettings.SharedSettings.username="";
			//UserSettings.SharedSettings.email = email;
			//UserSettings.SharedSettings.social_id = social_id;
			//UserSettings.SharedSettings.social_name = social_name;
			//UserSettings.SharedSettings.social_picture_url = social_picture_url;
			//UserSettings.SharedSettings.IsFacebookLogIn=true;
			//ServerController.SharedInstance.Login(email,"");
		}
	}
	
	
	public void StartFaceBookLogIn()
	{
		//ProgressView.SetText("Log In with Facebook...");
		//ProgressView.StartProgress(true);
		
		//UserSettings.SharedSettings.IsFacebookLogIn=true;
		
		FacebookAndroid.init();
		PostImageFlag=false;
		print("start log in");
		
		var isSessionValid = FacebookAndroid.isSessionValid();
//		if(isSessionValid==true){
//			print("Already logged in");
//			//FacebookAndroid.reauthorizeWithPublishPermissions( new string[] { "publish_actions", "manage_friendlists" }, FacebookSessionDefaultAudience.EVERYONE );
//			//Facebook.instance.graphRequest( "me", completionHandler );
//		}
//		else{
//			print("loging in");
		FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday"} );
			//FacebookAndroid.loginWithPublishPermissions (new string[] { "publish_stream", "photo_upload" });
		//}
	}
	
	void ShareScreenShotToFaceBook(){
		//if(!UserSettings.SharedSettings.IsFacebookLogIn){
			//AlertView.ShowAlert ("You didn't login with Facebook!\nTo share your screenshot to facebook\n,please logout and login with facebook again!",true);
		//	return;
		//}
		//ProgressView.SetText("Sharing your screenshot to Facebook...");
		//ProgressView.StartProgress (true);
		StartCoroutine(myfunc());
		Debug.Log ("SSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
	}
	
	private bool PostImageFlag=false;
	
	IEnumerator myfunc(){
		Application.CaptureScreenshot( screenshotFilename );
		yield return new WaitForSeconds(2f);
		
		var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
		//AlertView.ShowAlert(pathToImage,true);
		
		var bytes = System.IO.File.ReadAllBytes( pathToImage );
		//Texture2D aa=new Texture2D(Screen.width,Screen.height);
		//aa.LoadImage (bytes);
		//GameObject.Find ("Cube").renderer.material.mainTexture=aa;
		if(bytes!=null){
			PostImageFlag=true;
			Facebook.instance.postImage( bytes, "Hi friend!", completionHandler );
			//AlertView.ShowAlert("Shared your game to Facebook sucessfully!",true);
		}else
			//AlertView.ShowAlert("Null entered",true);
			Debug.Log ("AAA");
	}

#endif
	
}
