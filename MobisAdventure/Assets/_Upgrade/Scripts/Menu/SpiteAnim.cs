using UnityEngine;
using System.Collections;

public class SpiteAnim : MonoBehaviour {

	public string[] spriteName;
	public float timeout = 0.1f;
	public float startDelay = 0.5f;
	public float pauseDelay = 2f;

	private UISprite sprite;

	void OnEnable(){
		sprite = GetComponent<UISprite> ();
		sprite.spriteName = spriteName[0];
		StartCoroutine ("Thread");
	}

	IEnumerator Thread(){
		yield return new WaitForSeconds(startDelay);
		while(true){
			for(int i=1;i<spriteName.Length;i++){
				sprite.spriteName = spriteName[i];
				yield return new WaitForSeconds(timeout);
			}
			sprite.spriteName = spriteName[0];
			yield return new WaitForSeconds(pauseDelay);
		}
	}
}
