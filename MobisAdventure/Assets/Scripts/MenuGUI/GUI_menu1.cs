using UnityEngine;
using System.Collections;

public class GUI_menu1 : MonoBehaviour {
	
	public Camera thisCam;
	public bool fitToScreen;
	public bool alignRight;
	public bool alignLeft;
	

	void Awake () {

		if(fitToScreen)
			FitToScreen();
		if(alignRight)
			AlignRight();
		if(alignLeft)
			AlignLeft();
	}
	

	void FitToScreen () {
		exSprite exS;
		exS = gameObject.GetComponent<exSprite>();
		exS.customSize = true;
		exS.height = Screen.height/2;
		exS.width = exS.height*2;
	}
	
	void AlignRight () {
		Vector3 worldPoint = thisCam.ScreenToWorldPoint(new Vector3(0, 0, 30));
		transform.localPosition = new Vector3(worldPoint.x, transform.localPosition.y, transform.localPosition.z) ;

	}
	void AlignLeft () {
		Vector3 worldPoint = thisCam.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 30));
		transform.localPosition = new Vector3(worldPoint.x, transform.localPosition.y, transform.localPosition.z) ;
	}
}
