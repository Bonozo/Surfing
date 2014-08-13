using UnityEngine;
using System.Collections;

public class MenuToggle : MonoBehaviour {

	public bool inGame = false;

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
		if(!inGame)
			collider.enabled = !MainMenu.Instance.isPopupActive;
		else
			collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}

	private IEnumerator ToggleWithTweener(){
		tweener.PlayReverse ();

		float tm = tweener.duration;
		while(tm>0f){tm-=0.016f;yield return null;}
		//yield return new WaitForSeconds(tweener.duration);

		Toggle ();
	}

	private void Toggle(){
		foreach(GameObject g in objectsEnable)
			NGUITools.SetActive(g,true);
		foreach(GameObject g in objectsDisable)
			NGUITools.SetActive(g,false);
	}

}
