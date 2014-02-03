using UnityEngine;
using System.Collections;

public class SettingsItem : MonoBehaviour {

	public bool defaultChecked;
	/// <summary>
	/// The key of the option (Important)
	/// </summary>
	public string optionName;
	public UILabel label;
	private UISprite sprite;
	
	private Color colGreen = new Color(0.75f,1f,0.75f,1f);
	private Color colRed = new Color(1f,0.75f,0.75f,1f);
	bool savedValue;
	bool gotcha=false;
	
	void Awake()
	{
		sprite = gameObject.GetComponentInChildren<UISprite>();
		sprite.color = status?colGreen:colRed;
	}
	
	void OnClick()
	{
		status=!status;
		sprite.color = status?colGreen:colRed;
	}

	public bool status{
		get{
			if(!gotcha)
			{
				savedValue = PlayerPrefs.GetInt(optionName,defaultChecked?1:0)==1;
				gotcha = true;
			}
			return savedValue;
		}
		set{
			savedValue = value;		
			PlayerPrefs.SetInt(optionName,savedValue?1:0);
			PlayerPrefs.Save();
		}
	}
}
