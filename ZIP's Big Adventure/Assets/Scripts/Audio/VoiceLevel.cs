using UnityEngine;
using System.Collections;

public class VoiceLevel : MonoBehaviour {

	public AudioClip clipStart;
	public AudioClip clipEnd;
	public float delayFinish;

	void PlayStart()
	{
		if(clipStart != null)
			AudioManager.Instance.PlayClip(clipStart);
	}

	void PlayAudio(string value)
	{
		Debug.Log ("event received: " + value);
		if(value.Length == 1 && char.IsLetter(value[0]))
			AudioManager.Instance.PlayLetter(value[0]);
	}
	
	void PlayEnd(bool correct)
	{
		if(!correct) return;
		//audio.Stop ();
		if(clipEnd != null)
			StartCoroutine (PlayClipDelayed (clipEnd, delayFinish));
	}
	
	IEnumerator PlayClipDelayed(AudioClip clip,float delay)
	{
		yield return new WaitForSeconds (delay);
		//audio.clip = clip;
		//audio.Play ();
		AudioManager.Instance.PlayClip (clip);
	}
}
