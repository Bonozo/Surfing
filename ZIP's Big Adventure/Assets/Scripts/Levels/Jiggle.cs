using UnityEngine;
using System.Collections;

// Jiggle Sprite depends on JumbleDrag Object
public class Jiggle : MonoBehaviour {

	private JumbleDrag jumbleDrag;
	private float angle = 0.05f;
	private float speed = 5f;

	void Awake(){
		jumbleDrag = GetComponent<JumbleDrag> ();
	}

	void OnEnable(){
		transform.rotation = Quaternion.identity;
		if(jumbleDrag != null)
			StartCoroutine (JiggleThread ());
	}

	private float lastAngle = 0;
	IEnumerator JiggleThread(){
		//yield return new WaitForEndOfFrame ();
		while(true){
			var rot = transform.rotation;
			rot.z = angle*Mathf.Cos(speed*Time.time);
			if(jumbleDrag.Completed && rot.z*lastAngle<=0f){
				transform.rotation = Quaternion.identity;
				break;
			}
			transform.rotation = rot;
			lastAngle = transform.rotation.z;
			yield return null;
			/*Debug.Log("z: " + Z);
			while(Z < angle ){
				transform.Rotate(0f,0f,speed*Time.deltaTime);
				yield return null;
			}
			yield return new WaitForSeconds (delay);
			while(Z > -angle ){
				transform.Rotate(0f,0f,-speed*Time.deltaTime);
				yield return null;
			}
			yield return new WaitForSeconds (delay);*/
		}	                              
	}

	/*private float Z{
		get{
			float z = transform.rotation.eulerAngles.z; if(z>180f) z-=360f;
			return z;
		}
	}*/
}
