using UnityEngine;
using System.Collections;

public class Surfer : MonoBehaviour {
	
	#region References
	
	public tk2dAnimatedSprite board;
	public ParticleSystem waterSpray;
	
	#endregion
	
	#region Public Attributes
	
	[System.NonSerialized]
	public float distanceTravelled=0f;
	
	[System.NonSerialized]
	public int coins = 0;
	
	[System.NonSerialized]
	public int lives = 3;
	
	#endregion
	
	#region Private Attributes
	
	private tk2dAnimatedSprite surferSprite;
	
	private int invincibility = 0;
	private bool jump = false;
	
	private bool _upvelocity = false;
	private bool upvelocity{
		get{
			return _upvelocity;
		}
		set{
			if(_upvelocity != value )
			{
				_upvelocity = value;
				PlayCurrentAnimation();
			}
		}
	}
	
	#endregion
	
	#region Properties
	
	public bool Invincibility { get { return invincibility>0; } }
	
	public Vector3 Position { get { return transform.position; } }

	#endregion
	
	#region OnEnable, OnDisable, Input Events
	
	void OnEnable()
	{
		FingerGestures.OnFingerDown += HandleFingerGesturesOnFingerDown;
		FingerGestures.OnFingerMove += HandleFingerGesturesOnFingerMove;
	}
	
	void OnDisable()
	{
		FingerGestures.OnFingerDown -= HandleFingerGesturesOnFingerDown;
		FingerGestures.OnFingerMove -= HandleFingerGesturesOnFingerMove;
	}
	
	void HandleFingerGesturesOnFingerDown (int fingerIndex, Vector2 fingerPos)
	{
		if( LevelInfo.State.state == GameState.Play )
		{
			if(PlayerTape(fingerPos))
				Jump();
			else
				AddForceToPlayer(fingerPos);
		}
	}
	
	void HandleFingerGesturesOnFingerMove (int fingerIndex, Vector2 fingerPos)
	{
		if( LevelInfo.State.state == GameState.Play )
			AddForceToPlayer(fingerPos);		
	}
	
	void AccelerationMove()
	{
		/*if( LevelInfo.State.state == GameState.Play && Option.tiltMove)
		{
			// get device acceleration
			Vector3 dir = Vector3.zero;
			dir.x = -Input.acceleration.y;
			dir.z = Input.acceleration.x;
			
			if(dir.sqrMagnitude > 1)
				dir.Normalize();
			dir.y = -dir.x;
			dir.x = dir.z;
			dir.z = 0;
			
			// update player
			if(Mathf.Abs(dir.x) >= 0.1f )
			{
				rigidbody.AddForce(Vector3.right*dir.x);
				upvelocity = rigidbody.velocity.y>0;
			}
		}*/
		if( LevelInfo.State.state == GameState.Play && Option.tiltMove)
		{
			var dir = AccelerationWithCalibrate.Acceleration;
			// update player
			if(Mathf.Abs(dir.x) >= 0.1f )
				rigidbody.AddForce(Vector3.right*dir.x);
			if(Mathf.Abs(dir.y) >= 0.1f )
				rigidbody.AddForce(Vector3.up*dir.y);
			upvelocity = rigidbody.velocity.y>0;
		}
	}
	
	private void AddForceToPlayer(Vector2 fingerPos)
	{
		fingerPos = LevelInfo.Camera.level.transform.worldToLocalMatrix * LevelInfo.Camera.level.ScreenToWorldPoint( fingerPos );
		var v = new Vector3(fingerPos.x,fingerPos.y,LevelInfo.Environments.depthSurfer);
		rigidbody.AddForce( (v-transform.position).normalized);	
		upvelocity = rigidbody.velocity.y>0;
	}
		
