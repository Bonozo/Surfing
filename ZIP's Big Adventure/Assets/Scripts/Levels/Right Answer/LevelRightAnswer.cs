using UnityEngine;
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

	public int answer1, answer2, answer3,rightanswrindex;
	public UISprite ans1, ans2, rightans;
	public UISprite shapeNewY;
	
	public void Initialize()
	{
		ans1.GetComponent<BoxCollider> ().size = new Vector3 (2f, 1.5f, 1f);
		ans2.GetComponent<BoxCollider> ().size = new Vector3 (2f, 1.5f, 1f);
		rightans.GetComponent<BoxCollider> ().size = new Vector3 (2f, 1.5f, 1f);

		transform.localPosition = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (1, 1, 1);

		Vector3 cc1 = new Vector3 (-220, -205, 0);
		Vector3 cc2 = new Vector3 (0, -205, 0);
		Vector3 cc3 = new Vector3 (220, -205, 0);

		//int idd = 0;
		if(rightanswrindex==1)
		{
			//idd = answer1;
			rightans.spriteName = ""+answer1;
			rightans.transform.localPosition = cc1;
			ans1.spriteName = ""+answer2;
			ans1.transform.localPosition = cc2;
			ans2.spriteName= ""+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==2)
		{
			//idd = answer2;
			rightans.spriteName = ""+answer2;
			rightans.transform.localPosition = cc2;
			ans1.spriteName = ""+answer1;
			ans1.transform.localPosition = cc1;
			ans2.spriteName= ""+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==3)
		{
			//idd = answer3;
			rightans.spriteName = ""+answer3;
			rightans.transform.localPosition = cc3;
			ans1.spriteName = ""+answer1;
			ans1.transform.localPosition = cc1;
			ans2.spriteName= ""+answer2;
			ans2.transform.localPosition = cc2;
		}

		shapeNewY.color = new Color (1f, 1f, 1f, 0f);
		gameBlock.level [0] = this;
		//rightans.GetComponent<EndItemChangeTexture> ().newSpriteName = "star" + idd;
	}

	#endregion
}
