using UnityEngine;
using System.Collections;

public class VoiceLevel : MonoBehaviour {

	public AudioClip clipStart;
	public AudioClip[] clipCorrect;
	public AudioClip[] clipWrong;

	void PlayStart()
	{
		if(clipStart != null)
			AudioManager.Instance.PlayClip(clipStart);
	}

	void PlayAnswer(bool correct)
	{
		if(correct)
		{
			if(clipCorrect.Length == 0)
				AudioManager.Instance.PlayCorrectAnswer();
			else
				AudioManager.Instance.PlayClip(clipCorrect[Random.Range(0,clipCorrect.Length)]);
		}
		else
		{
			if(clipWrong.Length == 0)
				AudioManager.Instance.PlayWrongAnswer();
			else
				AudioManager.Instance.PlayClip(clipWrong[Random.Range(0,clipWrong.Length)]);
		}
	}
}
