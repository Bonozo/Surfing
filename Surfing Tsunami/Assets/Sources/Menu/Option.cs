using UnityEngine;
using System.Collections;

public class Option : MonoBehaviour {
	
	#region Options
	private static float _volume = 1f;
	public static float volume{
		get{
			return _volume;
		}
		set{
			_volume = value;
			PlayerPrefs.SetFloat("options_volume",_volume);
			AudioListener.volume = _volume;
		}
	}
	private static bool _tiltMove = true;
	public static bool tiltMove{
		get{
			return _tiltMove;
		}
		set{
			_tiltMove = value;
			PlayerPrefs.SetInt("options_tilt_move",_tiltMove?1:0);
		}
	}
	
	public static void RestoreOptions()
	{
		volume = PlayerPrefs.GetFloat("options_volume",1f);
		_tiltMove = PlayerPrefs.GetInt("options_tilt_move",1)==1;
	}

	#endregion
	
	#region Debug Options
	public static bool ShowFPS = false;
	public static bool UnlimitedHealth = false;
	public static bool AnimTest = false;
	#endregion

	public tk2dButton buttonExit;
	public tk2dTextMesh title;
	public GameObject guiFPS;
	private bool debug=false;
	public bool showdebug=false;
	
	// Use this for initialization
	void OnEnable ()
	{
		buttonExit.ButtonPressedEvent += HandleButtonExitButtonPressedEvent;
		FingerGestures.OnSwipe += HandleFingerGesturesOnSwipe;
		
		debug = false;
		//showdebug = false;
	}
	
	void OnDisable()
	{
		buttonExit.ButtonPressedEvent -= HandleButtonExitButtonPressedEvent;
		FingerGestures.OnSwipe -= HandleFingerGesturesOnSwipe;	
	}
	
	
	void HandleFingerGesturesOnSwipe (Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		if(direction == FingerGestures.SwipeDirection.Left && velocity > 1000f)	
			showdebug = true;
	}

	void HandleButtonExitButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Title;
	}
	
	private Rect textRect(float index)
	{
		index++;
		return new Rect(Screen.width*0.05f,index*0.085f*Screen.height,Screen.width*0.4f,Screen.height*0.07f);
	}
	private Rect buttonRect(float index)
	{
		index++;
		return new Rect(Screen.width*0.5f,index*0.085f*Screen.height,Screen.width*0.4f,Screen.height*0.07f);
	}
	
	void OnGUI()
	{
		if( debug )
		{
			GUI.Label(textRect(1),"Show Framerate");
			if( GUI.Button(buttonRect(1),ShowFPS?"ON":"OFF") )
			{
				ShowFPS = !ShowFPS;
				guiFPS.SetActive(ShowFPS);
			}
			
			GUI.Label(textRect(2),"Invincibility");
			if( GUI.Button(buttonRect(2),UnlimitedHealth?"ON":"OFF") )
				UnlimitedHealth = !UnlimitedHealth;
			
			GUI.Label(textRect(3),"Anim Test");
			if( GUI.Button(buttonRect(3),AnimTest?"ON":"OFF") )
				AnimTest = !AnimTest;
			
			if( GUI.Button( new Rect(Screen.width-200,Screen.height-60,80,40),"Options"))
			{
				debug = false;
				title.text = "OPTIONS";
				title.Commit();
			}
		}
		
		else
		{
			GUI.Label(textRect(1),"Volume");
			volume = GUI.HorizontalSlider(buttonRect(1),volume,0f,1f);
			
			GUI.Label(textRect(2),"Tilt Move");
			if( GUI.Button(buttonRect(2),tiltMove?"ON":"OFF") )
				tiltMove = !tiltMove;
	
			GUI.Label(textRect(3),"Calibrate Vertical (" + AccelerationWithCalibrate.CurrentCalibrationPositive + ")");
			if( GUI.Button(buttonRect(3),"Calibrate (" + AccelerationWithCalibrate.DeviceAnglePositive + ")") )
				AccelerationWithCalibrate.Calibrate();
			
			if(showdebug && GUI.Button( new Rect(Screen.width-200,Screen.height-60,80,40),"Debug"))
			{
				debug = true;
				title.text = "DEBUG";
				title.Commit();
			}
		}
		
		if(GUI.Button( new Rect(Screen.width-100,Screen.height-60,80,40),"Credits"))
		{
			LevelInfo.State.state = GameState.Credits;	
		}
		

			
	}
}
