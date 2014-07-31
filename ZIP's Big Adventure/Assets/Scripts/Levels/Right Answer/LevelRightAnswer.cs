using UnityEngine;
using System.Collections;

public class LevelRightAnswer : ZIPLevel {
	
	public LevelRightAnswerButton[] buttons;
	public LevelRightAnswerButton correctAnswer;
	public EndItem[] endItems;
	
	public override void StartGame ()
	{
		foreach(var btr in buttons) btr.Reset();
		foreach(var et in endItems) et.Reset();
		gameObject.SetActive (true);
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
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
		SendMessage ("PlayFinish",SendMessageOptions.DontRequireReceiver);
		
		foreach(var btr in buttons) btr.DisableCollider();
		foreach(var et in endItems) et.Work();
		
		
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
	}

	#region Editor

	public void Initialize(){
		var g = transform.FindChild ("Game");

		float xT = 90f;

		g.Find("box").GetComponent<UISprite>().depth = 2;

		//XX (g.transform.Find ("number1"), xT);

		/* float delta1 = 250f;
		 float delta2 = 750f;
		 float delta3 = -120;

		Add (g.FindChild ("text1"), delta1);
		Add (g.FindChild ("number1"), delta1);

		Add (g.Find("answers"),delta2);
		Add (g.Find("text2"),delta2);
		Add (g.Find("number2"),delta2);
		
		Add (g.Find("shapes"),delta3);
		Add (g.Find("box"),delta3);*/
	}

	void XX(Transform t,float delta){
		var tt = t.transform.localPosition;
		tt.x += delta;
		t.transform.localPosition = tt;
	}

	void Add(Transform t,float delta){
		var tt = t.transform.localPosition;
		tt.y += delta;
		t.transform.localPosition = tt;
	}

	#endregion
}
