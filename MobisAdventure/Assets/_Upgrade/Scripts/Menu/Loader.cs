using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public static string sceneName = "menu";
	public static bool destroyme = false;
	public UISlider sliderLoadingStatus;
	public GameObject background;
	
	// Use this for initialization
	IEnumerator Start () {
		Time.timeScale = 1f;
		System.GC.Collect();

		background.SetActive(true);
		sliderLoadingStatus.gameObject.SetActive(true);
		AsyncOperation opr = Application.LoadLevelAdditiveAsync(sceneName);
		while(!opr.isDone)
		{
			sliderLoadingStatus.value = opr.progress;
			yield return null;
		}

		background.SetActive(false);
		sliderLoadingStatus.gameObject.SetActive(false);
		
		// set-up default parameters
		var save_destroyme = destroyme;
		sceneName = "menu";
		destroyme = false;
		
		if(save_destroyme) Destroy(this.gameObject);
	}
	
	// Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile Loader _staticInstance;	
    public static Loader Instance 
	{
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(Loader)) as Loader;
                    if (_staticInstance == null) {
                       Debug.LogError("The Loader instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
		}
	}
       
}
