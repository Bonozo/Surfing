using UnityEngine;
using System.Collections;

public class MusicLoop : MonoBehaviour {
	
	void Awake()
	{
		audio.playOnAwake = false;
		audio.loop = true;
	}

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		audio.Play ();
	}

	public void DropVolume(float delta,float timeDelta)
	{
		StartCoroutine (DropVolumeThread (delta, timeDelta));
	}

	private IEnumerator DropVolumeThread(float delta,float timeDelta)
	{
		float speed = delta / timeDelta;
		while(timeDelta > 0f)
		{
			float ddt = Time.deltaTime;
			timeDelta -= ddt;
			audio.volume = Mathf.Clamp(audio.volume - speed*ddt,0.05f,1f);
			yield return new WaitForEndOfFrame();
		}
	}

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile MusicLoop _staticInstance;	
	public static MusicLoop Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(MusicLoop)) as MusicLoop;
					if (_staticInstance == null) {
						Debug.LogError("The MusicLoop instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
