/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class MobiAssets : MonoBehaviour {

	public AudioClip clipMonsterFootstep;
	public AudioClip clipBoostPowerup;
	public AudioClip clipBoostUsage;

	public AudioClip clipImpactCrash;
	public AudioClip clipBoneCrash;

	public GameObject AudioFSM;

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile MobiAssets _staticInstance;	
	public static MobiAssets Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(MobiAssets)) as MobiAssets;
					if (_staticInstance == null) {
						Debug.LogError("The MobiAssets instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
