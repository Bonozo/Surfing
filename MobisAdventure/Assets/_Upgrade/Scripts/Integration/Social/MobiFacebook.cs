using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class MobiFacebook : MonoBehaviourGUI {

	public string fbname{get;private set;}
	public string fbfirstname{get;private set;}

	public bool Working{ get; private set; }
	public bool Result{ get; private set; }
	public bool LoggedIn{ get; private set; }
	
	private string screenshotFilename = "ingamerandomscreenshot.png";
	public void CaptureScreenShoot()
	{
		Application.CaptureScreenshot( screenshotFilename );
	}

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		Working = false;
		Result = false;
	}
	
	// Open All Events
	void OnEnable()
	{
		/*FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;
		
		FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedEvent;
		FacebookManager.dialogFailedEvent += dialogFailedEvent;
		
		FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent += restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent += restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;
		
		FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;*/
	}

	void OnDisable()
	{
		// Remove all the event handlers when disabled
		/*FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
		FacebookManager.loginFailedEvent -= loginFailedEvent;
		
		FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedEvent;
		FacebookManager.dialogFailedEvent -= dialogFailedEvent;
		
		FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent -= restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent -= restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;
		
		FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;*/
	}

	void Start()
	{
		LoggedIn = false;
		Init ();
	}
	
	public void Init()
	{
		Debug.Log ("Init called");
		#if UNITY_ANDROID
		FacebookAndroid.init();
		#elif UNITY_IPHONE
		FacebookBinding.init();
		#endif
	}

	#region Login

	public IEnumerator Login()
	{
		Working = true;
		#if UNITY_ANDROID
		FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
		#elif UNITY_IPHONE
		FacebookBinding.loginWithReadPermissions(new string[] { "email", "user_birthday" });
		#endif
		while(Working) yield return null;
	}

	void facebookLoginFailed( string error )
	{
		Debug.Log("Facebook login failed: " + error);
		Result = false;
		Working = false;
	}

	void loginFailedEvent( string error )
	{
		Debug.Log( "Facebook login failed: " + error );
		Result = false;
		Working = false;
	}

	void sessionOpenedEvent()
	{
		Debug.Log( "Successfully logged in to Facebook" );
		LoggedIn = true;
		Result = true;
		Working = false;
		/*#if UNITY_ANDROID
		Facebook.instance.graphRequest( "me", GetFacebookNamesHandler );
		#elif UNITY_IPHONE
		Facebook.instance.graphRequest( "me", GetFacebookNamesHandler );
		#endif*/
	}

	void GetFacebookNamesHandler( string error, object result )
	{
		Debug.Log ("me request: " + error);
		if(error==null)
		{
			var ht = result as Hashtable;
			fbname = ht["name"].ToString();
			fbfirstname = ht["first_name"].ToString();
			Debug.Log("Welcome, " + fbname);
			Result = true;
		}
		else
			Result = false;
		Working = false;
	}

	#endregion

	#region Reutorize with Publish Permissin

	public IEnumerator ReutorizeWithPublishPermission()
	{
		Working = true;	
		#if UNITY_ANDROID
		FacebookAndroid.reauthorizeWithPublishPermissions( new string[] { "publish_actions" },FacebookSessionDefaultAudience.Everyone );
		#elif UNITY_IPHONE
		FacebookBinding.reauthorizeWithPublishPermissions( new string[] { "publish_actions", "publish_stream" },FacebookSessionDefaultAudience.Everyone );//??
		#endif
		while(Working) yield return null;
	}

	void reauthorizationSucceededEvent()
	{
		Debug.Log( "reauthorizationSucceededEvent" );
		Result = true;
		Working = false;
	}
	
	void reauthorizationFailedEvent( string error )
	{
		Debug.Log( "reauthorizationFailedEvent: " + error );
		Result = false;
		Working = false;
	}

	#endregion

	#region Logout

	public void Logout()
	{
		#if UNITY_ANDROID
		FacebookAndroid.logout();
		#elif UNITY_IPHONE
		FacebookBinding.logout();
		#endif
	}

	void facebookDidLogoutEvent()
	{
		fbname = "";
		fbfirstname = "";
		LoggedIn = false;
	}

	#endregion

	#region Post On Wall

	public IEnumerator PostOnWall(int scores,string level)
	{
		Working = true;
		//var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
		//var bytes = System.IO.File.ReadAllBytes( pathToImage );
		#if UNITY_ANDROID || UNITY_IPHONE
		//Facebook.instance.postMessage("Message",PostedHandler);
		Facebook.instance.postMessageWithLinkAndLinkToImage(
			"I reached " + scores + " meters in " + level + " level.",
			"http://www.mobitekgames.com/games",
			"Monster Hill Racing",
			"http://www.mobitekgames.com/sites/default/files/imagepicker/2/LogoButton2.png",
			"", 
			PostedHandler );
		//Facebook.instance.postImage( bytes, posttext, PostedHandler );
		#endif

		while(Working) yield return null;
	}
	
	private void PostedHandler( string error, object result )
	{
		Debug.Log ("PostOnWall: " + error);
		Result = (error == null);
		Working = false;
	}

	#endregion

	public bool isSessionValid()
	{
		#if UNITY_ANDROID
		return FacebookAndroid.isSessionValid();
		#elif UNITY_IPHONE
		return FacebookBinding.isSessionValid();
		#else
		return false;
		#endif
	}
	
	#if UNITY_IPHONE || UNITY_ANDROID

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

	void dialogDidNotCompleteEvent()
	{
		Debug.Log( "facebookDialogDidntComplete" );
	}

	void graphRequestCompletedEvent( object obj )
	{
		Debug.Log( "graphRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}
	
	void facebookCustomRequestFailed( string error )
	{
		Debug.Log( "facebookCustomRequestFailed failed: " + error );
	}
	
	void restRequestCompletedEvent( object obj )
	{
		Debug.Log( "restRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}

	void restRequestFailedEvent( string error )
	{
		Debug.Log( "restRequestFailedEvent failed: " + error );
	}
		
	void facebookComposerCompletedEvent( bool didSucceed )
	{
		Debug.Log( "facebookComposerCompletedEvent did succeed: " + didSucceed );
	}
		

	
	#endif

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly  UnityEngine.Object _syncRoot = new UnityEngine.Object();
	private static volatile MobiFacebook _staticInstance;	
	public static MobiFacebook Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(MobiFacebook)) as MobiFacebook;
					if (_staticInstance == null) {
						Debug.LogError("The MobiFacebook instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}