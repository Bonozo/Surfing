using UnityEngine;
using System.Collections;

public class BonusAnimationScript : MonoBehaviour {
	
	void OnEnable()
	{
		StartCoroutine (ShowBonus ());
	}
	
	IEnumerator ShowBonus()
	{
		yield return new WaitForEndOfFrame();
		animation ["BonusText"].speed = 0.000001f;
		yield return new WaitForSeconds(0.25f);
		animation ["BonusText"].speed = 0.8f;
		while(animation.isPlaying)
			yield return new WaitForEndOfFrame();
		gameObject.SetActive (false);
	}

	/*void Update () 
	{
		if( !this.animation.isPlaying )
			this.gameObject.SetActive( false );
	
	}*/
}
