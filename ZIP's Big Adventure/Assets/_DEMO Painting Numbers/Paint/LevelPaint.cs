using UnityEngine;
using System.Collections;

public class LevelPaint : ZIPLevel {
	
	public Texture2D texture;
	public Color fillColor = Color.green;
	public UILabel labelPercent;
	public EndItem[] endItems;
	public float xpos = 0.2f;
	public float ypos = 0.2f;
	public float xscale = 0.2f;
	public float radius = 0.08f;

	private GameObject grid;
	private int width, height;
	private int pixelCount;
	public int pixelTotal;
	private bool done = false;
	private Vector3 lastMousePos;

	public override void StartGame ()
	{
		
		foreach(var et in endItems) et.Reset();

		float yscale = xscale * texture.height / texture.width;


		grid = new GameObject ();
		grid.name = "PaintNumber(Temporary)";
		grid.transform.position = new Vector3 (0, 0, 0f);
		grid.transform.localScale = new Vector3 (0f, 0f, 0f);
		grid.AddComponent ("GUITexture");
		grid.guiTexture.pixelInset = 
			new Rect ((xpos-xscale*0.5f)*Screen.width, (ypos-yscale*0.5f*Screen.width/Screen.height)*Screen.height,
			          xscale*Screen.width, yscale*Screen.width);
		grid.guiTexture.texture = texture;
		
		// Get the texture parameters
		width = texture.width;
		height = texture.height;
		
		// Clear texture
		for(int i=0;i<width;i++)
			for(int j=0;j<height;j++)
			if(texture.GetPixel(i,j) != Color.black && texture.GetPixel(i,j) != Color.clear){
				texture.SetPixel(i,j,Color.white);
				pixelCount++;
			}
		pixelTotal = pixelCount;
		texture.Apply ();

		done = false;
		lastMousePos = Vector3.zero;
		labelPercent.text = "0%";

		gameObject.SetActive (true);
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
	}
	public void FillEllipse(int x,int y,int r){
		if(texture.GetPixel(x,y) != Color.white && texture.GetPixel(x,y) != fillColor) return;
		for (int i=-r; i<=r; i++){
			int yy = Mathf.RoundToInt(Mathf.Sqrt(1f*r*r-i*i));
			for(int j=-yy;j<=yy;j++){
				if(x+i>=0&&x+i<texture.width&&y+j>=0&&y+j<texture.height&&texture.GetPixel(x+i,y+j)==Color.white){
					texture.SetPixel(x+i,y+j,fillColor);
					pixelCount--;
				}
			}
		}
		texture.Apply ();
	}
	
	void Update(){
		if(done) return;

		if(Input.GetMouseButton(0)){
			if(lastMousePos==Vector3.zero){
				Draw(Input.mousePosition);
			}
			else{
				var del = (Input.mousePosition-lastMousePos).normalized;
				int step = (int)Vector3.Distance(Input.mousePosition,lastMousePos);
				for(int i=1;i<step;i+=5)
					Draw(lastMousePos + i*del);
				Draw(Input.mousePosition);
			}
			lastMousePos = Input.mousePosition;
		}
		if(Input.GetMouseButtonUp(0)){
			lastMousePos = Vector3.zero;
		}
	}

	void Draw(Vector3 pos){
		if(done) return;
		pos.x -= grid.guiTexture.pixelInset.x;
		pos.y -= grid.guiTexture.pixelInset.y;
		
		int xpixel = (int)(pos.x*width/grid.guiTexture.pixelInset.width);
		int ypixel = (int)(pos.y*height/grid.guiTexture.pixelInset.height);

		if(xpixel>=0 && xpixel<texture.width && ypixel>=0 && ypixel<texture.height)
			FillEllipse(xpixel,ypixel,(int)(radius*Screen.width));
		
		int percent = (int)(100f*(pixelTotal-pixelCount)/pixelTotal);
		if(pixelCount < 15f){
			done = true;
			percent = 100;
			StartCoroutine(HappyEndThread());
		}
		labelPercent.text = percent.ToString() + "%";
	}

	private IEnumerator HappyEndThread()
	{
		GameController.Instance.PlayCorrectAnswer ();
		SendMessage ("PlayFinish", SendMessageOptions.DontRequireReceiver);

		yield return new WaitForSeconds (0.5f);
		float xspeed = 8f * grid.guiTexture.pixelInset.width;
		float yspeed = 8f * grid.guiTexture.pixelInset.height;
		while(grid.guiTexture.pixelInset.width>0 || grid.guiTexture.pixelInset.height>0){
			var px = grid.guiTexture.pixelInset;
			px.width-=xspeed*Time.deltaTime;
			px.height-=yspeed*Time.deltaTime;
			px.x += 0.5f*xspeed*Time.deltaTime;
			px.y += 0.5f*yspeed*Time.deltaTime;
			if(px.width<0f || px.height<0f)
				px.width = px.height = 0;
			grid.guiTexture.pixelInset = px;
			yield return null;
		}

		foreach(var et in endItems) et.Work();
		
		
		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo ();
		yield return new WaitForSeconds(6f);
		Destroy (grid.gameObject);
		gameBlock.LevelComplete ();
		
	}
}
