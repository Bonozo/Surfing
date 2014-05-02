using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	#region General

	public AudioClip[] clipRightAnswer;
	public AudioClip[] clipWrongAnswer;
	public AudioClip[] clipLetter;

	public void PlayCorrectAnswer()
	{
		audio.clip = clipRightAnswer[Random.Range(0,clipRightAnswer.Length)];
		audio.Play ();
	}

	public void PlayCorrectAnswer(float delay)
	{
		StartCoroutine (PlayClipDelayed (clipRightAnswer [Random.Range (0, clipRightAnswer.Length)], delay));
	}


	public void PlayWrongAnswer()
	{
		audio.clip = clipWrongAnswer[Random.Range(0,clipWrongAnswer.Length)];
		audio.Play ();
	}

	public void PlayLetter(char c)
	{
		int index = (int)(char.ToLower (c) - 'a');
		audio.clip = clipLetter [index];
		audio.Play ();

	}

	public void PlayClip(AudioClip clip)
	{
		audio.clip = clip;
		audio.Play ();
	}

	public void Stop()
	{
		audio.Stop ();
	}

	IEnumerator PlayClipDelayed(AudioClip clip,float delay)
	{
		yield return new WaitForSeconds (delay);
		audio.clip = clip;
		audio.Play ();
	}

	#endregion
	
	#region Static Instace
	
	//Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile AudioManager _staticInstance;
	public static AudioManager Instance {
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(AudioManager)) as AudioManager;
					if (_staticInstance == null) {
						Debug.LogError("The AudioManager instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
