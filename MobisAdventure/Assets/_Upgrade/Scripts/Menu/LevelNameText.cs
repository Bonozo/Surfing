using UnityEngine;
using System.Collections;

public class LevelNameText : MonoBehaviour {

	public string levelName;
	public string id;

	void OnEnable()
	{
		transform.FindChild ("Name").GetComponent<UILabel> ().text = levelName + " (level: " + currentLevel + ")";
	}

	public int currentLevel{
		get{
			var idd = "level_" + id;
			return PlayerPrefs.GetInt (idd, 1);
		}
	}
}
