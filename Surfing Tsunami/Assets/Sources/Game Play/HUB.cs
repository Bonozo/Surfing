using UnityEngine;
using System.Collections;

public class HUB : MonoBehaviour {
	
	public tk2dButton buttonPause;
	public tk2dTextMesh distanceTravelled;
	public tk2dTextMesh coins;
	
	public GameObject[] live;
	
	public GameObject guiAnimTest;
	
	void OnEnable()
	{
		buttonPause.ButtonDownEvent += HandleButtonPauseButtonPressedEvent;
		foreach(GameObject v in live) v.SetActive(false);
		guiAnimTest.SetActive(Option.AnimTest);
	}
	
	void OnDisable()
	{
		buttonPause.ButtonDownEvent -= HandleButtonPauseButtonPressedEvent;
	}
	
	void HandleButtonPauseButtonPressedEvent (tk2dButton source)
	{
		switch(LevelInfo.State.state)
		{
		case GameState.Play:
			LevelInfo.State.state = GameState.Paused;
			break;
		case GameState.Paused:
			LevelInfo.State.state = GameState.Play;
			break;
		default:
			Debug.LogError("Pause button pressed in not Play or Paused mode");
			break;
		}
	}
	
	void Update()
	{
		distanceTravelled.text = ""+(int)LevelInfo.Environments.surfer.distanceTravelled + "M";
		distanceTravelled.Commit();
		
		coins.text = ""+LevelInfo.Environments.surfer.coins;
		coins.Commit();
		
		/*??*//*bad coding*/
		UpdateLives();

	}
	
	void UpdateLives()
	{
		int lives = LevelInfo.Environments.surfer.lives;
		for(int i=0;i<lives;i++) live[i].SetActive(true);
		for(int i=lives;i<live.Length;i++) live[i].SetActive(false);		
	}
	
}
