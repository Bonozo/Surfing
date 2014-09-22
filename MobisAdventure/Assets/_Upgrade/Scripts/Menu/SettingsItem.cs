/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

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

	public string spriteEnabled;
	public string spriteDisabled;
	//private Color colGreen = Color.green;//new Color(0.75f,1f,0.75f,1f);
	//private Color colRed = Color.red;//new Color(1f,0.75f,0.75f,1f);
	bool savedValue;
	bool gotcha=false;
	
	void Awake()
	{
		sprite = gameObject.GetComponentInChildren<UISprite>();
		sprite.spriteName = status ? spriteEnabled : spriteDisabled;

		if (!PlayerPrefs.HasKey (optionName))
			status = defaultChecked;
	}
	
	void OnClick()
	{
		status=!status;
		sprite.spriteName = status ? spriteEnabled : spriteDisabled;
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
