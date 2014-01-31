using UnityEngine;
using System.Collections;

public class FitScreen : MonoBehaviour {
	public exSpriteFont speedo;
	
	void Start () {
		//get the exSprite component
		exSprite exS = GetComponent <exSprite>();		
		//set the camera viewing this sprite is using to the propper orthographic settings
		exS.renderCamera.orthographic = true;
		exS.renderCamera.orthographicSize = Screen.height / 2;
		//get the difference in screen and sprite with
		float ratio = Screen.width/exS.width;
		//allows us to change the values of the sprite's w&h
		exS.customSize = true;
		//sprite width will fit the screen
		exS.width = Screen.width;
		//sprite hight will maintain it's ratio with it's width
		exS.height = exS.height*ratio;
		
		exViewportPosition speedEVP = speedo.GetComponent <exViewportPosition>();
		
		float ratioSpeedo = (Screen.height/exS.height);
		speedEVP.y  = speedEVP.y/ratioSpeedo;
		
		speedo.scale = speedo.scale/ratioSpeedo;
//		speedo.scale.y = speedo.scale.y/ratioSpeedo;
		
	}
}
