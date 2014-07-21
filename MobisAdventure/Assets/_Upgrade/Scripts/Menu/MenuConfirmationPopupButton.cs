using UnityEngine;
using System.Collections;

public class MenuConfirmationPopupButton : MonoBehaviour {

	public MenuConfirmationPopup popup;
	public bool status;

	void OnClick()
	{
		//popup.OnPress(align);
		popup.Confirm(status);
	}
}
