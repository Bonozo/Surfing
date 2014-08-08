using UnityEngine;
using System.Collections;

public class ButtonContinueAfterCrash : MonoBehaviour {

	void OnClick(){

		if(MobiIAB.debug){
			//DeathScreen.Instance.ShowMessage("Purchasing this item in debug mode");
			PlayerController.Instance.ResumeGameAfterCrashing ();
		}
		else{
			DeathScreen.Instance.messageBox.Show("Not determined in real");
		}
	}

	void Update()
	{
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}
}
