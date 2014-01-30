using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

	public Vector3 speed;
	public bool random = false;
	public Vector3 minRange,maxRange;

	void Awake()
	{
		if (random) {
			speed.x = Random.Range(minRange.x,maxRange.x);
			speed.y = Random.Range(minRange.y,maxRange.y);
			speed.z = Random.Range(minRange.z,maxRange.z);
		}
	}

	void Update()
	{
		transform.Rotate(speed*Time.deltaTime);
	}
}
