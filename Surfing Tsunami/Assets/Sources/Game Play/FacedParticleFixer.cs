using UnityEngine;
using System.Collections;

public class FacedParticleFixer : MonoBehaviour {

	float dist;
	void Start()
	{
		dist = transform.localPosition.z;
	}

	void Update()
	{
		var lp = transform.localPosition;
		bool cnd = Mathf.Abs (transform.parent.localRotation.eulerAngles.y) < 0.1f;
		lp.z = cnd?dist:-dist;
		transform.localPosition = lp;
	}
}
