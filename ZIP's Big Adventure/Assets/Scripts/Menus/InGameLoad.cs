using UnityEngine;
using System.Collections;

public class InGameLoad : MonoBehaviour {

	public void LoadGame()
	{
		Resources.UnloadUnusedAssets ();
		var gmb = Resources.Load<GameObject> (GameController.gameType.ToString ());
		gmb = Instantiate (gmb) as GameObject;
		gmb.transform.parent = transform;
		gmb.transform.localPosition = new Vector3 (0f, 0f, 0f);
		gmb.transform.localScale = new Vector3 (1f, 1f, 1f);
	}
}
