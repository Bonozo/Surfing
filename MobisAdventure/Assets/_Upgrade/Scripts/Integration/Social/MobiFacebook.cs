using UnityEngine;
using System.Collections;

public class MobiFacebook : MonoBehaviour {

	private string id = "712765848745496";
	private string mobitekid = "171118409705449";

	void Start()
	{
		FacebookAndroid.init (true);
	}

	public bool IsSessionValid { get { return FacebookAndroid.isSessionValid (); } }
	public void Login()
	{
		FacebookAndroid.loginWithPublishPermissions (new string[] { "publish_actions" });
	}
	
	public void Like()
	{
		Facebook.instance.graphRequest(
			mobitekid+"/likes",
			Prime31.HTTPVerb.POST,
			( error, obj ) =>
			{
			if (obj != null)
			{
				Prime31.Utils.logObject( obj );
			}
			if( error != null )
			{
				ShowMessage("Error liking\n" + error);
				return;
			}
			ShowMessage("Liked");
		});
	}

	void ShowMessage(string message)
	{
		StartCoroutine (MainMenu.Instance.messagebox.Show (message));
	}

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
