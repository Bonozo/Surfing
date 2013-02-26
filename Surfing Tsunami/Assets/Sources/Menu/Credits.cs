using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	
	public GameObject text;
	public tk2dButton buttonExit;
	public float lenght = 300f;
	public float speed = 100f;
	
	private float currentHeight = 0f;
	
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.touchCount == 0 )
			currentHeight += Time.deltaTime*speed;
		
		if(currentHeight>lenght)
		{
			LevelInfo.State.state = GameState.Options;
		}
		
		var v = text.transform.position;
		v.y = currentHeight;
		text.transform.position = v;
	}
	
	void OnEnable()
	{
		currentHeight = 0;
		FingerGestures.OnDragMove += HandleFingerGesturesOnDragMove;
		buttonExit.ButtonPressedEvent += HandleButtonExitButtonPressedEvent;
	}
	
	void OnDisable()
	{
		FingerGestures.OnDragMove -= HandleFingerGesturesOnDragMove;
		buttonExit.ButtonPressedEvent -= HandleButtonExitButtonPressedEvent;
	}

	void HandleFingerGesturesOnDragMove (Vector2 fingerPos, Vector2 delta)
	{
		currentHeight += delta.y;
		currentHeight = Mathf.Clamp(currentHeight,0f,lenght);
	}
	
	void HandleButtonExitButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Options;	
	}
}
