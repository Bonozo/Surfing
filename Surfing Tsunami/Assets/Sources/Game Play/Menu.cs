using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
	public tk2dButton buttonContinue;
	public tk2dButton buttonRestart;
	public tk2dButton buttonMute;
	public tk2dButton buttonExit;
	
	private bool mute = false;
	
	void OnEnable()
	{
		buttonContinue.ButtonPressedEvent += HandleButtonContinueButtonPressedEvent;
		buttonRestart.ButtonPressedEvent += HandleButtonRestartButtonPressedEvent;
		buttonMute.ButtonPressedEvent += HandleButtonMuteButtonPressedEvent;
		buttonExit.ButtonPressedEvent += HandleButtonExitButtonPressedEvent;
	}
	
	void OnDisable()
	{
		buttonContinue.ButtonPressedEvent -= HandleButtonContinueButtonPressedEvent;
		buttonRestart.ButtonPressedEvent -= HandleButtonRestartButtonPressedEvent;
		buttonMute.ButtonPressedEvent -= HandleButtonMuteButtonPressedEvent;
		buttonExit.ButtonPressedEvent -= HandleButtonExitButtonPressedEvent;		
	}

	void HandleButtonExitButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Title;
	}

	void HandleButtonMuteButtonPressedEvent (tk2dButton source)
	{
		mute = !mute;
		AudioListener.volume = mute?0.0f:1.0f;
		
	}

	void HandleButtonRestartButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.StartNewGame();
	}

	void HandleButtonContinueButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Play;
	}
}
