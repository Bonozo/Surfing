using UnityEngine;
using System.Collections;

public class VoiceTaskLevel : MonoBehaviour {

	public AudioClip clipStart,clipFinish;
	public float delayStart=0f,delayFinish=0f;

	void Awake()
	{
		if(audio == null)
			gameObject.AddComponent<AudioSource>();
	}

	void StartLevel()
	{
		audio.Stop ();
		if(clipStart != null)
			StartCoroutine (PlayClipDelayed (clipStart, delayStart));
	}

	void EndLevel()
	{
		audio.Stop ();
		if(clipFinish != null)
			StartCoroutine (PlayClipDelayed (clipFinish, delayFinish));
	}

	IEnumerator PlayClipDelayed(AudioClip clip,float delay)
	{
		yield return new WaitForSeconds (delay);
		audio.clip = clip;
		audio.Play ();
	}
}
