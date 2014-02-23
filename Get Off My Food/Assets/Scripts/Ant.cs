using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {

	private float speed;
	private bool died = false;

	void Start()
	{
		float dist = 800f;
		float alpha = Random.Range (0f, 360f);
		float x = dist * Mathf.Cos (alpha * Mathf.PI / 180f);
		float y = dist * Mathf.Sin (alpha * Mathf.PI / 180f);
		Debug.Log ("x:" + x + ",y: " + y);

		transform.parent = LevelInfo.Instance.transform;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		transform.localPosition = new Vector3 (x, y, 0f);
		transform.localRotation = Quaternion.Euler (0f, 0f, alpha);

		int wave = LevelInfo.Instance.CurrentWave;
		float minSpeed = 75f + 5f * wave;
		float maxSpeed = 125f + 10f * wave;
		speed = Random.Range (minSpeed, maxSpeed);

		GetComponentInChildren<UISprite> ().spriteName = "ant" + Random.Range (1, 4);
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
		GetComponentInChildren<UISprite> ().color = new Color (0.5f, 0.5f, 0.5f, 1f);
		yield return new WaitForSeconds (1f);
		TweenAlpha.Begin (transform.GetChild (0).gameObject, 1f, 0f);
		Destroy (this.gameObject, 1f);

		//TweenScale.Begin (transform.GetChild (0).gameObject, 0.1f, transform.localScale*1.15f);
		//yield return new WaitForSeconds (0.1f);
		//TweenScale.Begin (transform.GetChild (0).gameObject, 0.5f, Vector3.zero);


	}

}
