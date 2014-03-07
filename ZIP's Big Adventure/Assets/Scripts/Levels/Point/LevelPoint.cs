using UnityEngine;
using System.Collections;

public class LevelPoint : ZIPLevel {

	public GameBlock gameBlock;
	public EndItem[] endItems;
	public EndItem basicEndItem;
	public PointButton pointButton;
	
	public AudioClip clipCorrectAnswer;
	public AudioClip clipWrongAnswer;
	
	public override void StartGame ()
	{
		foreach(var et in endItems) et.Reset();
		basicEndItem.Reset ();
		pointButton.Reset ();

		basicEndItem.GetComponent<UISprite> ().color = new Color (1f, 1f, 1f, 0f);
		gameObject.SetActive (true);
	}
	
	public void Answered(PointButton point)
	{
		point.DisableCollider ();
		StartCoroutine(HappyEndThread());
	}
	
	private IEnumerator HappyEndThread()
	{
		AudioSource.PlayClipAtPoint(clipCorrectAnswer,transform.position);

		foreach(var et in endItems) et.Work();
		basicEndItem.Work ();

		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}
}
