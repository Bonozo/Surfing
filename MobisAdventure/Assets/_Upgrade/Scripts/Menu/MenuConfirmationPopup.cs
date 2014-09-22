/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class MenuConfirmationPopup : MonoBehaviour {

	// References
	public UILabel popupLabel;
	public UILabel leftButtonLabel;
	public UILabel rightButtonLabel;

	private bool gotStatus=false;
	public bool status{get;private set;}
	public void Confirm(bool status){
		this.status = status;
		gotStatus=true;
	}
	public IEnumerator ShowPopupThread(string message,string confirmMessage,string backMessage){
		popupLabel.text = message;
		leftButtonLabel.text = backMessage;
		rightButtonLabel.text = confirmMessage;
		gotStatus=false;
		gameObject.SetActive(true);
		while(!gotStatus) yield return null;
		gameObject.SetActive(false);
	}
}
