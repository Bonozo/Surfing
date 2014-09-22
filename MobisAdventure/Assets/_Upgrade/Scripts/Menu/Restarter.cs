/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class Restarter : MonoBehaviour {

	public GameObject level;
	private GameObject old;

	void Awake()
	{
		if(GameObject.FindObjectOfType<PlayerController>() == null)
			CreateLevel ();
		else
			old = GameObject.FindObjectOfType<PlayerController>().gameObject;
	}

	void Update () 
	{
		if( Input.GetKeyDown(KeyCode.R))
		{
			CreateLevel();
		}
	}

	public void CreateLevel()
	{
		if(old != null)
			Destroy (old);
		old = Instantiate (level) as GameObject;

		// Increase camera size on wide screen devices
		old.GetComponentInChildren<Camera> ().orthographicSize = FixIpadHeight.isWideScreen ? 13 : 10;
		Time.timeScale = 1f;
	}

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile Restarter _staticInstance;	
	public static Restarter Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(Restarter)) as Restarter;
					if (_staticInstance == null) {
						Debug.LogError("The Restarter instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
