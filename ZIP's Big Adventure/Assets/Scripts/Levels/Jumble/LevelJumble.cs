using UnityEngine;
using System.Collections;

public class LevelJumble : ZIPLevel {

	public GameBlock gameBlock;
	public JumbleDrag[] drags;
	public EndItem[] endItems;
	public int dragsLenght;

	private int done;

	public override void StartGame ()
	{
		foreach(var dr in drags) dr.Reset();
		foreach(var et in endItems) et.Reset();
		done = 0;

		gameObject.SetActive (true);
	}

	public void Answered(JumbleDrag drag)
	{
		done++;
		drag.GetComponent<UISprite> ().depth--;
		if( done == dragsLenght)
			StartCoroutine(HappyEndThread());
	}

	private IEnumerator HappyEndThread()
	{
		foreach(var et in endItems) et.Work();
		foreach(var dr in drags) dr.DisableCollider();

		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		gameBlock.LevelComplete ();
		
	}

	#region Editor

	public UIAtlas atlas;
	public GameObject box;
	public GameObject picture;
	public GameObject title;

	public string pictureA;
	public string pictureB;
	public string[] words;

	public void Initialize()
	{
		title.GetComponent<UISprite> ().atlas = atlas;
		/*for(int i=1;i<=dragsLenght;i++)
		{
			box.transform.FindChild("box"+i).GetComponent<UISprite>().atlas = atlas;
			box.transform.FindChild("box"+i).GetComponent<UISprite>().spriteName = "Gray Box";
		}*/

		gameBlock.level [0] = this;

		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3 (1f, 1f, 1f);

		picture.GetComponent<UISprite> ().atlas = atlas;
		picture.GetComponent<UISprite> ().type = UISprite.Type.Simple;
		picture.GetComponent<UISprite> ().spriteName = pictureB;
		picture.GetComponent<UISprite> ().MakePixelPerfect ();
		//picture.GetComponent<EndItemChangeTexture> ().newScale = transform.localScale;
		picture.GetComponent<EndItemChangeTexture> ().newSpriteName = pictureB;
		picture.GetComponent<UISprite> ().spriteName = pictureA;
		picture.GetComponent<UISprite> ().MakePixelPerfect ();
		picture.GetComponent<UISprite> ().type = UISprite.Type.Sliced;



		dragsLenght = words.Length;
		drags = new JumbleDrag[dragsLenght];
		for(int i=1;i<=dragsLenght;i++)
		{
			var letter = box.transform.FindChild("letter"+i).GetComponent<UISprite>();

			letter.transform.localPosition = 
				box.transform.Find("box"+Random.Range(1,dragsLenght+1)).transform.localPosition + 
					(new Vector3(0,Random.Range(0,10)>4?1:-1,0)*195f);

			letter.type = UISprite.Type.Simple;
			letter.spriteName = words[i-1];
			letter.MakePixelPerfect();
			letter.type = UISprite.Type.Sliced;
			int count=0;
			for(int j=0;j<dragsLenght;j++)
				if( words[j] == words[i-1])
					count++;
			var drag = letter.GetComponent<JumbleDrag>();
			drags[i-1] = drag;
			drag.targetCollider = new Collider[count];
			for(int j=0,jj=0;j<drag.targetCollider.Length;jj++)
			{
				if(words[jj] == words[i-1])
				{
					drag.targetCollider[j] = box.transform.FindChild("box"+(jj+1)).collider;
					j++;
				}
			}
		}
	}

	#endregion
}
