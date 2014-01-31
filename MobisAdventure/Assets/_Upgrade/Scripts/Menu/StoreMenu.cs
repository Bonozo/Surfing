using UnityEngine;
using System.Collections;

public class StoreMenu : MonoBehaviour {

	// Copied logic from the OptinosMenu.cs (bad idea but fast)
	public StoreItem[] options;
	public string optionName;
	
	void Start()
	{
		int saved = PlayerPrefs.GetInt(optionName,1);
		Select(options[saved-1]);
	}
	
	private int selected=-1;
	public void Select(StoreItem item)
	{
		selected=0; while(options[selected]!=item) selected++;
		PlayerPrefs.SetInt(optionName,selected+1);
		foreach(StoreItem opt in options)
			opt.SetSelected(opt==item);
	}
	
	public int SelectedIndex{
		get{
			if(selected==-1) selected = PlayerPrefs.GetInt(optionName,1)-1;
			return selected;
		}
	}
}
