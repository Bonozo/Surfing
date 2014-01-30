using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	public static string sceneName = "title";

	public UIPanel panel;

	private float speed = 1f;
	
	IEnumerator Start()
	{
		System.GC.Collect ();

		panel.alpha = 1f;
		var opr = Application.LoadLevelAsync (sceneName);
		var time = 0.0f;

		while(!opr.isDone || time<1f)
		{
			time += speed*Time.deltaTime;
			time = Mathf.Min(time,opr.progress);
			yield return null;
		}

		time = 0.0f;
		while(time < 1f)
		{
			time += speed* Time.deltaTime;
			time = Mathf.Min(1f,time);
			panel.alpha = 1-time;
			yield return null;
		}

		Destroy (this.gameObject);
	}
}
