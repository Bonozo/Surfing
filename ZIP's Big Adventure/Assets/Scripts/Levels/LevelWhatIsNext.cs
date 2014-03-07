using UnityEngine;
using System.Collections;

public class LevelWhatIsNext : ZIPLevel {
	public GameBlock gameBlock;
	public UISprite basicEndSprite;
	public WhatIsNextDrag[] items;
	public WhatIsNextDrag correctAnswer;
	public Transform answerPlace;
	public EndItem[] endItems;

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
			GameController.Instance.PlayWrongAnswer();
			item.Reset();
		}
	}

	private IEnumerator HappyEndThread()
	{
		GameController.Instance.PlayCorrectAnswer ();
		foreach(var it in items) it.DisableCollider();
		yield return new WaitForEndOfFrame ();

		iTween.MoveTo (correctAnswer.gameObject, answerPlace.transform.position, 2f);
		foreach(var et in endItems) et.Work();
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(0.5f);
		TweenAlpha.Begin (basicEndSprite.gameObject, 1f, 1f);
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
	}

	#region What Does not Belong Editor

	public int correctIndex;
	public string[] itemnames;
	public Transform answer;

	public void Initialize()
	{
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		correctAnswer = items [correctIndex-1];
		for(int i=0;i<4;i++)
		{
			SetSprite(items[i].GetComponent<UISprite>(),itemnames[i]);
			if(i==correctIndex-1)
			{
				if(items[i].GetComponent<EndItemMoveTo>() != null)
					DestroyImmediate(items[i].GetComponent<EndItemMoveTo>());
				if(items[i].GetComponent<EndItemColor>() == null)
					items[i].gameObject.AddComponent<EndItemColor>();
				items[i].GetComponent<EndItemColor>().duration = 0.5f;
				items[i].GetComponent<EndItemColor>().delay = 0.5f;
				items[i].GetComponent<EndItemColor>().to = new Color(1f,1f,1f,0f);

				answer.transform.localPosition = items[i].transform.localPosition;
			}
			else
			{
				if(items[i].GetComponent<EndItemColor>() != null)
					DestroyImmediate(items[i].GetComponent<EndItemColor>());
				if(items[i].GetComponent<EndItemMoveTo>() == null)
					items[i].gameObject.AddComponent<EndItemMoveTo>();
				items[i].GetComponent<EndItemMoveTo>().defaultUp = false;
				items[i].GetComponent<EndItemMoveTo>().to = items[i].transform.localPosition;
				items[i].GetComponent<EndItemMoveTo>().duration = 2f;
				items[i].GetComponent<EndItemMoveTo>().delay = 0.0f;
			}
			endItems[i] = items[i].GetComponent<EndItem>();
		}
	}

	void SetSprite(UISprite sprite,string sname)
	{
		sprite.spriteName = sname;
		sprite.type = UISprite.Type.Simple;
		sprite.MakePixelPerfect ();
		sprite.type = UISprite.Type.Sliced;
	}

	#endregion
}
