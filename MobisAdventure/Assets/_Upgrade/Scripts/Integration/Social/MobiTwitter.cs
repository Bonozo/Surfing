using UnityEngine;
using System.Collections;

public class MobiTwitter : MonoBehaviour {
	
	public bool Working{ get; private set; }
	public bool Result{ get; private set; }
	public bool Inited{ get; private set; }
	
	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		Working = false;
		Result = false;
	}

	#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		TwitterManager.loginSucceededEvent += loginDidSucceedEvent;
		TwitterManager.loginFailedEvent += loginDidFailEvent;
		TwitterManager.requestDidFinishEvent += requestSucceededEvent;
		TwitterManager.requestDidFailEvent += requestFailedEvent;
		//TwitterManager.twitterInitializedEvent += twitterInitializedEvent;
	}

	void OnDisable()
	{
		// Listen to all events for illustration purposes
		TwitterManager.loginSucceededEvent -= loginDidSucceedEvent;
		TwitterManager.loginFailedEvent -= loginDidFailEvent;
		TwitterManager.requestDidFinishEvent -= requestSucceededEvent;
		TwitterManager.requestDidFailEvent -= requestFailedEvent;
		//TwitterManager.twitterInitializedEvent -= twitterInitializedEvent;
	}

	public IEnumerator Init()
	{
		if(Inited)
		{
			Working = false;
			Result = true;
		}
		else
		{
			Working = true;
			TwitterAndroid.init( "bZbuIhuGHAN1Fs3QuP8dDQ", "P8ckiUMXVsqnYcyyqGt1Y4Z0ZriPNxSj9slYFwCtE" );
			float time = RealTime.time + 1f;
			while(time > RealTime.time) yield return new WaitForEndOfFrame();
			Inited = true;
			Result = true;
			Working = false;
			while(Working) yield return null;
		}
	}

	public IEnumerator Login()
	{
		if(IsLoggedIn())
		{
			Result = true;
			Working = false;
		}
		else
		{
			Working = true;
			TwitterAndroid.showLoginDialog();
			while(Working) yield return null;
		}
	}

	public bool IsLoggedIn()
	{
		return TwitterAndroid.isLoggedIn();
	}

	public void Logout()
	{
		TwitterAndroid.logout();
	}

	public IEnumerator Post(int scores,string level)
	{
		Working = true; 
		TwitterAndroid.postStatusUpdate ("I reached " + scores + " meters in " + level + " level.\nHill Climb Racing\n#MobiTekGames\nhttp://www.mobitekgames.com/games");
		while(Working) yield return null;
	}

	void loginDidSucceedEvent( string username )
	{
		Debug.Log( "loginDidSucceedEvent.  username: " + username );	
		Result = true;
		Working = false;
	}	
	
	void loginDidFailEvent( string error )
	{
		Debug.Log( "loginDidFailEvent. error: " + error );
		Result = false;
		Working = false;
	}	
	
	void requestSucceededEvent( object response )
	{
		Debug.Log( "requestSucceededEvent" );
		Prime31.Utils.logObject( response );
		Result = true;
		Working = false;
	}
		
	void requestFailedEvent( string error )
	{
		Debug.Log( "requestFailedEvent. error: " + error );
		Result = false;
		Working = false;
	}
		
	void twitterInitializedEvent()
	{
		Debug.Log( "twitterInitializedEvent" );
		Inited = true;
		Result = true;
		Working = false;
	}
	#endif

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly  UnityEngine.Object _syncRoot = new UnityEngine.Object();
	private static volatile MobiTwitter _staticInstance;	
	public static MobiTwitter Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(MobiTwitter)) as MobiTwitter;
					if (_staticInstance == null) {
						Debug.LogError("The MobiTwitter instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
