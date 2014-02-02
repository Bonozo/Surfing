using UnityEngine;
using System.Collections;

public class LevelPictureWithWords : ZIPLevel {

	public GameBlock gameBlock;
	public PictureWithWordButton[] buttons;
	public PictureWithWordButton correctAnswer;
	public EndItem[] endItems;
	
	public AudioClip clipCorrectAnswer;
	public AudioClip clipWrongAnswer;
	
	public override void StartGame ()
	{
		foreach(var btr in buttons) btr.Reset();
		foreach(var et in endItems) et.Reset();
		gameObject.SetActive (true);
	}
	
	public void Answered(PictureWithWordButton point)
	{
		if(point == correctAnswer)
		{
			StartCoroutine(HappyEndThread());
		}
		else
		{
			AudioSource.PlayClipAtPoint(clipWrongAnswer,transform.position);
		}
	}
	
	private IEnumerator HappyEndThread()
	{
		AudioSource.PlayClipAtPoint(clipCorrectAnswer,transform.position);
		
		foreach(var btr in buttons) btr.DisableCollider();
		foreach(var et in endItems) et.Work();


		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}
}
