using UnityEngine;
using System.Collections;

public class VoiceLevel : MonoBehaviour {

	public AudioClip clipStart;
	public AudioClip clipCorrect1,clipCorrect2;
	public AudioClip clipWrong1,clipWrong2;

	void StartLevel()
	{
		if(clipStart != null)
			AudioManager.Instance.PlayClip(clipStart);
	}
}
