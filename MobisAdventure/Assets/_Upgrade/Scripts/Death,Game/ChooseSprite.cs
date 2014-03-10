using UnityEngine;
using System.Collections;

public class ChooseSprite : MonoBehaviour {

	public string androidSprite;
	public string iPhoneSprite;

	void Awake()
	{
		#if UNITY_ANDROID
		GetComponent<UISprite>().spriteName = androidSprite;
		#endif
		#if UNITY_IPHONE
		GetComponent<UISprite>().spriteName = iPhoneSprite;
		#endif
	}
}
