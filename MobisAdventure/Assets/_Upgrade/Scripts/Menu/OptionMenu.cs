using UnityEngine;
using System.Collections;

public class OptionMenu : MonoBehaviour {

	public OptionItem[] options;
	public string optionName;
	
	void Start()
	{
		int saved = PlayerPrefs.GetInt(optionName,1);
		Select(options[saved-1]);
	}

	private int selected=-1;
	public void Select(OptionItem item)
	{
		selected=0; while(options[selected]!=item) selected++;
		PlayerPrefs.SetInt(optionName,selected+1);
		foreach(OptionItem opt in options)
			opt.SetSelected(opt==item);
	}

	public string SelectedName{
		get{
			if(selected==-1) selected = PlayerPrefs.GetInt(optionName,1)-1;
			return options[selected].itemName;
		}
	}

	public int SelectedIndex{
		get{
			if(selected==-1) selected = PlayerPrefs.GetInt(optionName,1)-1;
			return selected;
		}
	}

	public OptionItem SelectedItem{
		get{
			if(selected==-1) selected = PlayerPrefs.GetInt(optionName,1)-1;
			return options[selected];
		}
	}
}
