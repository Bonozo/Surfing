using UnityEngine;
using System.Collections;

public class GUI_menu : MonoBehaviour {
	
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
		Debug.Log("RIGHT");
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 500));
		Vector3 myXY = transform.localPosition;
		transform.position = Vector3.right*worldPoint.x;
		transform.localPosition = new Vector3(myXY.x, myXY.y, transform.localPosition.z);

	}
		void AlignLeft () {
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 500));
		Vector3 myXY = transform.localPosition;
		transform.position = Vector3.right*worldPoint.x;
		transform.localPosition = new Vector3(myXY.x, myXY.y, transform.localPosition.z);


	}
}
