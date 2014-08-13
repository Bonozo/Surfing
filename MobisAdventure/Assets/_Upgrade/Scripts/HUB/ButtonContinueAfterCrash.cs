using UnityEngine;
using System.Collections;

public class ButtonContinueAfterCrash : MonoBehaviour {

	void OnClick(){

		if(MobiIAB.debug){
			//DeathScreen.Instance.ShowMessage("Purchasing this item in debug mode");
			PlayerController.Instance.ResumeGameAfterCrashing ();
		}
		else{
			StartCoroutine(BuySpecialOffer());
		}
	}

	IEnumerator BuySpecialOffer(){
		MobiIAB.Instance.RequestProductData();
		DeathScreen.Instance.messageBox.ShowLoading(true);
		float tm = RealTime.time + 3f;
		while(tm>RealTime.time && !MobiIAB.Instance.Connected)
			yield return new WaitForEndOfFrame();

		if(!MobiIAB.Instance.Connected){
			DeathScreen.Instance.messageBox.ShowLoading(false);
			DeathScreen.Instance.ShowMessageAndDoNothingMore("Unable to connect to the Server.");
		}
		else{
			IAP.purchaseConsumableProduct( "hkr_offer1", ( didSucceed, error ) => {
				if( !didSucceed ){
					DeathScreen.Instance.messageBox.ShowLoading(false);
					DeathScreen.Instance.messageBox.Show("Failed to purchase!");
				} else{
					DeathScreen.Instance.messageBox.ShowLoading(false);
					PlayerController.Instance.ResumeGameAfterCrashing ();
				}
			});
		}
	}

	void Update()
	{
		collider.enabled = !DeathScreen.Instance.messageBox.gameObject.activeSelf;
	}
}
