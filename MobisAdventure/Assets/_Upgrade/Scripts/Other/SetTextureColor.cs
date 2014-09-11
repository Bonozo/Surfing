using UnityEngine;
using System.Collections;

public class SetTextureColor : MonoBehaviour {

	public Texture2D texture;

	void Start(){
		for(int i=0;i<texture.width;i++)
			for(int j=0;j<texture.height;j++){
			var col = texture.GetPixel(i,j);
			col.g = col.b = col.r;
			texture.SetPixel(i,j,col);
		}

		texture.Apply ();
		byte[] bytes = texture.EncodeToPNG();
		System.IO.File.WriteAllBytes("D:\\green.png", bytes);
		
	}
}
