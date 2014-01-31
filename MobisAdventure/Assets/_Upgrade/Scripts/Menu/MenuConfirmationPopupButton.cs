using UnityEngine;
using System.Collections;

public class MenuConfirmationPopupButton : MonoBehaviour {

	public MenuConfirmationPopup popup;
	public bool status;

	void OnPress()
	{
		//popup.OnPress(align);
		popup.Confirm(status);
	}
}
