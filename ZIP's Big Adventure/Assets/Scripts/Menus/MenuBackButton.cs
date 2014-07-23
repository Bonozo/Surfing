using UnityEngine;
using System.Collections;

public class MenuBackButton : ButtonToggle {

	protected override sealed void OnClick(){
		Title.firstLaunch = true;
		AudioManager.Instance.Stop ();
		base.OnClick ();
	}
}
