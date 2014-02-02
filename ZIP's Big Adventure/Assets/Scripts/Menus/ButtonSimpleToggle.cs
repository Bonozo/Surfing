using UnityEngine;
using System.Collections;

public class ButtonSimpleToggle : MonoBehaviour {

	public GameObject from,to;
	
	void OnClick()
	{
		GameController.Instance.SimpleToggle(from,to);
	}
}
