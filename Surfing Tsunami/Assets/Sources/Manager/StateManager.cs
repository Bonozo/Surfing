using UnityEngine;
using System.Collections;

public enum GameState
{
	Title,
	Play,
	Paused,
	WipeOut,
	Scoreboard,
	Options,
	Credits,
	Store,
	None
}

public enum WindState
{
	Left,
	Right,
	Down,
	DownLeft,
	DownRight
}

public enum Powerups
{
	FillLives,
	Invincibility
}

public class StateManager : MonoBehaviour {
	
	#region game Parameters
	
	public float WindTime = 30f;
	public float MaxForce = 30f;
	public float PlayerForce = 100f;
	
	#endregion
	
	#region In Game Used Parameters
	
	[System.NonSerializedAttribute]
	public WindState wind = WindState.Down;
	
	[System.NonSerializedAttribute]
	public Vector3 force = new Vector3(0f,0f,0f);
	
	[System.NonSerializedAttribute]
	public int Section = 0;
	
	public bool IsAnimatedState{
		get{
			return state == GameState.Play || state == GameState.WipeOut || state == GameState.Scoreboard;
		}
	}
	
	#endregion
	
	#region Private Field
	
	private float windtime = 0f;
	
	#endregion
	
	private GameState _state = GameState.None;
	public GameState state{
		get
		{
			return _state;
		}
		set
		{
			GameState last = _state;
			_state = value;
			
			LevelInfo.Environments.transformTitle.gameObject.SetActive(_state == GameState.Title);
			
			switch(_state)
			{
			case GameState.Title:
				RemoveSpawnedObjects();
				
				LevelInfo.Environments.transformSurfer.gameObject.SetActive(false);
				LevelInfo.Environments.transformPauseMenu.gameObject.SetActive(false);
				LevelInfo.Environments.transformTitle.gameObject.SetActive(true);
				LevelInfo.Environments.transformHUB.gameObject.SetActive(false);
				LevelInfo.Environments.transformScoreboard.gameObject.SetActive(false);
				LevelInfo.Environments.transformOptions.gameObject.SetActive(false);
				//LevelInfo.Environments.transformStore.gameObject.SetActive(false);
				
				break;
			case GameState.Play:
				LevelInfo.Environments.transformSurfer.gameObject.SetActive(true);
				LevelInfo.Environments.transformPauseMenu.gameObject.SetActive(false);
				LevelInfo.Environments.transformTitle.gameObject.SetActive(false);
				LevelInfo.Environments.transformHUB.gameObject.SetActive(true);
				LevelInfo.Environments.transformScoreboard.gameObject.SetActive(false);
				LevelInfo.Environments.transformOptions.gameObject.SetActive(false);
				
				break;
			case GameState.Paused:
				LevelInfo.Environments.transformSurfer.gameObject.SetActive(true);
				LevelInfo.Environments.transformPauseMenu.gameObject.SetActive(true);
				LevelInfo.Environments.transformTitle.gameObject.SetActive(false);
				LevelInfo.Environments.transformHUB.gameObject.SetActive(true);
				LevelInfo.Environments.transformOptions.gameObject.SetActive(false);
				break;
			case GameState.WipeOut:
				LevelInfo.Environments.surfer.WipeOut();
				break;
			case GameState.Scoreboard:
				LevelInfo.Environments.transformSurfer.gameObject.SetActive(true);
				LevelInfo.Environments.transformPauseMenu.gameObject.SetActive(false);
				LevelInfo.Environments.transformTitle.gameObject.SetActive(false);
				LevelInfo.Environments.transformHUB.gameObject.SetActive(true);
				LevelInfo.Environments.transformScoreboard.gameObject.SetActive(true);	
				LevelInfo.Environments.transformOptions.gameObject.SetActive(false);
				break;
			case GameState.Options:
				LevelInfo.Environments.transformSurfer.gameObject.SetActive(false);
				LevelInfo.Environments.transformPauseMenu.gameObject.SetActive(false);
				LevelInfo.Environments.transformTitle.gameObject.SetActive(false);
				LevelInfo.Environments.transformHUB.gameObject.SetActive(false);
				LevelInfo.Environments.transformScoreboard.gameObject.SetActive(false);
				LevelInfo.Environments.transformOptions.gameObject.SetActive(true);	
				LevelInfo.Environments.transformCredits.gameObject.SetActive(false);
				break;
			case GameState.Credits:
				LevelInfo.Environments.transformOptions.gameObject.SetActive(false);	
				LevelInfo.Environments.transformCredits.gameObject.SetActive(true);
				break;
			case GameState.Store:
				LevelInfo.Environments.transformSurfer.gameObject.SetActive(false);
				LevelInfo.Environments.transformPauseMenu.gameObject.SetActive(false);
				LevelInfo.Environments.transformTitle.gameObject.SetActive(false);
				LevelInfo.Environments.transformHUB.gameObject.SetActive(false);
				LevelInfo.Environments.transformScoreboard.gameObject.SetActive(false);
				LevelInfo.Environments.transformOptions.gameObject.SetActive(false);
				//LevelInfo.Environments.transformStore.gameObject.SetActive(true);
				break;
			}
			
			if( (last == GameState.Paused || last == GameState.Scoreboard || last == GameState.None) &&
				(state == GameState.Title || state == GameState.Store) )
			{
				LevelInfo.Audio.audioLoop.clip = LevelInfo.Audio.clipMenu;
				LevelInfo.Audio.audioLoop.Play();
				LevelInfo.Environments.windArrow.HideArrow();
				RemoveSpawnedObjects();
			}
			
			Store.Instance.ShowStore = state == GameState.Store;
			Time.timeScale = state==GameState.Paused?0.0f:1.0f;
		}
	}
	
