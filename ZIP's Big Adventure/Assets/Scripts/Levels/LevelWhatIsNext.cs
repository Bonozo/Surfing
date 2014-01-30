using UnityEngine;
using System.Collections;

public class LevelWhatIsNext : ZIPLevel {
	public GameBlock gameBlock;
	public UISprite basicEndSprite;
	public WhatIsNextDrag[] items;
	public WhatIsNextDrag correctAnswer;
	public Transform answerPlace;
	public EndItem[] endItems;

	public AudioClip clipCorrectAnswer;
	public AudioClip clipWrongAnswer;
	
	public override void StartGame ()
	{
		foreach(var it in items) it.Reset();
		foreach(var et in endItems) et.Reset();

		basicEndSprite.alpha = 0f;
		basicEndSprite.gameObject.SetActive (true);

		gameObject.SetActive (true);
	}

	public void Answered(WhatIsNextDrag item)
	{
		if(item == correctAnswer){
			StartCoroutine(HappyEndThread());
		}
		else{
			AudioSource.PlayClipAtPoint(clipWrongAnswer,transform.position);
			item.Reset();
		}
	}

	private IEnumerator HappyEndThread()
	{
		AudioSource.PlayClipAtPoint(clipCorrectAnswer,transform.position);
		foreach(var it in items) it.DisableCollider();

		iTween.MoveTo (correctAnswer.gameObject, answerPlace.transform.position, 2f);
		foreach(var et in endItems) et.Work();
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(0.5f);
		TweenAlpha.Begin (basicEndSprite.gameObject, 1f, 1f);
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}
}
