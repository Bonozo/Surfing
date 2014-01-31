using UnityEngine;
using System.Collections;

public class UpgradeMenu : MonoBehaviour {

	public OptionMenu sleds;
	public SledUpgrade[] sledUpgrade;
	public GameObject[] sledSprite;
	public Transform sledSpriteTransform;

	void OnEnable(){
		Destroy(sledSpriteTransform.GetChild(0).gameObject);
		var sd = Instantiate(sledSprite[sleds.SelectedIndex]) as GameObject;
		sd.transform.parent = sledSpriteTransform;
		sd.transform.localPosition = Vector3.zero;
		sd.transform.localScale = new Vector3(1,1,1);
		Init();
	}

	public string CurrentSledName{
		get{
			return sleds.SelectedName;
		}
	}

	public void Init()
	{
		foreach(SledUpgrade s in sledUpgrade)
			s.Init();
	}
}
