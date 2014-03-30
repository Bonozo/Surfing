using UnityEngine;
using System.Collections;

public class LevelTrace : ZIPTrace {
	
	public Trace trace;
	public EndItem[] endItems;

	public override void StartGame ()
	{
		trace.Reset();
		foreach(var et in endItems) et.Reset();
		gameObject.SetActive(true);
	}

	public override void Complete()
	{
		StartCoroutine(HappyEndThread());
	}

	private IEnumerator HappyEndThread()
	{
		
		GameController.Instance.PlayCorrectAnswer ();
		
		foreach(var et in endItems) et.Work();
		
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
	}

}
