using UnityEngine;
using System.Collections;

public class StoreMenuButton : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			LevelInfo.State.state = GameState.Title;
		}
	}
}
