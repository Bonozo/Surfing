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
	}
	
	private IEnumerator HappyEndThread()
	{
		GameController.Instance.PlayCorrectAnswer ();
		SendMessage ("PlayFinish", SendMessageOptions.DontRequireReceiver);

		foreach(var btr in buttons) btr.DisableCollider();
		foreach(var et in endItems) et.Work();


		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}

	#region Editor

	private Vector3[] c = new Vector3[] {
		new Vector3(255,94,0),
		new Vector3(0,120,199),
		new Vector3(251,39,188),
		new Vector3(62,120,199),
		new Vector3(0,175,0),
		new Vector3(219,0,191),
		new Vector3(0,164,0),
		new Vector3(255,130,0),
		new Vector3(255,76,199),
		new Vector3(116,76,199),
		new Vector3(255,0,255),
		new Vector3(210,77,202),
		new Vector3(78,171,134)
	};

	private Color RandomColor(){
		var cc = c [Random.Range (0, c.Length)];
		Color d = new Color (cc.x / 256f, cc.y / 256f, cc.z / 256f, 1f);
		return d;
	}

	public void Initialize(){
		var p = transform.FindChild ("Game");
		var p1 = p.transform.Find ("word1").GetComponent<UILabel> ();
		var p2 = p.transform.Find ("word2").GetComponent<UILabel> ();
		var p3 = p.transform.Find ("word3").GetComponent<UILabel> ();

		p1.color = RandomColor ();
		p2.color = RandomColor ();
		p3.color = RandomColor ();
	}
	#endregion

}
