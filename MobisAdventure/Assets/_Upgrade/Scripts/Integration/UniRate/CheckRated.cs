/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class CheckRated : MonoBehaviour {

	private string prefName = "mobiisrunisrated";

	private bool status;
	public bool RateButtonClicked{ get { return status; } }

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
		status = (PlayerPrefs.GetInt (prefName, 0) == 1);
		Debug.Log ("Rate Status: " + (status ? "Rated" : "Not rated yet"));
		UniRate.Instance.OnUserAttemptToRate += () => {
			status = true;
			PlayerPrefs.SetInt (prefName, 1);
			PlayerPrefs.Save ();
		};
	}

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile CheckRated _staticInstance;	
	public static CheckRated Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(CheckRated)) as CheckRated;
					if (_staticInstance == null) {
						Debug.LogError("The CheckRated instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
