﻿using UnityEngine;
using System.Collections;

public enum GameLevel
{
	First,
	Kindergarten,
	PreK,
	None
}

public enum GameType
{
	shapes,
	numbers,
	letters,
	patterns
}

public class GameController : MonoBehaviour {

	#region Settings

	public static GameLevel gameLevel = GameLevel.Kindergarten;
	public static GameType gameType = GameType.shapes;

	#endregion

	#region Toggles

	public float toggleTime = 1f;
	private bool toggleProcessing = false;
	public void Toggle(GameObject from,GameObject to)
	{
		if(!toggleProcessing)
		{
			toggleProcessing = true;
			StartCoroutine(RotateToggle(from,to));
		}
	}

	public void SimpleToggle(GameObject from,GameObject to)
	{
		to.SetActive(true);
		from.SetActive(false);
	}
	
	private IEnumerator RotateToggle(GameObject from,GameObject to)
	{
		float delta = 180f/toggleTime;

		yield return new WaitForEndOfFrame ();

		foreach(Transform col in from.transform)
			if(col.collider != null)
				col.collider.enabled = false;

		float timer = toggleTime*0.5f;
		while(timer>0f)
		{
			float t = Mathf.Min(Time.deltaTime,timer);
			timer-=Time.deltaTime;
			from.transform.Rotate(0f,-t*delta,0f);
			yield return null;
		}
		
		from.SetActive(false);
		to.transform.rotation = Quaternion.Euler(0f,90f,0f);
		to.SetActive(true);
		foreach(Transform col in to.transform)
			if(col.collider != null)
				col.collider.enabled = false;
		
		timer = toggleTime*0.5f;
		while(timer>0f)
		{
			float t = Mathf.Min(Time.deltaTime,timer);
			timer-=Time.deltaTime;
			to.transform.Rotate(0f,-t*delta,0f);
			yield return null;
		}
			
		yield return new WaitForEndOfFrame ();
		
		foreach(Transform col in to.transform)
			if(col.collider != null)
				col.collider.enabled = true;

		// Reset from parameters
		foreach(Transform col in from.transform)
			if(col.collider != null)
				col.collider.enabled = true;
		from.transform.rotation = Quaternion.Euler(0f,0f,0f);

		toggleProcessing = false;
	}
	
	#endregion

	#region Audio

	public AudioClip clipCorrectAnswer;
	public AudioClip clipWrongAnswer;

	public void PlayCorrectAnswer()
	{
		AudioSource.PlayClipAtPoint(clipCorrectAnswer,transform.position);
		AudioManager.Instance.PlayCorrectAnswer ();
	}

	public void PlayWrongAnswer()
	{
		AudioSource.PlayClipAtPoint(clipWrongAnswer,transform.position);
		AudioManager.Instance.PlayWrongAnswer ();
	}

	#endregion

	#region Static Instace
	
	//Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile GameController _staticInstance;
    public static GameController Instance {
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(GameController)) as GameController;
                    if (_staticInstance == null) {
                       Debug.LogError("The GameController instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion

	#region Update

	void Update()
	{
		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

	#endregion
	
}
