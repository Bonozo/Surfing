/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {

	void Update()
	{
		transform.rotation = PlayerController.Instance.transform.rotation;
		transform.Rotate(0f,-90f,0f);
	}
}
