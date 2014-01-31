using UnityEngine;
using System.Collections;

public class TextGen : MonoBehaviour
{
	Texture2D texture = null;
	
	int colorSize = 512 * 512;
	Color32[] colorSetA = new Color32[512 * 512];
	Color32[] colorSetB = new Color32[512 * 512];
	
    void Start()
	{	
		texture = new Texture2D(512, 512);
        renderer.material.mainTexture = texture;
		
		for(int i = 0; i < colorSize; ++i)
			colorSetA[i] = new Color32(255, 0, 0, 255);
		
		for(int i = 0; i < colorSize; ++i)
			colorSetB[i] = new Color32(0, 0, 255, 255);
    }
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			texture.SetPixels32(colorSetA);
        	texture.Apply(true);
		}
		else if(Input.GetKeyDown(KeyCode.B))
		{
			texture.SetPixels32(colorSetB);
        	texture.Apply(true);
		}
	}
}
