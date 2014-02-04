using UnityEngine;
using System.Collections;

public class LevelJumble : ZIPLevel {

	public GameBlock gameBlock;
	public JumbleDrag[] drags;
	public EndItem[] endItems;

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
		if( done == drags.Length)
			StartCoroutine(HappyEndThread());
	}

	private IEnumerator HappyEndThread()
	{
		foreach(var et in endItems) et.Work();

		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}
}
