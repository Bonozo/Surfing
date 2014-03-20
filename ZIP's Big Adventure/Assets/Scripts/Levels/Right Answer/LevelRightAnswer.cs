﻿using UnityEngine;
using System.Collections;

public class LevelRightAnswer : ZIPLevel {

	public GameBlock gameBlock;
	public LevelRightAnswerButton[] buttons;
	public LevelRightAnswerButton correctAnswer;
	public EndItem[] endItems;
	
	public override void StartGame ()
	{
		foreach(var btr in buttons) btr.Reset();
		foreach(var et in endItems) et.Reset();
		gameObject.SetActive (true);
	}
	
	public void Answered(LevelRightAnswerButton point)
	{
		if(point == correctAnswer)
		{
			StartCoroutine(HappyEndThread());
		}
		else
		{
			GameController.Instance.PlayWrongAnswer();
		}
	}
	
	private IEnumerator HappyEndThread()
	{
		GameController.Instance.PlayCorrectAnswer();
		
		foreach(var btr in buttons) btr.DisableCollider();
		foreach(var et in endItems) et.Work();
		
		
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
	}

	#region Editor

	//public string answer1, answer2, answer3;
	public int rightanswrindex;
	public int howmany;
	public UISprite ans1, ans2, rightans;
	public UISprite[] hms;
	public EndItemList shapes;
	public GameObject shapeX;
	
	public void Initialize()
	{
		if(transform.FindChild ("Game").FindChild ("_begin") != null)
			DestroyImmediate(transform.FindChild ("Game").FindChild ("_begin").gameObject);
		//ans1.GetComponent<BoxCollider> ().size = new Vector3 (2f, 1.5f, 1f);
		//ans2.GetComponent<BoxCollider> ().size = new Vector3 (2f, 1.5f, 1f);
		//rightans.GetComponent<BoxCollider> ().size = new Vector3 (2f, 1.5f, 1f);

		transform.localPosition = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (1, 1, 1);

		Vector3 cc1 = new Vector3 (0, -390, 0);
		Vector3 cc2 = new Vector3 (175, -390, 0);
		Vector3 cc3 = new Vector3 (350, -390, 0);

		//int idd = 0;
		if(rightanswrindex==1)
		{
			//idd = answer1;
			//rightans.spriteName = ""+answer1;
			rightans.transform.localPosition = cc1;
			//ans1.spriteName = ""+answer2;
			ans1.transform.localPosition = cc2;
			//ans2.spriteName= ""+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==2)
		{
			//idd = answer2;
			//rightans.spriteName = ""+answer2;
			rightans.transform.localPosition = cc2;
			//ans1.spriteName = ""+answer1;
			ans1.transform.localPosition = cc1;
			//ans2.spriteName= ""+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==3)
		{
			//idd = answer3;
			//rightans.spriteName = ""+answer3;
			rightans.transform.localPosition = cc3;
			//ans1.spriteName = ""+answer1;
			ans1.transform.localPosition = cc1;
			//ans2.spriteName= ""+answer2;
			ans2.transform.localPosition = cc2;
		}

		proc (ans1);
		proc (ans2);
		proc (rightans);

		gameBlock.level [0] = this;

		foreach (var v in hms)
		{
			v.spriteName = "" + howmany;
			v.MakePixelPerfect();
		}


		foreach(Transform item in shapes.transform)
		{
			if(item.GetComponent<EndItemColor>() != null)
				DestroyImmediate(item.GetComponent<EndItemColor>());
			if(item.GetComponent<EndItemChangeTexture>() != null)
				DestroyImmediate(item.GetComponent<EndItemChangeTexture>());

			if(item.name == "shape" && item.GetComponent<UISprite>().spriteName != rightans.spriteName)
			{
				item.gameObject.AddComponent<EndItemColor>();
				item.gameObject.GetComponent<EndItemColor>().to = new Color(1f,1f,1f,0f);
				item.gameObject.GetComponent<EndItemColor>().duration = 1f;
				item.gameObject.GetComponent<EndItemColor>().delay = 0.0f;
			}

			if(item.name == "shapeX")
			{
				item.gameObject.AddComponent<EndItemColor>();
				item.gameObject.GetComponent<EndItemColor>().to = new Color(1f,1f,1f,1f);
				item.gameObject.GetComponent<EndItemColor>().duration = 1f;
				item.gameObject.GetComponent<EndItemColor>().delay = 0.0f;
				item.GetComponent<UISprite>().color = new Color(1f,1f,1f,1f- item.GetComponent<UISprite>().color.a);
			}
		}
		shapes.EditorAddChilds ();

		shapeX.gameObject.GetComponent<EndItemColor>().to = new Color(1f,1f,1f,1f);
		shapeX.gameObject.GetComponent<EndItemColor>().duration = 1f;
		shapeX.gameObject.GetComponent<EndItemColor>().delay = 0.0f;
		shapeX.GetComponent<UISprite>().color = new Color(1f,1f,1f,1f- shapeX.GetComponent<UISprite>().color.a);
	}

	void proc(UISprite sprite)
	{
		sprite.type = UISprite.Type.Simple;
		sprite.MakePixelPerfect ();
		sprite.type = UISprite.Type.Sliced;
	}

	#endregion
}
