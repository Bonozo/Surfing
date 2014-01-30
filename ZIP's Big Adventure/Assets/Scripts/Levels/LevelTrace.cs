using UnityEngine;
using System.Collections;

public class LevelTrace : ZIPTrace {

	public GameBlock gameBlock;
	public GameObject guiGame;
	public GameObject guiHappyEnd;
	public Trace trace;
	public AudioClip clipCorrectAnswer;

	public override void StartGame ()
	{
		guiHappyEnd.SetActive(false);
		guiGame.SetActive(true);

		trace.Reset();

		guiGame.transform.localScale = new Vector3 (1f, 1f, 1f);
		gameObject.SetActive(true);
	}

	public override void Complete()
	{
		StartCoroutine(HappyEndThread());
	}

	private IEnumerator HappyEndThread()
	{
		AudioSource.PlayClipAtPoint(clipCorrectAnswer,transform.position);
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo();
		yield return new WaitForSeconds(0.5f);

		var ct = 0.15f;

		var gsc = guiGame.transform.localScale;
		iTween.ScaleTo (guiGame, Vector3.zero, ct);
		yield return new WaitForSeconds (ct);
		guiGame.SetActive(false);

		var hsc = guiHappyEnd.transform.localScale;
		guiHappyEnd.transform.localScale = Vector3.zero;
		guiHappyEnd.SetActive (true);
		iTween.ScaleTo (guiHappyEnd, hsc, ct);
		yield return new WaitForSeconds (ct);

		iTween.Stop (guiGame);
		iTween.Stop (guiHappyEnd);

		guiGame.transform.localScale = gsc;
		guiHappyEnd.transform.localScale = hsc;

		yield return new WaitForSeconds(2f);
		yield return new WaitForSeconds(3f);
		gameBlock.LevelComplete();
	}

}
