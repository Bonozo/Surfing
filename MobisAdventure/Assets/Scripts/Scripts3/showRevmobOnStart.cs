using UnityEngine;
using System.Collections;

public class showRevmobOnStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find("RevmobManager").SendMessage("showRevmobFullScreen",SendMessageOptions.DontRequireReceiver);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
