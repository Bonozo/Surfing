using UnityEngine;
using System.Collections;

public class Yeti : MonoBehaviour {
	
	private float speed = 12f;
	private float speedUpInterval = 25f;
	private float speedUpDelta = 1f;

	private float appearSpeed = 12f;
	private float appearSpeedUpDelta = 0.5f;
	private float chaseSpeed = 3f;
	private float chaseSpeedUpDelta = 1f;
	private float maxChaseSpeed = 11f;

	private float distance = 10f;
	//private float begginingAppearTime = 5f;

	private float boostAlertDistance = 50f;
	private float playerDeathDistance = 2.5f;

	private bool started = false;
	public bool boost{ get; set; }

	public float Distance{ get { return PlayerController.Instance.transform.position.x - playerDeathDistance - distance; } }
	public bool IsIntro { get { return !started; } }

	private ButtonBoost buttonBoost;
	private GameObject monsterT;

	void Awake()
	{
		buttonBoost = GameObject.FindObjectOfType<ButtonBoost> ();
		monsterT = transform.parent.FindChild ("Monster").gameObject;
		if(monsterT == null) Debug.Log("why is null?");
		boost = false;

		var pos = transform.position;
		pos.z = 6;
		transform.position = pos;
	}

	void Start()
	{
		StartCoroutine ("MonsterAI");
		StartCoroutine ("MonsterBoost");
	}

	IEnumerator MonsterAI()
	{
		yield return new WaitForSeconds (0.25f);
		monsterT.audio.Play ();
		// First plan
		float ttime = 0f;
		while( ttime < 2f)
		{
			ttime += Time.deltaTime;
			float mmin = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f)).x - 4f;
			distance = mmin + ttime*3f;
			SetupHeight();
			yield return null;
		}
		ttime = 0f;
		while(ttime < 3f)
		{
			ttime += Time.deltaTime;
			float mmin = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f)).x - 4f;
			distance = mmin + 6f;
			SetupHeight();
			yield return null;
		}

		started = true;

		while(PlayerController.Instance.life)
		{
			yield return new WaitForSeconds(speedUpInterval);
			speed += speedUpDelta;
			appearSpeed += appearSpeedUpDelta;
			chaseSpeed += chaseSpeedUpDelta;
			if(chaseSpeed > maxChaseSpeed) chaseSpeed = maxChaseSpeed;
		}
	}

	private float nextBoostDistance = 1400f;
	private float nextBoostDelta = 1500f;
	IEnumerator MonsterBoost()
	{
		while(true)
		{
			if( distance >= nextBoostDistance)
			{
				boost = true;
				nextBoostDistance += nextBoostDelta;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	void Update()
	{
		// Audio
		if( PlayerController.Instance.life)
		{
			float dist = Distance;
			MusicLoop.Instance.audio.volume = 0.05f + Mathf.Clamp (Distance * 0.1f, 0f, 0.5f);
			monsterT.audio.volume = 2f * (0.55f - MusicLoop.Instance.audio.volume);
		}
	}

	void FixedUpdate () 
	{
		if(!started) return;

		float currentSpeed = speed;
		float currentAppearSpeed = appearSpeed;
		float currentChaseSpeed = chaseSpeed;
		if(boost)
		{
			currentSpeed *= 2f;
			currentAppearSpeed *= 2f;
			currentChaseSpeed *= 2f;
		}

		float mmin = Camera.main.ScreenToWorldPoint (new Vector3 (0f, 0f, 0f)).x - 4f;

		if(distance + currentSpeed * Time.deltaTime >= mmin) // yeti is appearing
		{
			float mmax = PlayerController.Instance.transform.position.x-2.5f;
			currentSpeed = currentAppearSpeed-(currentAppearSpeed-currentChaseSpeed)*(distance-mmin)/(mmax-mmin);
		}

		distance += currentSpeed * Time.deltaTime;
		if(distance > PlayerController.Instance.transform.position.x - playerDeathDistance)
		{
			PlayerController.Instance.Death(true);
			distance = PlayerController.Instance.transform.position.x - playerDeathDistance;
		}

		// Alert: Need Boost
		buttonBoost.Animate(boost && Distance < boostAlertDistance);

		if(distance < mmin)
			transform.position.Set(distance,0f,transform.position.z);
		else
			SetupHeight ();
	}

	void SetupHeight()
	{
		Vector3 pos = transform.position;
		pos.x = distance;
		Path.m_path.GetHeight(transform.position, out pos.y);
		pos.y += 3f;
		
		RaycastHit hit;
		Physics.Raycast (pos, Vector3.down, out hit);
		
		pos.y -= hit.distance;
		transform.position = pos;
	}
}
