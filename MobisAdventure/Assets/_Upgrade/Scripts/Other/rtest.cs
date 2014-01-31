using UnityEngine;
using System.Collections;

public class rtest : MonoBehaviour {

	/*public float a=500,b=1;

	void Update(){
		var pos = transform.localPosition;
		pos.x = a*Mathf.Sin(b*Time.time);
		transform.localPosition = pos;
	}*/

	public float forceX=100f,forceY;
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			rigidbody.AddForce(forceX,forceY,0f);
	}
}
