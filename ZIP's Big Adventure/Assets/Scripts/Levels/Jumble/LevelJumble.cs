using UnityEngine;
using System.Collections;

public class LevelJumble : ZIPLevel {
	
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
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
	}

	public void Answered(JumbleDrag drag)
	{
		done++;
		if(drag.GetComponent<UISprite>() != null)
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

	#region Editor Letters/First/Punctuation

	/*public UIAtlas atlas;
	public GameObject box;
	public GameObject picture;
	public GameObject title;

	public string pictureA;
	public string pictureB;
	public string[] words;

	public void Initialize()
	{
		title.GetComponent<UISprite> ().atlas = atlas;

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
	}*/

	#endregion

	#region Editor Letters/Kindergarten/Position Words

	/*public string[] names;
	public int correct;
	public UISprite cor, wr1, wr2;
	public UISprite box;

	public void Initialize()
	{
		Vector3[] tt = new Vector3[] { new Vector3(-500f,-310f,0f),new Vector3(0,-310f,0f),new Vector3(500f,-310f,0f)};

		transform.localPosition = new Vector3 (0f, 0f, 0f);
		transform.localScale = new Vector3 (1f, 1f, 1f);
		gameBlock.level [0] = this;

		if(correct==1)
		{
			cor.spriteName = names[0]; cor.gameObject.transform.localPosition = tt[0];
			wr1.spriteName = names[1]; wr1.gameObject.transform.localPosition = tt[1];
			wr2.spriteName = names[2]; wr2.gameObject.transform.localPosition = tt[2];
		}

		if(correct==2)
		{
			cor.spriteName = names[1]; cor.gameObject.transform.localPosition = tt[1];
			wr1.spriteName = names[0]; wr1.gameObject.transform.localPosition = tt[0];
			wr2.spriteName = names[2]; wr2.gameObject.transform.localPosition = tt[2];
		}

		if(correct==3)
		{
			cor.spriteName = names[2]; cor.gameObject.transform.localPosition = tt[2];
			wr1.spriteName = names[0]; wr1.gameObject.transform.localPosition = tt[0];
			wr2.spriteName = names[1]; wr2.gameObject.transform.localPosition = tt[1];
		}

		cor.MakePixelPerfect ();
		wr1.MakePixelPerfect ();
		wr2.MakePixelPerfect ();

		box.spriteName = names [correct - 1];
		box.MakePixelPerfect ();
		box.enabled = !box.enabled;
	}*/

	#endregion

	#region Editor Shapes/PreK/Drag

	/*public string txt;
	public UISprite[] box, item;
	public UISprite gray;

	public void Initialize()
	{
		gameBlock.level [0] = this;
		
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3 (1f, 1f, 1f);

		for(int i=0;i<item.Length;i++)
		{
			box[i].spriteName = item[i].spriteName = txt+(i+1).ToString();
			box[i].MakePixelPerfect();
			item[i].MakePixelPerfect();
			item[i].GetComponent<JumbleDrag>().targetCollider[0] = box[i].collider;
		}
		gray.spriteName = txt + " Gray";
		gray.MakePixelPerfect ();
	}*/

	#endregion

	#region Editor Letters/PreK/Dragging

	/*public string word;
	public Transform box;
	public int singleDistance = 200;

	public void Initialize()
	{
		box.transform.localPosition = new Vector3 (0f, -20f, 0f);
		return;

		gameBlock.level [0] = this;
		
		transform.localPosition = Vector3.zero;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		
		dragsLenght = word.Length;
		drags = new JumbleDrag[dragsLenght];

		for(int i=1;i<=7;i++)
		{
			box.FindChild("box"+i).gameObject.SetActive(i<=dragsLenght);
			box.FindChild("letter"+i).gameObject.SetActive(i<=dragsLenght);
		}

		int distdelta = singleDistance * (dragsLenght-1) / 2;
		for(int i=1;i<=dragsLenght;i++)
		{
			drags[i-1] = box.Find("letter"+i).GetComponent<JumbleDrag>();
			MP(box.FindChild("box"+i),"gray"+word[i-1]);
			MP (box.Find("letter"+i),""+word[i-1]);

			box.FindChild("box"+i).transform.localPosition = new Vector3(-distdelta+(i-1)*singleDistance,20f,0f);
			box.FindChild("letter"+i).transform.localPosition = new Vector3(-distdelta+(i-1)*singleDistance,230f,0f);

			var current = box.FindChild("letter"+i).GetComponent<JumbleDrag>();
			int count = 0; for(int j=0;j<drags.Length;j++) if(word[j] == word[i-1]) count++;
			current.targetCollider = new Collider[count];
			for(int j=0,k=0;j<drags.Length;j++) 
				if(word[j] == word[i-1])
				{
					current.targetCollider[k++] = box.FindChild("box"+(j+1)).collider;
				}

			float randY = (Random.Range(0,10)>5)?230:-190;
			float randX = Random.Range(-distdelta-50,distdelta+50);
			box.FindChild("letter"+i).transform.localPosition = new Vector3(randX,randY,0f);
		}
	}

	void MP(Transform tr ,string nm)
	{
		var sprite = tr.GetComponent<UISprite> ();
		sprite.spriteName = nm;
		sprite.type = UISprite.Type.Simple;
		sprite.MakePixelPerfect ();
		sprite.type = UISprite.Type.Sliced;
	}*/

	#endregion

	#region Editor Numbers/PreK/Drag
	
	/*public string[] names;
	public int correct;
	public UISprite cor, wr1, wr2;
	public UISprite box;

	public void Initialize()
	{		
		transform.localPosition = new Vector3 (0f, 0f, 0f);
		transform.localScale = new Vector3 (1f, 1f, 1f);
		gameBlock.level [0] = this;

		Vector3[] tt = new Vector3[] { new Vector3(-500f,-310f,0f),new Vector3(0,-310f,0f),new Vector3(500f,-310f,0f)};

		if(correct==1)
		{
			cor.spriteName = names[0]; cor.gameObject.transform.localPosition = tt[0];
			wr1.spriteName = names[1]; wr1.gameObject.transform.localPosition = tt[1];
			wr2.spriteName = names[2]; wr2.gameObject.transform.localPosition = tt[2];
		}

		if(correct==2)
		{
			cor.spriteName = names[1]; cor.gameObject.transform.localPosition = tt[1];
			wr1.spriteName = names[0]; wr1.gameObject.transform.localPosition = tt[0];
			wr2.spriteName = names[2]; wr2.gameObject.transform.localPosition = tt[2];
		}

		if(correct==3)
		{
			cor.spriteName = names[2]; cor.gameObject.transform.localPosition = tt[2];
			wr1.spriteName = names[0]; wr1.gameObject.transform.localPosition = tt[0];
			wr2.spriteName = names[1]; wr2.gameObject.transform.localPosition = tt[1];
		}

		cor.MakePixelPerfect ();
		wr1.MakePixelPerfect ();
		wr2.MakePixelPerfect ();

		box.spriteName = names [correct - 1];
		box.MakePixelPerfect ();
		box.enabled = !box.enabled;
	}
	*/
	#endregion
}
