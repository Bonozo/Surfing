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
	
	/*public int number1, number2, answer1, answer2, answer3,rightanswrindex;
	public UISprite sp1, sp2, ans1, ans2, rightans;
	

	
	public void Initialize()
	{
		Vector3 cc1 = new Vector3 (410, 379, 0);
		Vector3 cc2 = new Vector3 (410, 179, 0);
		Vector3 cc3 = new Vector3 (410, -26, 0);

		transform.localPosition = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (1, 1, 1);
		sp1.spriteName = "" + number1;
		sp1.type = UISprite.Type.Simple;
		sp1.MakePixelPerfect ();
		sp1.type = UISprite.Type.Sliced;
		sp2.spriteName = "" + number2;
		sp2.type = UISprite.Type.Simple;
		sp2.MakePixelPerfect ();
		sp2.type = UISprite.Type.Sliced;
		
		int idd = 0;
		if(rightanswrindex==1)
		{
			idd = answer1;
			rightans.spriteName = "Circle"+answer1;
			rightans.transform.localPosition = cc1;
			ans1.spriteName = "Circle"+answer2;
			ans1.transform.localPosition = cc2;
			ans2.spriteName= "Circle"+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==2)
		{
			idd = answer2;
			rightans.spriteName = "Circle"+answer2;
			rightans.transform.localPosition = cc2;
			ans1.spriteName = "Circle"+answer1;
			ans1.transform.localPosition = cc1;
			ans2.spriteName= "Circle"+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==3)
		{
			idd = answer3;
			rightans.spriteName = "Circle"+answer3;
			rightans.transform.localPosition = cc3;
			ans1.spriteName = "Circle"+answer1;
			ans1.transform.localPosition = cc1;
			ans2.spriteName= "Circle"+answer2;
			ans2.transform.localPosition = cc2;
		}
		
		rightans.GetComponent<EndItemChangeTexture> ().newSpriteName = "star" + idd;
		gameBlock.level [0] = this;

	}*/

	public int answer1, answer2, answer3,rightanswrindex;
	public UISprite ans1, ans2, rightans;
	

	
	public void Initialize()
	{
		Vector3 cc1 = new Vector3 (-416, 0, 0);
		Vector3 cc2 = new Vector3 (0, 0, 0);
		Vector3 cc3 = new Vector3 (416, 0, 0);

		transform.localPosition = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (1, 1, 1);
		
		int idd = 0;
		if(rightanswrindex==1)
		{
			idd = answer1;
			//rightans.spriteName = ""+answer1;
			rightans.transform.localPosition = cc1;
			//ans1.spriteName = ""+answer2;
			ans1.transform.localPosition = cc2;
			//ans2.spriteName= ""+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==2)
		{
			idd = answer2;
			//rightans.spriteName = ""+answer2;
			rightans.transform.localPosition = cc2;
			//ans1.spriteName = ""+answer1;
			ans1.transform.localPosition = cc1;
			//ans2.spriteName= ""+answer3;
			ans2.transform.localPosition = cc3;
		}
		if(rightanswrindex==3)
		{
			idd = answer3;
			//rightans.spriteName = ""+answer3;
			rightans.transform.localPosition = cc3;
			//ans1.spriteName = ""+answer1;
			ans1.transform.localPosition = cc1;
			//ans2.spriteName= ""+answer2;
			ans2.transform.localPosition = cc2;
		}
		
		//rightans.GetComponent<EndItemChangeTexture> ().newSpriteName = "star" + idd;
		gameBlock.level [0] = this;

	}
	
	#endregion
}
