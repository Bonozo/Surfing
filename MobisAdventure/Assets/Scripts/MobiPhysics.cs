using UnityEngine;
using System.Collections;

public class MobiPhysics : MonoBehaviour {
	
	private float smooth = 20f;
	private float random = 5f;
	private float minRotate = -12f, maxRotate=6f;
	private Transform follow;
	
	void Start()
	{
		follow = PlayerController.Instance.transform;
	}
	
	void Update()
	{
		var rt = follow.localRotation.eulerAngles.x;
		if(rt>180f) rt-=360f;
		
		rt += Random.Range(-random,random);
		rt = Mathf.Clamp(-rt,minRotate,maxRotate);
		var cur = transform.localRotation.eulerAngles.z;
		if(cur>180f) cur-=360f;
		cur = Mathf.Lerp(cur,rt,smooth*Time.deltaTime);
		//cur += smooth*Time.deltaTime*(cur>rt?-1f:1f);
		
		var eulerrot = transform.localRotation.eulerAngles;
		eulerrot.z = cur;
		transform.localRotation = Quaternion.Euler(eulerrot);
	}
}
