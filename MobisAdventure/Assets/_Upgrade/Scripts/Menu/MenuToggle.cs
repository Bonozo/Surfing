using UnityEngine;
using System.Collections;

public class MenuToggle : MonoBehaviour {

	public GameObject[] objectsEnable;
	public GameObject[] objectsDisable;

	public void OnClick()
	{
		foreach(GameObject g in objectsEnable)
			NGUITools.SetActive(g,true);
		foreach(GameObject g in objectsDisable)
			NGUITools.SetActive(g,false);
	}

	void Update()
	{
		collider.enabled = !MainMenu.Instance.isPopupActive;
	}
}
