﻿using UnityEngine;
using System.Collections;

public class LevelTraceThePath : ZIPTrace {
	
	public GameObject shakeTransform;
	public Trace trace;

	public override void StartGame ()
	{
		trace.Reset();
		gameObject.SetActive (true);
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
	}

	public override void Complete()
	{
		SendMessage ("PlayFinish", true, SendMessageOptions.DontRequireReceiver);
		StartCoroutine (HappyEnd ());
	}

	IEnumerator HappyEnd()
	{
		GameController.Instance.PlayCorrectAnswer ();
		if(shakeTransform != null)
			iTween.ShakeRotation (shakeTransform, new Vector3 (0f, 0f, 1.33f), 1f);
		yield return new WaitForSeconds(1f);
		gameBlock.path.OneStepGo();
		yield return new WaitForSeconds(0.5f);
		if(shakeTransform != null)
			iTween.ShakeRotation (shakeTransform, new Vector3 (0f, 0f, 1.11f), 1f);
		yield return new WaitForSeconds(4.5f);
		gameBlock.LevelComplete ();
	}
}
