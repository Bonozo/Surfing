using UnityEngine;
using System.Collections;

public class ButtonContent : MonoBehaviour {

	public GameType gameType;
	public AudioClip clip;
	private Color grayOutColor = new Color(0.2f,0.2f,0.2f,1f);

	void Awake()
	{
		var button = GetComponent<UIButton> ();
		button.disabledColor = grayOutColor;
		if (PlayerPrefs.HasKey (gameType.ToString())){
			button.isEnabled = false;
			transform.FindChild("Play").gameObject.SetActive(false);
		}
	}

	void OnClick()
	{
		var buttons = GameObject.FindObjectsOfType<ButtonContent> ();
		foreach(var bt in buttons)
			bt.collider.enabled = false;
		var backbutton = GameObject.FindObjectOfType<MenuBackButton> ();
		backbutton.collider.enabled = false;
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
