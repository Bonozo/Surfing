using UnityEngine;
using System.Collections;

public class LevelPictureWithWords : ZIPLevel {
	
	public PictureWithWordButton[] buttons;
	public PictureWithWordButton correctAnswer;
	public EndItem[] endItems;

	public override void StartGame ()
	{
		foreach(var btr in buttons) btr.Reset();
		foreach(var et in endItems) et.Reset();
		gameObject.SetActive (true);
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
	}
	
	public void Answered(PictureWithWordButton point)
	{
		if(point == correctAnswer)
		{
			StartCoroutine(HappyEndThread());
		}
		else
		{
			GameController.Instance.PlayWrongAnswer();
		}
		SendMessage ("PlayEnd", point == correctAnswer, SendMessageOptions.DontRequireReceiver);
	}
	
	private IEnumerator HappyEndThread()
	{
		GameController.Instance.PlayCorrectAnswer ();
		
		foreach(var btr in buttons) btr.DisableCollider();
		foreach(var et in endItems) et.Work();


		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}
}
