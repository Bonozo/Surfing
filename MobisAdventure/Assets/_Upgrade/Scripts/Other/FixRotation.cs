using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {

	void Update()
	{
		transform.rotation = transform.root.rotation;
		transform.Rotate(0f,-90f,0f);
	}
}
