using UnityEngine;
using System.Collections;

public class MenuTween : MonoBehaviour {

	void OnEnable(){
		var tweener = GetComponent<UITweener> ();
		tweener.PlayForward ();
	}
}
