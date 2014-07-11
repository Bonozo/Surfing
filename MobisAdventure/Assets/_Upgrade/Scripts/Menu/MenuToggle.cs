using UnityEngine;
using System.Collections;

public class MenuToggle : MonoBehaviour {

	public GameObject[] objectsEnable;
	public GameObject[] objectsDisable;
	public UITweener tweener;

	public void OnClick()
	{
		if (tweener != null) {
			StartCoroutine(ToggleWithTweener());
			return;
		}
		Toggle ();
	}

	void Update()
	{
		collider.enabled = !MainMenu.Instance.isPopupActive;
	}

	private IEnumerator ToggleWithTweener(){
		tweener.PlayReverse ();
		yield return new WaitForSeconds(tweener.duration);
		Toggle ();
	}

	private void Toggle(){
		foreach(GameObject g in objectsEnable)
			NGUITools.SetActive(g,true);
		foreach(GameObject g in objectsDisable)
			NGUITools.SetActive(g,false);
	}
}
