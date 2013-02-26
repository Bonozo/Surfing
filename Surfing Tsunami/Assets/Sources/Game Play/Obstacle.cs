using UnityEngine;
using System.Collections;

public class Obstacle : SpawnedItem {
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		tag = "Obstacle";
	}
	
	void OnTriggerEnter(Collider col)
	{
		if( col.gameObject.tag == "Player")
		{
			iTween.ShakeRotation(this.gameObject,new Vector3(2,2f,0f),1f);
			var v = transform.position-col.gameObject.transform.position;
			v.z=0;
			AddForce( v.normalized*LevelInfo.State.PlayerForce );
		}
	}
	
	protected override void Update ()
	{
		base.Update();
	}
}
