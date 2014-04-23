using UnityEngine;
using System.Collections;

public class Yeti : MonoBehaviour {
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.touchCount==4)
			enabled = false;

		var pos = transform.position;
		pos.x = PlayerController.Instance.transform.position.x - PlayerController.Instance.monsterDistance * 0.1f;
		transform.position = pos;

		float newHeight;
		Path.m_path.GetHeight(transform.position, out newHeight);
		pos.y = newHeight + 10f;

		RaycastHit hit;
		Physics.Raycast (pos, Vector3.down, out hit);

		pos.y -= hit.distance;
		transform.position = pos;
	}
}
