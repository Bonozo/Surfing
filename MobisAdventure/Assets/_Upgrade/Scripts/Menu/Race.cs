/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class Race : MonoBehaviour {

	public UILabel labelLevel;
	public UILabel labelTarget;
	public OptionMenu levelMenu;

	public UITweener[] tweeners;

	void OnEnable(){
		int levelIndex = PlayerPrefs.GetInt ("mobilevel", 1) - 1;
		labelLevel.text = GameManager.levelNames [levelIndex].ToUpper ();
		string levelprefname = "level_" + levelMenu.options [levelIndex].GetComponent<LevelNameText> ().id;
		var currentLevel = PlayerPrefs.GetInt (levelprefname, 1);
		labelTarget.text = "Target: Level " + currentLevel + " (" + LevelController.levelDist[currentLevel+1] + "m)";

		StartCoroutine (DoTweens ());
	}

	public float tweenTime = 0.5f;
	IEnumerator DoTweens(){
		foreach(var t in tweeners) t.Reset();
		yield return new WaitForSeconds(tweenTime);
		foreach(var t in tweeners) t.PlayForward();
	}
}
