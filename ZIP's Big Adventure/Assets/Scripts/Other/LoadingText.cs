using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class LoadingText : MonoBehaviour {

	public float speed = 0.2f;

	IEnumerator Start(){

		int ind = 0;
		UILabel label = GetComponent<UILabel> ();

		while(true){
			yield return new WaitForSeconds(speed);
			label.text = "LOADING" + (ind==0?"":ind==1||ind==5?".":ind==2||ind==4?"..":"...");
			ind = ++ind%6;
		}
	}
}
