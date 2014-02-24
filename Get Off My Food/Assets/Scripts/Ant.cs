using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {

	private float speed;
	private bool died = false;
	private Vector3 beginPos;

	void Start()
	{
		float dist = 800f;
		float alpha = Random.Range (0f, 360f);
		float x = dist * Mathf.Cos (alpha * Mathf.PI / 180f);
		float y = dist * Mathf.Sin (alpha * Mathf.PI / 180f);
		beginPos = new Vector3 (x, y, 0f);

		transform.parent = LevelInfo.Instance.transform;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		transform.localPosition = beginPos;
		transform.localRotation = Quaternion.Euler (0f, 0f, alpha);

		int wave = LevelInfo.Instance.CurrentWave;
		float minSpeed = 0.3f + 0.05f * wave;
		float maxSpeed = 0.5f + 0.05f * wave;
		speed = Random.Range (minSpeed, maxSpeed);

		GetComponentInChildren<UISprite> ().spriteName = "ant" + Random.Range (1, 4);
	}

	void Update()
	{
		if(!died && transform.localPosition.magnitude > 85f)
		{
			var dist = Vector3.Distance(transform.localPosition,beginPos);
			LookTo(Vector3.zero,Mathf.Sin(dist*0.05f)*15);
			transform.position += transform.up * speed * Time.deltaTime; 
		}
	}

	/*void OnPress(bool isDown)
	{
		if(!died && isDown)
		{
			died = true;
			StartCoroutine (Die ());
		}
	}*/

	IEnumerator Die()
	{
		//collider.enabled = false;
		LevelInfo.Instance.AddAnt ();
		GetComponentInChildren<UISprite> ().color = new Color (0.5f, 0.5f, 0.5f, 1f);
		yield return new WaitForSeconds (1f);
		TweenAlpha.Begin (transform.GetChild (0).gameObject, 1f, 0f);
		Destroy (this.gameObject, 1f);

		//TweenScale.Begin (transform.GetChild (0).gameObject, 0.1f, transform.localScale*1.15f);
		//yield return new WaitForSeconds (0.1f);
		//TweenScale.Begin (transform.GetChild (0).gameObject, 0.5f, Vector3.zero);


	}

	private void LookTo(Vector3 target,float extraAngle)
	{
		Vector3 diff = target - transform.position;
		diff.Normalize();
		
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
		transform.Rotate (0f, 0f, extraAngle, Space.Self);
	}

	public void AddForce(Vector2 delta)
	{
		if(!died)
		{
			died = true;
			rigidbody2D.AddForce(delta*5f);
			StartCoroutine(Die());
		}
	}

	public void Squash()
	{
		if(!died)
		{
			died = true;
			StartCoroutine(Die());
		}
	}
}
