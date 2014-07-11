using UnityEngine;
using System.Collections;

public class LevelNameText : MonoBehaviour {

	public string levelName;
	public string id;

	void OnEnable()
	{
		
		var idd = "level_" + id;
		var currentLevel = PlayerPrefs.GetInt (idd, 1);

		transform.FindChild ("Name").GetComponent<UILabel> ().text = levelName + " (level: " + currentLevel + ")";
	}
}
