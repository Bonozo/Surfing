using UnityEngine;
using System.Collections;

public class LevelPaint : ZIPLevel {
	
	public Texture2D texture;
	public Color fillColor = Color.green;

	private GameObject grid;
	private int width, height;
	private int pixelCount;

	public override void StartGame ()
	{
		/*texture = gridTexture.mainTexture;
		for(int i=0;i<10;i++)
			texture.SetPixel(i,0,Color.green);
		texture.Apply ();*/
	}

	void Awake(){
		// Create a grid
		grid = new GameObject ();
		grid.name = "PaintNumber(Temporary)";
		grid.transform.position = new Vector3 (0f, 0f, 0f);
		grid.transform.localScale = new Vector3 (0f, 0f, 0f);
		grid.AddComponent ("GUITexture");
		grid.guiTexture.pixelInset = new Rect (0, 0, 480, 320);
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
		texture.Apply ();
	}

	public void FillEllipse(int x,int y,int r){
		for (int i=-r; i<=r; i++){
			int yy = Mathf.RoundToInt(Mathf.Sqrt(1f*r*r-i*i));
			for(int j=-yy;j<=yy;j++){
				int a=x+i,b=y+j;
				if(a>=0&&a<width&&b>=0&&b<=height&&texture.GetPixel(a,b)==Color.white){
					texture.SetPixel(a,b,fillColor);
					pixelCount--;
					if(pixelCount<100)
						Debug.Log("pixels: " + pixelCount);
					if(pixelCount == 0)
						Debug.Log("Well Done");
				}
			}
		}
		texture.Apply ();
	}

	void Update(){
		if(Input.GetMouseButton(0)){
			var pos = Input.mousePosition;
			pos.x -= grid.guiTexture.pixelInset.x;
			pos.y -= grid.guiTexture.pixelInset.y;

			int xpixel = (int)(pos.x*width/grid.guiTexture.pixelInset.width);
			int ypixel = (int)(pos.y*height/grid.guiTexture.pixelInset.height);

			FillEllipse(xpixel,ypixel,25);
		}
	}
}
