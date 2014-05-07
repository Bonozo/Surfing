using UnityEngine;
using System.Collections;

public class showRevmobOnStart : MonoBehaviour {
	
	void Start () 
	{
		#if !UNITY_EDITOR
		if(PlayerPrefs.GetInt("RevmobStatus",0) == 0)
			GameObject.Find("RevmobManager").SendMessage("showRevmobFullScreen",SendMessageOptions.DontRequireReceiver);
		#endif
	}
}