	private bool PlayerTape(Vector2 finger)
	{
		finger = LevelInfo.Camera.level.transform.worldToLocalMatrix * LevelInfo.Camera.level.ScreenToWorldPoint( finger );
		float w = surferSprite.GetBounds().size.x*0.5f;
		float h = surferSprite.GetBounds().size.y*0.5f;
		var p = transform.position;
		return p.x-w<=finger.x && finger.x<=p.x+w && p.y-h<=finger.y && finger.y<=p.y+h;
	}
	
	#endregion
	
	#region Awake, Start
	
	void Awake()
	{
		surferSprite = GetComponent<tk2dAnimatedSprite>();
	}
	
	void Start()
	{
		Reset();
	}	
	
	#endregion
	
	#region Update
	
	void Update()
	{		
		if(Input.GetKeyUp(KeyCode.J))
		{
			StartCoroutine(StartInvincibilityPowerup());
		}
		
		if(LevelInfo.State.state != GameState.Play ) return;
		
		FlashUpdates();
		PowerupMessageUpdates();
		UpdateDistanceTravelled();
		AccelerationMove();

		AccuratePosition();

		var v = transform.localScale;
		v.x = Mathf.Abs(v.x)*(rigidbody.velocity.x<0?-1:1);
		transform.localScale = v;
	}
	
	private Vector3 lastPosition;
	void UpdateDistanceTravelled()
	{
		distanceTravelled += 0.01f*Vector3.Distance(lastPosition,transform.position);
		lastPosition = transform.position;
	}
	
	void AccuratePosition()
	{
		float w = surferSprite.GetBounds().size.x*0.5f;
		float h = surferSprite.GetBounds().size.y*0.5f;
		
		var pos = transform.position;
		pos.x = Mathf.Clamp(pos.x,w,LevelInfo.Camera.Width-w);
		pos.y = Mathf.Clamp(pos.y,h,LevelInfo.Camera.Height-h);
		transform.position = pos;
	}
	
	#endregion
	
	#region Collision Detection
	
	void OnTriggerEnter(Collider col)
	{
		if( col.gameObject.tag == "Collectable" )
		{
			coins++;
			LevelInfo.Audio.audioPlayer.PlayOneShot(LevelInfo.Audio.clipCollectable);
			Destroy(col.gameObject);
		}
		else if( col.gameObject.tag == "Obstacle" )
		{
			LevelInfo.Audio.audioPlayer.PlayOneShot(LevelInfo.Audio.clipBump);
			rigidbody.velocity *= 0.1f;
			
			if( !Option.UnlimitedHealth && !Invincibility && lives>0)
			{
				lives--;
				if(lives>0)
				{
					StartFlash();
				}
				else
					LevelInfo.State.state = GameState.WipeOut;
			}
			Instantiate(LevelInfo.Environments.prefabWaterSpray,transform.position,Quaternion.identity);

		}
		else if( col.gameObject.tag == "Powerup" )
		{
			switch(col.gameObject.GetComponent<Powerup>().powerupType)
			{
			case Powerups.FillLives:
				lives = 3;
				break;
			case Powerups.Invincibility:
				ShowPowerupMessage(1.0f);
				StartCoroutine(StartInvincibilityPowerup());
				break;
			}
			Destroy(col.gameObject);
		}
	}	
	
	#endregion
	
	#region Surfer Methods
	
	public void Reset()
	{
		StopAllCoroutines();
		
		distanceTravelled = 0.0f;
		coins = 0;
		lives = 3;
		invincibility = 0;
		invincibilityDrinking = 0;
		
		ClearFlash();
		waterSpray.Clear();
		waterSpray.enableEmission = true;
			
		transform.position = new Vector3(LevelInfo.Camera.Width*0.5f,LevelInfo.Camera.Height-surferSprite.GetBounds().size.y-50f,LevelInfo.Environments.depthSurfer);
		lastPosition = transform.position;
		rigidbody.velocity = Vector3.zero;
		
		surferSprite.color = Color.white;
		PlayAnimation("down");
	}
	
	
	public void WipeOut()
	{
		StartCoroutine(WipeOutThread());
	}
	
