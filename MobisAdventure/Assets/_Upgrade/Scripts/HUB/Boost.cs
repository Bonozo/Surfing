using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {

	private int maxBoost = 6;
	private int currentBoost=0;
	private int frame = 0;
	private int maxframe = 4;
	private float framerate = 0.25f;

	void OnClick(bool isDown)
	{
		if(isDown)
		{
			// to do
		}
	}

	IEnumerator Start()
	{
		UISprite sprite = GetComponent<UISprite> ();
		while(true)
		{
			if(frame==0)
				yield return new WaitForSeconds(1f);
			else
				yield return new WaitForSeconds (framerate);
			int cmpframe = Mathf.Min(maxframe,currentBoost);
			if(++frame > cmpframe) frame = -cmpframe;
			sprite.spriteName = "Boost-Ready-Light" + Mathf.Abs(frame);
		}
	}

	public void AddBoost(int amount)
	{
		currentBoost += amount;
		if(currentBoost > maxBoost)
			currentBoost = maxBoost;

		if(currentBoost==0) framerate = 0.1f;
		if(currentBoost==1) framerate= 0.5f;
		if(currentBoost==2) framerate= 0.11f;
		if(currentBoost>=2) framerate= 0.09f;

	}

	public void ClearBoosts()
	{
		currentBoost = 0;
	}
}
