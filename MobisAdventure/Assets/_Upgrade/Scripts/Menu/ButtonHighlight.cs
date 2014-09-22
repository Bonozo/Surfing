/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class ButtonHighlight : MonoBehaviour {

	public float pauseTime = 0.5f;
	UITweener tweener;

	void Awake(){
		tweener = GetComponent<UITweener> ();
	}

	void OnEnable(){
		StartCoroutine (Highlight ());
	}

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Space))
		{
			tweener.PlayForward();
			Debug.Log(tweener.gameObject.name);
		}
	}


	IEnumerator Highlight(){
		while(true){
			yield return new WaitForSeconds (pauseTime);
			tweener.PlayForward ();
			yield return new WaitForSeconds(tweener.duration);
			tweener.PlayReverse ();
			yield return new WaitForSeconds(tweener.duration);
		}
	}
}
