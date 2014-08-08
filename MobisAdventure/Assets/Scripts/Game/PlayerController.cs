using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{
	public bool s_pause = false;
	
	public WheelCollider m_backWheel = null;
	public WheelCollider m_frontWheel = null;
	
	//	public float m_maxSpeed = 300.0f;
	//	public float m_maxBraking = 150.0f;
	public float m_speed = 300.0f;
	public float m_break = 300.0f;
	public float m_airSpeed = 100.0f;
	public float m_accelThresh = 8.0f;
	public float m_boost = 10.0f; //multiplyer for speed
	private int s_gas = 0;

	[HideInInspector]
	public bool _isGrounded = true;
	[HideInInspector]
	public bool isGas = false;
	[HideInInspector]
	public bool isBoost = false;
	
	private Texture2D m_bar = null;
	
	private bool m_init = true;
	
	//AUDIO
	//	public AudioSource a_doom;
	public AudioClip[] a_monsterHowel;
	public AudioClip[] a_monsterGrunt;
	public AudioClip[] a_monsterAttack;
	
	//MONSTER
	public bool life = true;
	
	//collider settings
	private Vector3 classic_cc = new Vector3(0f, 1.15f, -.48f);
	private Vector3 turbo_cc = new Vector3(0f, 1.15f, -.18f);
	private Vector3 track_cc = new Vector3(0f, 1.15f, -.39f);
	private Vector3 sport_cc = new Vector3(0f, 1.3f, -.27f);
	private Vector3 hybrid_cc = new Vector3(0f, 1.3f, -.21f);
	
	private Vector3 p_startPosition;
	public float p_distTraveled;
	
	public float p_maxHeight;

	public TextMesh tm_dist;
	public TextMesh tm_distHigh;
	
	//dark screen
	public Transform sfx_frostObj;
	private Color sfx_frostColor;
	
	public Transform chaseBar;
	private float chaseBar_scale;
	//pause flag
	
	private bool pause_flag = false;
	Vector3 savedVelocity;
	Vector3 savedAngularVelocity;
	
	// by Aharon
	// messing up the code that had already messed up
	[System.NonSerialized][HideInInspector]
	public bool tilt = false;
	private Boost boost;
	private Yeti yeti;
	
	[System.NonSerialized][HideInInspector]
	public Ragdoll ragdoll;
	[System.NonSerialized][HideInInspector]
	public GameObject bike;
	
	void Awake()
	{
		m_backWheel.radius = 0.74f;
		m_frontWheel.radius = 0.68f;
		
		tilt = PlayerPrefs.GetInt("options_control")==1;

		boost = GameObject.FindObjectOfType<Boost> ();
		yeti = GameObject.FindObjectOfType<Yeti> ();
	}

	void Start()
	{
		SetupBike();
		SetupAudio ();
		DistanceHighScore();
		if(!chaseBar)
			chaseBar = GameObject.Find("Chase_Bar").transform;
		
		m_frontWheel.motorTorque = 0;
		m_backWheel.motorTorque = 0;
		m_bar = new Texture2D(1, 1);
		m_bar.SetPixel(0, 0, Color.green);
		
		//sfx_frost fade
		if(sfx_frostObj){			
			sfx_frostColor = sfx_frostObj.renderer.material.color;
			sfx_frostColor.a = 0;
		}
		
		//get start position
		p_startPosition = transform.position;

		ShowControlSelect ();
	}

	void ShowControlSelect(){
		string pfName = "control_showed";
		if (!PlayerPrefs.HasKey (pfName)) {
			DeathScreen.Instance.controlsScreen.SetActive(true);
			Time.timeScale = 0f;
			PlayerPrefs.SetInt(pfName,1);
			PlayerPrefs.Save();
		}
	}

	void SetupAudio()
	{
		bool optMusic = PlayerPrefs.GetInt ("options_music", 1) == 1; // default value: true
		bool optSound = PlayerPrefs.GetInt ("options_sound", 1) == 1; // default value: true

		MusicLoop.Instance.audio.mute = !optMusic;
		MusicLoop.Instance.audio.volume = 0.1f;
		MusicLoop.Instance.DropVolume (-0.4f, 3f);

		AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();
		foreach(var aud in audios)
			if( aud != MusicLoop.Instance.audio)
				aud.mute = !optSound;
	}

	private float rotateFactor = 2f;
	private float boostingTime = 0f;
	void FixedUpdate()
	{
		if(life && !s_pause && !death){

			float tiltangle = -8f*DeviceAcceleration.Acceleration.x;
			float tiltMin = 0.8f;
			if(!tilt) tiltangle = 0;

			boostingTime = Mathf.Max(0f,boostingTime -= Time.deltaTime);

			if(controlLeftRotate || Input.GetAxis("Horizontal")<0f){
				if(rigidbody.angularVelocity.z<0f)
					rigidbody.angularVelocity += new Vector3(0f,0f,10*rotateFactor*Time.deltaTime);
				else
					rigidbody.angularVelocity += new Vector3(0f,0f,rotateFactor*Time.deltaTime);
				//transform.Rotate(-rotateFactor*Time.fixedDeltaTime,0f,0f);
			}
			else if(controlRightRotate || Input.GetAxis("Horizontal")>0f){
				if(rigidbody.angularVelocity.z>0f)
					rigidbody.angularVelocity += new Vector3(0f,0f,-10*rotateFactor*Time.deltaTime);
				else
					rigidbody.angularVelocity += new Vector3(0f,0f,-rotateFactor*Time.deltaTime);
				//transform.Rotate(rotateFactor*Time.fixedDeltaTime,0f,0f);
			}
			else if(Mathf.Abs(tiltangle)>tiltMin) {
				if(tiltangle>0){
					if(rigidbody.angularVelocity.z<0f)
						rigidbody.angularVelocity += new Vector3(0f,0f,10*tiltangle*Time.deltaTime);
					else
						rigidbody.angularVelocity += new Vector3(0f,0f,tiltangle*Time.deltaTime);
				}
				else{
					if(rigidbody.angularVelocity.z>0f)
						rigidbody.angularVelocity += new Vector3(0f,0f,10*tiltangle*Time.deltaTime);
					else
						rigidbody.angularVelocity += new Vector3(0f,0f,tiltangle*Time.deltaTime);
				}
			}
			else
				rigidbody.angularVelocity *= 0.9f;

			if(controlAcceleration || boostingTime>0f){
				isGas = true;
				if(m_backWheel.isGrounded)
					Gas(m_speed*(boostingTime>0f?4f:1f) );
				else if(!controlLeftRotate && !controlRightRotate && Input.GetAxis("Horizontal")==0f)
				{
					// airSpeed is 25f after 30 speed
					float airSpeed = Mathf.Max(25f,m_airSpeed-Speed*m_airSpeed/30f);
					rigidbody.AddRelativeTorque (-airSpeed, 0, 0);
				}
			}
			else{
				isGas = false;
			}

			if(controlBoost && boost.currentBoosts>0){
				yeti.boost = false;
				boostingTime += (float)boost.currentBoosts;
				boost.ClearBoosts();
				audio.PlayOneShot(MobiAssets.Instance.clipBoostUsage);
			}

			// if(tilt)
			/*float angle = -4f*DeviceAcceleration.Acceleration.x;
			if(!m_backWheel.isGrounded){
				rigidbody.AddRelativeTorque (-m_airSpeed*angle, 0, 0);
			}
			else //apply torque
				rigidbody.AddTorque (m_airSpeed*angle, 0, 0);*/
		}
		return;
	}
	
	void Update()
	{		
		#region UNITY_EDITOR
		if(Input.GetKeyDown(KeyCode.Space))
			boost.AddBoost(1);
		#endregion	
		
		if(m_init)
		{
			A_Howel();
			PlaceOnPath();
			m_init = false;
		}

		//var speed = Mathf.Round(rigidbody.velocity.magnitude*2.0f);
		//var topSpeed = 100;
		//var speedFraction = speed / topSpeed;
		//var needleAngle = Mathf.Lerp(70, -80, speedFraction);
		
		//
		//s_handle.transform.Rotate(0,0,needleAngle);
		
		//p_distTraveled = Mathf.Round(Vector3.Distance(transform.position, p_startPosition));
		p_distTraveled = Mathf.Round(transform.position.x-p_startPosition.x);
		tm_dist.text = p_distTraveled.ToString();

		//update chase bar
		if(chaseBar){
			chaseBar_scale = 4.0f*Mathf.Clamp(2*yeti.Distance, 0.0f, 100.0f)*.01f;
			chaseBar.localPosition = new Vector3(chaseBar_scale + 2, chaseBar.localPosition.y, chaseBar.localPosition.z) ;
		}
		
		//expose grounded bool
		_isGrounded = m_backWheel.isGrounded;
		//for modifying particles and sound
		if(!_isGrounded){
			HeightTrick();
			FlipTrick();
		} else {
			if(highTrickCashed)
				StartCoroutine("LandingTrick");
			highTrickCashed = false;
			trickHeight = 0.0f;
			t_isFlipping = false;
			t_endTrick = false;
		}
		
		//if(!life)
		//	fadeAmmount = 1f;
		
		if (sfx_frostObj && !yeti.IsIntro && life){
			sfx_frostColor.a =  fadeAmmount - (yeti.Distance * 0.18f);
			sfx_frostObj.renderer.material.color = sfx_frostColor;
		}
	}

	float fadeAmmount = 0.75f;
	public IEnumerator FadeOut(){
		float speed = 10f;
		while(sfx_frostColor.a<fadeAmmount){
			sfx_frostColor.a += 0.016f*speed;
			sfx_frostObj.renderer.material.color = sfx_frostColor;
			yield return new WaitForEndOfFrame();
		}
	}

	// TRICKS
	private float trickHeight = 0.0f;
	private bool highTrickCashed = false;
	//HEIGHT	
	void HeightTrick(){
		float newHeight;
		Path.m_path.GetHeight(transform.position, out newHeight);
		newHeight = transform.position.y - newHeight;
		if(trickHeight < newHeight){
			//remember this as the highest point
			trickHeight = newHeight;
			//			Debug.Log("climb");
		} else if (trickHeight > 5.0f && !highTrickCashed){//we went as high as we could	
			//			Debug.Log("trick");
			//give coins and boost based off height
			if(Mathf.RoundToInt(trickHeight)>0)
				GiveCoins(Mathf.RoundToInt(trickHeight));
			highTrickCashed = true;
			//is this the highest hight this run?
			if(trickHeight > p_maxHeight){
				p_maxHeight = trickHeight;	
			}
		}
		
	}
	//FLIP
	private bool t_frontFlip;
	private bool t_isFlipping = false;
	private bool t_endTrick;
	void FlipTrick(){
		if(death) return;
		if(!t_isFlipping && transform.up.y < 0 && !t_endTrick){
			
			if(transform.up.x > 0){//front flip?
				t_frontFlip = true;
				
			} else {
				t_frontFlip = false;
			}
			t_isFlipping = true;
		}
		
		if(t_isFlipping && transform.up.y < 0 && !t_endTrick) {
			if(transform.up.x > 0){//front flip?
				if (t_frontFlip == true){
					//FRONT FLIPPED!!!!
					t_endTrick = true;
				}
			} else { //back flip?
				if (t_frontFlip == false){
					//BACK FLIPPED!!!!
					t_endTrick = true;
				}
			}
			
		}if(t_endTrick && transform.up.y > 0) {	
			if(t_isFlipping){
				if(t_frontFlip){
					GiveCoins(1000);
					DeathScreen.Instance.levelController.ShowFrontflipBonus();
					boost.AddBoost(1);
					//instantiate particles and 
				} else {
					GiveCoins(1000);
					DeathScreen.Instance.levelController.ShowBackflipBonus();
					boost.AddBoost(1);
				}
			}
			//upright
			t_isFlipping = false;
			t_endTrick = false;
		}
	}

	//LANDING
	IEnumerator LandingTrick(){
		yield return new WaitForSeconds(0.5f);
		if(m_frontWheel.isGrounded && !death){
			//maybe check velocity is above a certain amount	
			/*??*/	//GiveBoost(1); 
		}
	}

	void GiveCoins(int coins){
		//in the coin counter prefab
		Coin_Counter.AddCoins(coins);
	}
	
	//BRAKE	
	public void Brake(float amount)
	{
		rigidbody.AddForce (Vector3.right * -amount);
	}
	
	//GAS	
	public void Gas(float amount)
	{	
		//bonus acceleration for low velocity
		if(rigidbody.velocity.magnitude < m_accelThresh)
		{
			rigidbody.AddForce (Vector3.right * amount*2);
		}
		else
			rigidbody.AddForce (Vector3.right * amount);
	}

	//KILL TRIGGER
	public void OnTriggerEnter(Collider other){
		if(life && other.gameObject.tag == "Ground"){
			// Sled crash
			audio.PlayOneShot(MobiAssets.Instance.clipImpactCrash,1f);
			Death(false);

		}
	}
	
	//DEATH
	public void Death(bool monstercause) {
		if(monstercause){
			deathskip=true;
		}

		if(!death){
			death = true;
			StartCoroutine("DeathThread",monstercause);
		}
	}
	
	bool deathskip=false;
	bool death=false;
	private IEnumerator DeathThread(bool monstercause) {
		
		// below code copy
		s_gas = 0;
		isGas = false;
		isBoost = false;
		
		if(!monstercause)
		{
			ragdoll.Work();
			float time = 3.5f;
			while(time>=0f&&!deathskip)
			{
				time -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
		}
		
		//GameObject.Find("RevmobManager").SendMessage("showRevmobFullScreen",SendMessageOptions.DontRequireReceiver);
		life=false;
		//rigidbody.drag = 2;
		//Debug.Log("DEATH");
		
		chaseBar.parent.gameObject.SetActive(false);

		//bonus and save coins
		// Removed bonus
		//Coin_Counter.AddSaveCoins(250);
		
		//rigidbody.isKinematic = true;
		UpdateEndMenu();
		
		// invention
		DeathScreen.Instance.Show(monstercause,
		                          Mathf.RoundToInt(p_distTraveled),
		                          (int)(Mathf.Round(PlayerPrefs.GetFloat(DeathScreen.Instance.levelName + "MaxDistance"))),
		                          Coin_Counter.m_ccounter.coin_balance - Coin_Counter.m_ccounter.start_balance);
		
	}
	
	public void DistanceHighScore() {
		//distance highscore
		if(PlayerPrefs.HasKey(DeathScreen.Instance.levelName + "MaxDistance")){
			tm_distHigh.text = ((int)(Mathf.Round(PlayerPrefs.GetFloat(DeathScreen.Instance.levelName + "MaxDistance")))).ToString() + "m";;
			//tm_distHigh.gameObject.renderer.material.color = Color.yellow;
		} else {
			tm_distHigh.gameObject.SetActive(false);
		}
	}	
	
	public void UpdateEndMenu() {		
		//update dist traveled
		p_distTraveled = Mathf.RoundToInt(p_distTraveled);
		//is it farther than ever before?!
		if(PlayerPrefs.HasKey(DeathScreen.Instance.levelName + "MaxDistance")){
			if(PlayerPrefs.GetFloat(DeathScreen.Instance.levelName + "MaxDistance") < p_distTraveled){
				//NEW RECORD!
				PlayerPrefs.SetFloat(DeathScreen.Instance.levelName + "MaxDistance", p_distTraveled);
			}
		} else {
			PlayerPrefs.SetFloat(DeathScreen.Instance.levelName + "MaxDistance", p_distTraveled);
		}
		
		
		
		//uptate highest jump
		//is it higher than ever before?!
		if(PlayerPrefs.HasKey("MaxHeight")){
			if(PlayerPrefs.GetFloat("MaxHeight") < p_maxHeight){
				//NEW RECORD!
				PlayerPrefs.SetFloat("MaxHeight", p_maxHeight);
			}
		} else {
			PlayerPrefs.SetFloat("MaxHeight", p_maxHeight);			
		}
	} 
	
	//END SCREEN
	public IEnumerator FadeTo(float aValue, float aTime, Transform trans)
	{
		float alpha = trans.renderer.material.color.a;
		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
		{
			Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
			trans.renderer.material.color = newColor;
			yield return null;
		}
	}
	
	//SOUNDS
	public void A_Grunt(){
		audio.PlayOneShot(a_monsterGrunt[Random.Range(0,a_monsterGrunt.Length)], 1f);
		//Debug.Log("grunt");
	}
	public void A_Howel(){
		audio.PlayOneShot(a_monsterHowel[Random.Range(0,a_monsterHowel.Length)], 1f);
		//Debug.Log("howel");
	}
	public void A_Attack(){
		audio.PlayOneShot(a_monsterAttack[Random.Range(0,a_monsterAttack.Length)], 1f);
		//Debug.Log("attack");
	}
	//SET BIKE PROPERTIES
	public void SetupBike(){
		//load selected sled
		//		Debug.Log(GameManager.m_chosenSled);
		bike = Instantiate(Resources.Load("Bikes/Sled_" + GameManager.m_chosenSled)) as GameObject;
		
		if(!bike)
		{
			bike = Instantiate(Resources.Load("Classic")) as GameObject;
		} 
		//place selected sled
		bike.transform.parent = transform.Find("Sled");
		bike.transform.localPosition = Vector3.zero;
		
		// update 1
		bike.transform.Find("2_Mobi").gameObject.SetActive(false);
		CreateNewRagdoll ();
		
		//		m_backWheel = newSled.transform.Find("_BackWheel").GetComponent<WheelCollider>();
		//		m_frontWheel = newSled.transform.Find("_FrontWheel").GetComponent<WheelCollider>();
		//setup collider
		
		
		float acc_bonus = 0.0f; //low numbers
		float speed_bonus = 0.0f; // default is 300
		
		//	Get Bikes name in order to set the air control and additional update variables. and set propper collider pos
		var collider = gameObject.collider as SphereCollider;
		if(GameManager.m_chosenSled.ToString() == "Classic"){
			//set collider
			collider.center = classic_cc;
			//upgrades
			acc_bonus = 12f; //low numbers
			speed_bonus = 350f;
			rigidbody.mass = rigidbody.mass + 0f;
			
		} else if(GameManager.m_chosenSled.ToString() == "Turbo") {
			//set collider
			collider.center = turbo_cc;
			//upgrades
			acc_bonus = 14f; //low numbers
			speed_bonus = 400f;
			rigidbody.mass = rigidbody.mass + 0f;
		} else if(GameManager.m_chosenSled.ToString() == "Track") {
			//set collider
			collider.center = track_cc;
			//upgrades
			acc_bonus = 16f; //low numbers
			speed_bonus = 500f;
			rigidbody.mass = rigidbody.mass + 0;
		} else if(GameManager.m_chosenSled.ToString() == "Sport") {
			//set collider
			collider.center = sport_cc;
			//upgrades
			acc_bonus = 18f; //low numbers
			speed_bonus = 600f;
			rigidbody.mass = rigidbody.mass + 0f;
		} else if(GameManager.m_chosenSled.ToString() == "Hybrid") {
			//set collider
			collider.center = hybrid_cc;
			//upgrades
			acc_bonus = 20f; //low numbers
			speed_bonus = 700f;
			rigidbody.mass = rigidbody.mass + 0f;
		}	
		
		//	Get the upgrades and their value and use those as multipliers for the speed and shit.
		int engine = PlayerPrefs.GetInt (GameManager.m_chosenSled.ToString () + "_" + "Engine",0);
		int suspension = PlayerPrefs.GetInt (GameManager.m_chosenSled.ToString () + "_" + "Suspension",0);
		int tread = PlayerPrefs.GetInt (GameManager.m_chosenSled.ToString () + "_" + "Tread", 0);
		
		//Debug.Log(GameManager.m_chosenSled.ToString() + " engine: " + engine +
		//          ", suspension: " + suspension + ", tread: " + tread);
		
		m_accelThresh = (acc_bonus) + engine;
		m_airSpeed = 150 + 10*suspension;
		//set players springness? mass?
		m_speed = speed_bonus + 25*tread;

		// new control system
		rotateFactor += 0.2f * suspension;
		
	}

	void CreateNewRagdoll(){
		GameObject rg = Instantiate(Resources.Load("Ragdolls/"+GameManager.m_chosenMobi)) as GameObject;
		ragdoll = rg.GetComponent<Ragdoll>();
		ragdoll.gameObject.SetActive(false);
		ragdoll.gameObject.transform.parent = bike.transform.Find("ragdollposition");
		//Debug.Log("name is: " + newSled.transform.Find("ragdollposition").name);
		ragdoll.gameObject.transform.localPosition = Vector3.zero;
		ragdoll.gameObject.transform.localRotation = Quaternion.identity;
		ragdoll.gameObject.transform.localScale = new Vector3(1f,1f,1f);
		ragdoll.gameObject.SetActive(true);
	}

	public void ResumeGameAfterCrashing(){
		rigidbody.isKinematic = false;
		Destroy (ragdoll.gameObject);
		GameObject.Find ("Failed Screen").SetActive (false);
		CreateNewRagdoll ();
		transform.localRotation = Quaternion.Euler (0f, 90f, 0f);
		PlaceOnPath ();
		life = true;
		death = false;
		Time.timeScale = 1f;
		sfx_frostColor.a = 0f;
		sfx_frostObj.renderer.material.color = sfx_frostColor;

		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;

		// Remove Flips
		t_isFlipping = false;
		t_endTrick = false;
		boostingTime = 0f;

		// Turn up volume
		MusicLoop.Instance.DropVolume (-0.4f, 3f);

		// Setup Yeti
		yeti.ReSetupOnPlayerRespawn ();
		chaseBar.parent.gameObject.SetActive(true);
		deathskip = false;
	}
	
	public void PlaceOnPath()
	{
		transform.forward = Path.Current.GetNormal(transform.position);
		Vector3 startPos = transform.position;
		Path.Current.GetHeight(transform.position, out startPos.y);
		transform.position = startPos + (transform.up * transform.localScale.y);
	}
	
	#region Properties
	
	public bool GamePaused { get{ return s_pause; }}
	public float Speed { get { return Mathf.Round (rigidbody.velocity.magnitude * 2.0f); } }
	public float BoostingTime { get { return boostingTime; } }
	
	#endregion

	#region Updates 3

	public void PauseGame()
	{
		if(life && !death)
		{
			isGas = false;
			s_pause = !s_pause;
			pause_flag = s_pause;
			Time.timeScale = s_pause ? 0.0f : 1f;
			DeathScreen.Instance.ShowPauseScreen(pause_flag);
		}
	}

	public bool controlAcceleration{get;set;}
	public bool controlBoost{get;set;}
	public bool controlLeftRotate{get;set;}
	public bool controlRightRotate{get;set;}

	#endregion

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile PlayerController _staticInstance;	
	public static PlayerController Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(PlayerController)) as PlayerController;
					if (_staticInstance == null) {
						Debug.LogError("The PlayerController instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
	
}