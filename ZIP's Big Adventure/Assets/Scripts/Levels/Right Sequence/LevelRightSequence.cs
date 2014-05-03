using UnityEngine;
using System.Collections;

public class LevelRightSequence : ZIPLevel {
	
	public int countBy;

	private int current;
	private int count;

	public override void StartGame ()
	{
		current = countBy;
		count = 0;
		
		gameObject.SetActive (true);
		var game = transform.GetChild (0);
		foreach(Transform part in game)
		{
			var child = part.GetComponent<LevelRightSequenceButton>();
			if(child != null)
			{
				count++;
				child.Reset();
			}
		}
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
	}
	
	public void Answer(LevelRightSequenceButton answer)
	{
		if(answer.value == current)
		{
			current += countBy;
			count--;
			GameController.Instance.PlayCorrectAnswer ();
			answer.PlayAnim(true);
		}
		else
		{
			GameController.Instance.PlayWrongAnswer ();
			answer.PlayAnim(false);
		}

		if(count==0)
			StartCoroutine(HappyEndThread());

	}
	
	private IEnumerator HappyEndThread()
	{
		SendMessage ("PlayFinish",SendMessageOptions.DontRequireReceiver);
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
	}

}
