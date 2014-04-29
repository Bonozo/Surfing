using UnityEngine;
using System.Collections;

public class ButtonContent : MonoBehaviour {

	public GameType gameType;
	public AudioClip clip;

	void Awake()
	{
		if (PlayerPrefs.HasKey (gameType.ToString()))
			this.GetComponent<UIButton> ().isEnabled = false;
	}

	void OnClick()
	{
		var buttons = GameObject.FindObjectsOfType<ButtonContent> ();
		foreach(var bt in buttons)
			bt.collider.enabled = false;
		StartCoroutine (StartGame ());
	}
	IEnumerator StartGame()
	{
		AudioManager.Instance.PlayClip (clip);
		while(AudioManager.Instance.audio.isPlaying) yield return null;
		
		GameController.gameType = gameType;
		Loader.sceneName = gameType.ToString();
		Application.LoadLevel ("loader");
	}
}
