using UnityEngine;
using System.Collections;

public class FirstTimePlayScreen : MonoBehaviour {
	
	void Awake(){
		var prefName = "firstTimePlayed";
		if( PlayerPrefs.GetInt(prefName,0)==0 ){
			NGUITools.SetActiveChildren(gameObject,true);
			PlayerPrefs.SetInt(prefName,1);
			PlayerPrefs.Save();
		}
	}
}
