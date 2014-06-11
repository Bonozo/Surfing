using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public static string sceneName = "title";
		
	IEnumerator Start()
	{
		// Removing all unused resources
		System.GC.Collect ();

		AsyncOperation unloading = Resources.UnloadUnusedAssets ();
		yield return unloading;
		
		System.GC.Collect ();

		// Loading new level
		var opr = Application.LoadLevelAsync (sceneName);
		yield return opr;

		Destroy (this.gameObject);
	}
}
