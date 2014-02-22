using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {

	private float speed;
	private bool died = false;

	void Start()
	{
		Debug.Log ("new ant");
		float dist = 800f;
		float alpha = Random.Range (0f, 360f);
		float x = dist * Mathf.Sin (alpha);
		float y = dist * Mathf.Cos (alpha);

		transform.parent = LevelInfo.Instance.transform;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		transform.localPosition = new Vector3 (x, y, 0f);

		int wave = LevelInfo.Instance.CurrentWave;
		float minSpeed = 75f + 5f * wave;
		float maxSpeed = 125f + 10f * wave;
		speed = Random.Range (minSpeed, maxSpeed);
	}

	void Update()
	{
		if(!died && transform.localPosition.magnitude > 85f)
		{
			var dir = -1f*transform.localPosition.normalized;
			transform.localPosition += dir*speed*Time.deltaTime;
		}
	}

	void OnPress(bool isDown)
	{
		if(!died && isDown)
		{
			died = true;
			StartCoroutine (Die ());
		}
	}

	IEnumerator Die()
	{
		collider.enabled = false;
		LevelInfo.Instance.AddAnt ();
		TweenScale.Begin (transform.GetChild (0).gameObject, 0.1f, transform.localScale*1.15f);
		yield return new WaitForSeconds (0.1f);
		TweenScale.Begin (transform.GetChild (0).gameObject, 0.5f, Vector3.zero);
		Destroy (this.gameObject, 1f);

	}

}
