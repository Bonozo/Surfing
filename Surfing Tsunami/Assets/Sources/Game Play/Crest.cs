using UnityEngine;
using System.Collections;


public class Crest : MonoBehaviour {
	
	private float dieheight = 220f;
	
	void Start()
	{
		transform.position = new Vector3(Random.Range(0f,LevelInfo.Camera.Width),-20f,145f);
		transform.parent = LevelInfo.Environments.transformObstacles;
	}
	
	void Update()
	{
		if(!LevelInfo.State.IsAnimatedState) return;
		float my = LevelInfo.State.MaxForce*Time.deltaTime;
		float mx = 0f;
		if( LevelInfo.State.wind == WindState.Left || LevelInfo.State.wind == WindState.DownLeft) mx=0.1f*my;
		if( LevelInfo.State.wind == WindState.Right || LevelInfo.State.wind == WindState.DownRight) mx=-0.1f*my;
		transform.Translate(mx,my,0);
		if(transform.position.y>=dieheight) Destroy(this.gameObject);
	}
	

}
