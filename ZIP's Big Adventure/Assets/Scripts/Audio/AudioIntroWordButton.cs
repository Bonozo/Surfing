using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(BoxCollider))]
public class AudioIntroWordButton : MonoBehaviour {

	public AudioClip clip;
	public float delay = 4f;
	private bool ended = false;
	IEnumerator Start(){
		if(clip!=null){
			yield return new WaitForSeconds(delay);
			AudioManager.Instance.PlayClip(clip);
			ended = true;
		}
	}

	void OnClick(){
		if(ended && clip!=null)
			AudioManager.Instance.PlayClip(clip);
	}
}
