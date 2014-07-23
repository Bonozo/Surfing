using UnityEngine;
using System.Collections;

public class ButtonToggle : MonoBehaviour {

	public enum ToggleType{
		Rotate,
		Simple
	}

	public ToggleType toggleType = ToggleType.Rotate;
	public GameObject from,to;
	
	protected virtual void OnClick()
	{
		switch (toggleType) 
		{
		case ToggleType.Rotate:
			GameController.Instance.Toggle(from,to);
			break;
		case ToggleType.Simple:
			GameController.Instance.SimpleToggle(from,to);
			break;
		}
	}
}
