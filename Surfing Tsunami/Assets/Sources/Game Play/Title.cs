using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
	
	public tk2dButton buttonExit;
	public tk2dButton buttonPlay;
	public tk2dButton buttonOption;
	public tk2dButton buttonStore;
	
	void OnEnable()
	{
		buttonPlay.ButtonPressedEvent += HandleButtonPlayButtonPressedEvent;
		buttonOption.ButtonPressedEvent += HandleButtonOptionButtonPressedEvent;
		buttonExit.ButtonPressedEvent += HandleButtonExitButtonPressedEvent;
		buttonStore.ButtonPressedEvent += HandleButtonStoreButtonPressedEvent;
	}
	
	void OnDisable()
	{
		buttonPlay.ButtonPressedEvent -= HandleButtonPlayButtonPressedEvent;
		buttonOption.ButtonPressedEvent -= HandleButtonOptionButtonPressedEvent;
		buttonExit.ButtonPressedEvent -= HandleButtonExitButtonPressedEvent;
		buttonStore.ButtonPressedEvent -= HandleButtonStoreButtonPressedEvent;
	}
	
	void HandleButtonPlayButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Play;
		LevelInfo.State.StartNewGame();		
	}

	void HandleButtonOptionButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Options;
	}
	
	void HandleButtonExitButtonPressedEvent (tk2dButton source)
	{
		Application.Quit();
	}
	
	void HandleButtonStoreButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Store;
	}
}
