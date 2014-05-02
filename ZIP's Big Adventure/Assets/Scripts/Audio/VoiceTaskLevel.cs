using UnityEngine;
using System.Collections;

public class VoiceTaskLevel : MonoBehaviour {

	#region Start
	public AudioClip clipStart;

	void PlayStart()
	{
		AudioManager.Instance.Stop ();
		if(clipStart != null)
			AudioManager.Instance.PlayClip (clipStart);
	}

	#endregion

	#region Part
	
	public bool playPartMessage;

	void PlayPart(string value)
	{
		if(playPartMessage)
		{
			// letter
			if(value.Length == 1 && char.IsLetter(value[0]))
				AudioManager.Instance.PlayLetter(value[0]);
		}
	}

	#endregion

	#region End

	public AudioClip clipEnd;
	public float delayEnd;
	public bool randomEnd;

	void PlayEnd()
	{
		if(randomEnd)
			AudioManager.Instance.PlayCorrectAnswer(delayEnd);
		
		else if(clipEnd != null)
			StartCoroutine (PlayClipDelayed (clipEnd, delayEnd));
	}

	#endregion

	#region Finish

	// finish
	public AudioClip clipFinish;
	public float delayFinish;
	public bool randomFinish;

	void PlayFinish()
	{
		if(randomFinish)
			AudioManager.Instance.PlayCorrectAnswer(delayFinish);
		
		else if(clipFinish != null)
			StartCoroutine (PlayClipDelayed (clipFinish, delayFinish));
	}

	#endregion



	IEnumerator PlayClipDelayed(AudioClip clip,float delay)
	{
		yield return new WaitForSeconds (delay);
		AudioManager.Instance.PlayClip (clip);
	}
}
