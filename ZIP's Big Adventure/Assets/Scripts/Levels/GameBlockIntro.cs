using UnityEngine;
using System.Collections;

public class GameBlockIntro : MonoBehaviour {
	
	public iTween.EaseType easyType1 = iTween.EaseType.linear;
	public iTween.EaseType easyType2 = iTween.EaseType.linear;
	public AudioClip clipSpeech;
	private Vector3 from = new Vector3 (-2500f, 0f, 0f);
	private Vector3 to = new Vector3(2500f,0f,0f);
	public float dur = 2f;

	public void Reset()
	{
		transform.localPosition = from;
	}

	public IEnumerator ComeAround()
	{
		iTween.Stop (gameObject, false);
		transform.localPosition = from;
		iTween.MoveTo(gameObject,iTween.Hash("position",Vector3.zero,"easetype",easyType1,"time",dur,"islocal",true));
		yield return new WaitForSeconds (dur + 0.1f);
		AudioManager.Instance.PlayClip (clipSpeech);
	}

	public IEnumerator GoAway()
	{
		GetComponentInChildren<UIButton> ().collider.enabled = false;
		iTween.Stop (gameObject, false);
		iTween.MoveTo(gameObject,iTween.Hash("position",to,"easetype",easyType2,"time",dur,"islocal",true));
		yield return new WaitForSeconds (dur + 0.1f);
	}
}
