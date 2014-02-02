using UnityEngine;
using System.Collections;

public class PointButton : MonoBehaviour {

	public LevelPoint level;

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
