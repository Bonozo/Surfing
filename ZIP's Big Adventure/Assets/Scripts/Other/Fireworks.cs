using UnityEngine;
using System.Collections;

public class Fireworks : MonoBehaviour {

	public GameObject[] particles;
	public Vector3 rangeBound;
	public float rateMin,rateMax;
	public float startDelayRate;

	bool work = false;
	float nextTime;

	void OnEnable()
	{
		work = true;
		nextTime = Random.Range(0.1f,startDelayRate);
	}

	void OnDisable()
	{
		work = false;
		foreach (Transform particle in transform)
			Destroy (particle.gameObject);
	}

	void Update()
	{
		if (work)
		{
			nextTime -= Time.deltaTime;
			if(nextTime <= 0f)
			{
				nextTime = Random.Range(rateMin,rateMax);
				GameObject part = Instantiate(particles[Random.Range(0,particles.Length)]) as GameObject;
				part.transform.parent = transform;
				Vector3 partpos = Vector3.zero;
				partpos.x = Random.Range(-rangeBound.x,rangeBound.x);
				partpos.y = Random.Range(-rangeBound.y,rangeBound.y);
				part.transform.localPosition = partpos;
			}
		}
	}
}
