/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class Speedometer : MonoBehaviour {

	private float smoothPeriod = 0.03f;
	void Update()
	{
		var speed = PlayerController.Instance.Speed;
		var newrot = Quaternion.Euler(0f, 0f, 90f - speed * 90f / 50f);
		transform.rotation = Quaternion.Slerp(transform.rotation,newrot,smoothPeriod);
	}
}
