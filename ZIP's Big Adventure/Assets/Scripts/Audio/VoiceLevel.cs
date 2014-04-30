using UnityEngine;
using System.Collections;

public class VoiceLevel : MonoBehaviour {

	public AudioClip clipStart;

	void PlayStart()
	{
		if(clipStart != null)
			AudioManager.Instance.PlayClip(clipStart);
	}
}
