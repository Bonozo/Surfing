using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {
	
	public tk2dButton buttonExit;
	
	void OnEnable ()
	{
		buttonExit.ButtonPressedEvent += HandleButtonExitButtonPressedEvent;
	}
	
	void OnDisable()
	{
		buttonExit.ButtonPressedEvent -= HandleButtonExitButtonPressedEvent;
	}

	void HandleButtonExitButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Title;
	}
	
}