	private IEnumerator WipeOutThread()
	{	
		LevelInfo.Audio.PauseLoop();
		LevelInfo.Audio.audioPlayer.PlayOneShot(LevelInfo.Audio.clipWipeOut);
		
		waterSpray.enableEmission = false;
		PlayAnimation("wipeout");
		yield return new WaitForSeconds(3f);
		LevelInfo.State.state = GameState.Scoreboard;
	}
	
	public void Jump()
	{
		if(!jump&&invincibility==0)
			StartCoroutine(JumpThread());
	}
	
	private IEnumerator JumpThread()
	{
		jump = true;
		invincibility++;
		waterSpray.enableEmission = false;
		PlayAnimation("360");
		while(surferSprite.Playing)
			yield return null;
		waterSpray.enableEmission = true;
		invincibility--;
		jump=false;
		PlayCurrentAnimation();
	}
	
	float invincibilityTime = 0f;
	int invincibilityDrinking = 0;
	private IEnumerator StartInvincibilityPowerup()
	{
		invincibility++;
		
		LevelInfo.Audio.audioPlayer.PlayOneShot(LevelInfo.Audio.clipPowerupInvincibility);	
		
		invincibilityDrinking++;
		PlayAnimation("ED");
		yield return new WaitForSeconds(3.5f);
		invincibilityDrinking--;
		if(invincibilityDrinking==0)
			PlayAnimation("EDS");
		invincibilityTime = 6.5f;
		while(invincibilityTime>=0f)
		{
			invincibilityTime -= Time.deltaTime;
			yield return null;
		}		
		invincibility--;
		if(invincibility==0) PlayCurrentAnimation();
	}
	
	#endregion
	
	#region PowerupMessage
	
	float powerupMessageTime = 0.0f;
	void PowerupMessageUpdates()
	{
		if(powerupMessageTime>0f) powerupMessageTime=Mathf.Clamp(powerupMessageTime-Time.deltaTime,0f,float.PositiveInfinity);
		LevelInfo.Environments.powerupMessage.SetActive(powerupMessageTime>0f);
	}
	
	void ShowPowerupMessage(float time)
	{
		powerupMessageTime = time;
	}
	
	#endregion
	
	#region Flush

	float flash = 0.0f;
	float flashTime = 0.0f;
	void FlashUpdates()
	{
		if (flash > 0f) 
		{
			flashTime += Time.deltaTime;
			flashTime = flashTime % 0.2f;
			surferSprite.color = (flashTime > 0.1f)?Color.red:Color.white;
			if( (flash -= Time.deltaTime) <= 0f ) surferSprite.color = Color.white;
		}		
	}
	
	private void ClearFlash()
	{
		flash = 0f;
	}
	
	public void StartFlash()
	{
		flash = 0.2f;
		flashTime = 0.0f;
	}
	
	#endregion
	
	#region Animations
	
	private void PlayCurrentAnimation()
	{
		if(jump || invincibility>0 ) return;

		if(upvelocity)
			PlayAnimation("up");
		else
			PlayAnimation("down");
	}
	
	private void PlayAnimation(string animName)
	{
		surferSprite.Play("surfer2_" + animName);	
		board.Play("board1_" + animName);	
	}
	
	
	#endregion
	
	#region Editor + Tests
	
	IEnumerator SaveScreenShot()
	{
		Debug.Log("start");
		yield return new WaitForSeconds(2f);
		Application.CaptureScreenshot("D:\\1.png");
		yield return new WaitForSeconds(0.5f);
		Application.CaptureScreenshot("D:\\2.png");
		yield return new WaitForSeconds(0.5f);
		Application.CaptureScreenshot("D:\\3.png");
		yield return new WaitForSeconds(0.5f);
		Application.CaptureScreenshot("D:\\4.png");
	}
	
	#endregion
}
