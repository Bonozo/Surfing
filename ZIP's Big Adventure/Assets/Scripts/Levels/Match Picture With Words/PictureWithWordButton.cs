using UnityEngine;
using System.Collections;

public class PictureWithWordButton : MonoBehaviour {

	public LevelPictureWithWords level;

	public void Reset()
	{
		collider.enabled = true;
	}

	public void DisableCollider()
	{
		collider.enabled = false;
	}

	void OnClick()
	{
		level.Answered (this);
	}

}
