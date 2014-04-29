using UnityEngine;
using System.Collections;

public class LevelTraceThePath : ZIPTrace {
	
	public GameObject area;
	public Trace trace;

	public override void StartGame ()
	{
		trace.Reset();
		gameObject.SetActive (true);
		SendMessage ("StartLevel");
	}

	public override void Complete()
	{
		SendMessage ("EndLevel");
		StartCoroutine (HappyEnd ());
	}

	IEnumerator HappyEnd()
	{
		GameController.Instance.PlayCorrectAnswer ();
		iTween.ShakeRotation (area, new Vector3 (0f, 0f, 1.33f), 1f);
		yield return new WaitForSeconds(1f);
		gameBlock.path.OneStepGo();
		yield return new WaitForSeconds(0.5f);
		iTween.ShakeRotation (area, new Vector3 (0f, 0f, 1.11f), 1f);
		yield return new WaitForSeconds(4.5f);
		gameBlock.LevelComplete ();
	}
}
