/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class ButtonInviteFriends : MonoBehaviour {

	void Update()
	{
		collider.enabled = !MainMenu.Instance.isPopupActive;
		//transform.localPosition = new Vector3 (0f, -110f * FacebookAdvanced.Instance.leaderboardTotal - 20f, 0f);
	}

	void OnClick(){
		FacebookAdvanced.Instance.InvitePlayer ();
	}
}