	void RemoveSpawnedObjects()
	{
		foreach(Transform t in LevelInfo.Environments.transformObstacles) Destroy(t.gameObject);
		foreach(Transform t in LevelInfo.Environments.transformCrests) Destroy(t.gameObject);
	}
	
	public void StartNewGame()
	{
		Section = 1;
		
		LevelInfo.State.state = GameState.Play;
		RemoveSpawnedObjects();
		LevelInfo.Environments.surfer.Reset();
		StartWind(WindState.Down);
		LevelInfo.Audio.StopPlayer();
		LevelInfo.Audio.PlayGameLoop();
	}
	
	void Start()
	{
		state = GameState.Title;
	}
	
	void Update()
	{
		WindUpdates();
	}
	
	#region Wind
	
	void WindUpdates()
	{
		if(state != GameState.Play ) return;
		
		if( Input.GetKeyUp(KeyCode.L))
			StartWind(WindState.Left);
		if( Input.GetKeyUp(KeyCode.R))
			StartWind(WindState.Right);
		if( Input.GetKeyUp(KeyCode.D))
			StartWind(WindState.Down);
		if( Input.GetKeyUp(KeyCode.B) )
			StartWind(WindState.DownLeft);
		if( Input.GetKeyUp(KeyCode.N) )
			StartWind(WindState.DownRight);
		
		windtime-=Time.deltaTime;
		if(windtime<0f)
		{
			Section++;
			StartWind((WindState)Random.Range(0,5));
		}
		
		switch(wind)
		{
		case WindState.Down:
			force.y *= 1.05f;
			force.x *= 0.95f;
			break;
		case WindState.Left:
		case WindState.Right:
			force.x *= 1.05f; 
			force.y *= 0.95f; 
			break;
		case WindState.DownLeft:
		case WindState.DownRight:
			force.x *= 1.05f; 			
			force.y *= 1.05f; 
			break;
		}
		
		force.x = Mathf.Clamp(force.x,-MaxForce,MaxForce);
		force.y = Mathf.Clamp(force.y,-MaxForce,MaxForce);	
	}
	
	void StartWind(WindState state)
	{
		wind = state;
		LevelInfo.Environments.windArrow.ShowArrow(wind);
		windtime = WindTime;
		
		switch(wind)
		{
		case WindState.Down:
			if(force.y<1) force.y = 1f;
			break;
		case WindState.Left:
			if(force.x<1) force.x = 1f; 
			break;
		case WindState.Right:
			if(force.x>-1) force.x = -1f; 
			break;
		case WindState.DownLeft:
			if(force.y<1) force.y = 1f;
			if(force.x<1) force.x = 1f; 
			break;
		case WindState.DownRight:
			if(force.y<1) force.y = 1f;
			if(force.x>-1) force.x = -1f; 
			break;
		}
	}
	
	#endregion
}
