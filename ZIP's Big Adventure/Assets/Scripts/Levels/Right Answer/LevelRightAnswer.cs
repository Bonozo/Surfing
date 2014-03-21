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

	//public string answer1, answer2, answer3;
	public int correctAnswerIndex;
	public UISprite appears, appearInx;
	
	public void Initialize()
	{
		for(int i=0;i<4;i++)
			buttons[i].GetComponent<BoxCollider>().size = new Vector3(1f,1f,1f);
		transform.localPosition = new Vector3 (0, 0, 0);
		transform.localScale = new Vector3 (1, 1, 1);
		gameBlock.level [0] = this;

		correctAnswer = buttons [correctAnswerIndex - 1];
		endItems = new EndItem[5];
		appears.color = new Color (1f, 1f, 1f, 1f-appears.color.a);
		appearInx.gameObject.SetActive (!appearInx.gameObject.activeSelf);

		for(int i=0;i<4;i++)
		{
			if(buttons[i].GetComponent<EndItemColor>() != null)
				DestroyImmediate(buttons[i].GetComponent<EndItemColor>());
			if(buttons[i].GetComponent<EndItemMoveTo>() != null)
				DestroyImmediate(buttons[i].GetComponent<EndItemMoveTo>());
			if(buttons[i].GetComponent<EndItemMultipleEndItem>() != null)
				DestroyImmediate(buttons[i].GetComponent<EndItemMultipleEndItem>());
			if(buttons[i].GetComponent<EndItemChangeTexture>() != null)
				DestroyImmediate(buttons[i].GetComponent<EndItemChangeTexture>());

			buttons[i].GetComponent<UISprite>().MakePixelPerfect();

			if(i == correctAnswerIndex-1)
			{
				buttons[i].gameObject.AddComponent<EndItemMoveTo>();
				buttons[i].GetComponent<EndItemMoveTo>().defaultUp = false;
				buttons[i].GetComponent<EndItemMoveTo>().duration = 1.5f;
				buttons[i].GetComponent<EndItemMoveTo>().to = new Vector3(0f,200f,0f);
			}
			else
			{
				buttons[i].gameObject.AddComponent<EndItemColor>();
				buttons[i].GetComponent<EndItemColor>().to = new Color(1f,1f,1f,0f);
					buttons[i].GetComponent<EndItemColor>().duration = 0.75f;
			}
			endItems[i] = buttons[i].GetComponent<EndItem>();
		}
		endItems [4] = appears.GetComponent<EndItem> ();

	}

	void proc(UISprite sprite)
	{
		sprite.type = UISprite.Type.Simple;
		sprite.MakePixelPerfect ();
		sprite.type = UISprite.Type.Sliced;
	}

	#endregion
}
