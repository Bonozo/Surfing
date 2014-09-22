/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class FailedScreen : MonoBehaviour {

	public UILabel textCause;
	public UISprite iconTarget;
	public UILabel textTarget;
	public UISprite iconGoal;
	public UILabel textGoal;

	public void Show(bool monstercause,int distance,int best,int score){

		//if(monstercause) textCause.text = "THE MONSTER CAUGHT YOU!";
		//else textCause.text = "YOU CRASHED!";
		textCause.text = "YOU FAILED!";

		iconTarget.spriteName = DeathScreen.Instance.levelController.ReachTarget ? "checkmark" : "cross";
		textTarget.text = "Reach Target " + MainMenu.PutCommas (DeathScreen.Instance.levelController.InitialTarget) + "m";
		iconGoal.spriteName = (distance>=DeathScreen.Instance.levelController.GoalDistance) ? "checkmark" : "cross";
		textGoal.text = "Reach Goal 10,000m";

		gameObject.SetActive (true);
	}

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile FailedScreen _staticInstance;	
	public static FailedScreen Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(FailedScreen)) as FailedScreen;
					if (_staticInstance == null) {
						Debug.LogError("The FailedScreen instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
