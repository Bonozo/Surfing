using UnityEngine;
using System.Collections;

public class LevelWhatIsNext : ZIPLevel {
	
	public UISprite basicEndSprite;
	public WhatIsNextDrag[] items;
	public WhatIsNextDrag correctAnswer;
	public Transform answerPlace;
	public EndItem[] endItems;
	public float endTime = 2f;

	public override void StartGame ()
	{
		foreach(var it in items) it.Reset();
		foreach(var et in endItems) et.Reset();

		basicEndSprite.alpha = 0f;
		basicEndSprite.gameObject.SetActive (true);

		gameObject.SetActive (true);
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
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
		SendMessage ("PlayFinish", true, SendMessageOptions.DontRequireReceiver);
		GameController.Instance.PlayCorrectAnswer ();
		foreach(var it in items) it.DisableCollider();
		yield return new WaitForEndOfFrame ();

		iTween.MoveTo (correctAnswer.gameObject, answerPlace.transform.position, endTime);
		foreach(var et in endItems) et.Work();
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(0.5f);
		TweenAlpha.Begin (basicEndSprite.gameObject, 1f, 1f);
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
	}

	#region What Does not Belong Editor

	/*public int correctIndex;
	public string[] itemnames;
	public Transform answer;

	public void Initialize()
	{
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		gameBlock.level [0] = this;


		correctAnswer = items [correctIndex-1];
		for(int i=0;i<itemnames.Length;i++)
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
				items[i].GetComponent<EndItemMoveTo>().resetBefore = false;
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
	}*/

	#endregion

	#region What Is Next

	public GameObject Center;
	public UIAtlas atlas;

	public void Initialize()
	{
		/*for(int i=0;i<Center.transform.childCount;i++)
			Center.transform.GetChild(i).GetComponent<UISprite>().atlas = atlas;
		Center.transform.parent.FindChild("Title").GetComponent<UISprite>().atlas = atlas;
		gameBlock = transform.parent.parent.parent.parent.GetComponent<GameBlock> ();
		return;*/


		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		gameBlock.level [0] = this;

		Vector3 pos;
		if(Center.transform.FindChild("Basic") != null)
			pos = Center.transform.FindChild("Basic").transform.localPosition;
		else
			pos = Center.transform.FindChild("BasicEnd").transform.localPosition;
		pos.x = -pos.x; pos.y=0; pos.z = 0;
		Center.GetComponent<EndItemMoveTo> ().to = pos;

		//if(Center.transform.parent.FindChild("_begin") != null)
		//	DestroyImmediate(Center.transform.parent.FindChild("_begin").gameObject);
	}

	#endregion
}
