using UnityEngine;
using System.Collections;

public class LevelJumble : ZIPLevel {

	public GameBlock gameBlock;
	public JumbleDrag[] drags;
	public EndItem[] endItems;
	public int dragsLenght;

	private int done;

	public override void StartGame ()
	{
		foreach(var dr in drags) dr.Reset();
		foreach(var et in endItems) et.Reset();
		done = 0;

		gameObject.SetActive (true);
	}

	public void Answered(JumbleDrag drag)
	{
		done++;
		drag.GetComponent<UISprite> ().depth--;
		if( done == dragsLenght)
			StartCoroutine(HappyEndThread());
	}

	private IEnumerator HappyEndThread()
	{
		foreach(var et in endItems) et.Work();
		foreach(var dr in drags) dr.DisableCollider();

		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}
}
