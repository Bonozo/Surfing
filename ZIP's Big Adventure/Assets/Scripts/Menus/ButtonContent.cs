using UnityEngine;
using System.Collections;

public class ButtonContent : MonoBehaviour {

	public GameType gameType;
	public AudioClip clip;
	public GameObject dancingCharacter;
	private Color grayOutColor = new Color(0.7f,0.7f,0.7f,1f);

	void Awake()
	{
		// Delete the next 4 lines and me
		//PlayerPrefs.SetInt ("numbers", 1);
		//PlayerPrefs.SetInt ("shapes", 1);
		//PlayerPrefs.SetInt ("letters", 1);
		//PlayerPrefs.SetInt ("patterns", 1);

		var button = GetComponent<UIButton> ();
		button.disabledColor = grayOutColor;
		if (PlayerPrefs.HasKey (gameType.ToString())){
			button.isEnabled = false;
			transform.FindChild("Play").gameObject.SetActive(false);
			transform.FindChild("Back").gameObject.SetActive(true);
			dancingCharacter.SetActive(true);
		} else{
			dancingCharacter.SetActive(false);
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
