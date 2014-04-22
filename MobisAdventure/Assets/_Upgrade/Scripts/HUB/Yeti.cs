using UnityEngine;
using System.Collections;

public class Yeti : MonoBehaviour {

	public float scal = 0.1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var pos = PlayerController.Instance.transform.position;
		pos.x -= PlayerController.Instance.monsterDistance * scal;
		//pos.x = Camera.main.ViewportToWorldPoint (new Vector3 (0f, pos.y, pos.z)).x;

		float newHeight;
		Path.m_path.GetHeight(transform.position, out newHeight);
		pos.y = newHeight;

		transform.position = pos;
	}
}
