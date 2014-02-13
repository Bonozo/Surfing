using UnityEngine;
using System.Collections;

public class TitleButton : MonoBehaviour {
	
	public GameLevel gameLevel;
	public GameObject from,to;
	void OnClick()
	{
		GameController.gameLevel = gameLevel;
		GameController.Instance.SimpleToggle (from, to);
	}
}
