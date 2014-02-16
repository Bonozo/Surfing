using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour
{
	public GameObject bonusText;
	public bool s_pause = false;
	public GameObject s_pauser;

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
	public bool _isGrounded = true;
	
	public bool isGas = false;
	
	public float boostJuice = 0.0f;
	public float max_boost = 6.0f;
	public bool isBoost = false;
	public GameObject[] boostBloops;
	
	private Texture2D m_bar = null;
	
	private bool m_init = true;
	
	//AUDIO
//	public AudioSource a_doom;
	public AudioClip[] a_monsterHowel;
	public AudioClip[] a_monsterGrunt;
	public AudioClip[] a_monsterAttack;
	
	public Transform trick1;
	public Transform boost_trick1;
	
	//MONSTER
	public bool life = true;
	public Transform monster;
	public float monsterDistance;
	private float m_monsterSpeed = 12.0f;
	private float monsterSpeedUpTimestep = 30f;
	
	//collider settings
	private Vector3 classic_cc = new Vector3(0f, 1.15f, -.48f);
	private Vector3 turbo_cc = new Vector3(0f, 1.15f, -.18f);
	private Vector3 track_cc = new Vector3(0f, 1.15f, -.39f);
	private Vector3 sport_cc = new Vector3(0f, 1.3f, -.27f);
	private Vector3 hybrid_cc = new Vector3(0f, 1.3f, -.21f);
	
	//mobi swapping
	//private GameObject newMobi;
	//public Material[] mobiMats;
	
	//end menu
	public Transform endMenuButton;
	public Transform endMenuHome;
	public GameObject s_left;
	public GameObject s_right;
	public GameObject s_boost;
	
	
	private Vector3 p_startPosition;
	public float p_distTraveled;
	public TextMesh tm_distTraveled;
	
	public float p_maxHeight;
	public TextMesh tm_maxHeight;
	
	public TextMesh txt_DeathReason;
	public TextMesh txt_pausePosition;
	public TextMesh txt_p1;
	public TextMesh txt_p2;
	public TextMesh txt_p3;
	
	public TextMesh tm_speedo;
	public TextMesh tm_dist;
	public TextMesh tm_distHigh;
	
//	public int p_coinsEarned;
	public TextMesh tm_coinsEarned;
		
	//dark screen
	public Transform sfx_frostObj;
	private Color sfx_frostColor;
	
	public Transform chaseBar;
	private float chaseBar_scale;
	//pause flag
	
	private bool pause_flag = false;
	public GameObject darkScreenPause;
	Vector3 savedVelocity;
    Vector3 savedAngularVelocity;
	
	// by Aharon

	// ragdoll
	private Ragdoll ragdoll;

	void Awake()
	{
		m_backWheel.radius = 0.74f;
		m_frontWheel.radius = 0.68f;
	}
	
	
		// Subscribe to events
	void OnEnable(){
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
		EasyTouch.On_SimpleTap -= On_SimpleTap;
	}

	public void Resume()
	{
		Gesture gesture = new Gesture ();
		gesture.pickObject = s_pauser;
		On_SimpleTap (gesture);
	}

	private void On_SimpleTap( Gesture gesture){
		//Debug.Log("Simple Tap");
		if(life && !death && gesture.pickObject == s_pauser){
			s_pause = !s_pause;
			//txt_p1.text = ""
			if (pause_flag == false){
				//Time.timeScale = 0;
				savedVelocity = rigidbody.velocity;
       			savedAngularVelocity = rigidbody.angularVelocity;
      			//rigidbody.isKinematic = true;
				Debug.Log(savedVelocity);
				rigidbody.Sleep();
				//GameObject.Find("DarkPlanePause").SetActive(true);
				darkScreenPause.SetActive(true);
				//sfx_frostObj.renderer.material.color.a = 0.5f;
				pause_flag = true;
				txt_p1.text = "";
				txt_p2.text = "";
				txt_p3.text = "";
				//txt_DeathReason.gameObject.transform.position = txt_p2.gameObject.transform.position;
				//tm_coinsEarned.gameObject.transform.position = txt_pausePosition.gameObject.transform.position;
		
			}
			else
			{
				//Time.timeScale = 1;
				//rigidbody.isKinematic = false;
				rigidbody.WakeUp();
				//rigidbody.velocity = savedVelocity;
      			rigidbody.AddForce( savedVelocity, ForceMode.VelocityChange );
       			rigidbody.AddTorque( savedAngularVelocity, ForceMode.VelocityChange );
				pause_flag = false;
				//GameObject.Find("DarkPlanePause").SetActive(false);
				//sfx_frostObj.renderer.material.color.a = 0.0f;
				darkScreenPause.SetActive(false);
			}

			DeathScreen.Instance.ShowPauseScreen(pause_flag);
			//Debug.Log("paused");
		}
		
		if (gesture.pickObject.name == "_Restart"){
			Application.LoadLevel(Application.loadedLevel); //Application.loadedLevel
		}else if (gesture.pickObject.name == "_MainMenu"){
			Application.LoadLevel(0); //Application.loadedLevel
		} 
	}
	
	private void On_TouchDown( Gesture gesture){
		if(life && !s_pause && !death){
			if(gesture.pickObject.name == "LeftGas"){
				s_gas = -1;	
			} else if (gesture.pickObject.name == "RightGas"){
				s_gas = 1;
			} else if (gesture.pickObject.name == "BoostGas"){
				s_gas = 2;
			}
		}
//		else {
//			if (gesture.pickObject.name == "_Restart"){
//						Application.LoadLevel(Application.loadedLevel); //Application.loadedLevel
//			}else if (gesture.pickObject.name == "_MainMenu"){
//				Application.LoadLevel(0); //Application.loadedLevel
//			} 
//		}
	}
	private void On_TouchUp( Gesture gesture){
		s_gas = 0;
	}
	
	void Start()
	{
		SetupBike();
		DistanceHighScore();
		//find left and right
		if(!s_left)
			s_left = GameObject.Find("LeftGas");
		if(!s_right)
			s_right = GameObject.Find("RightGas");
		if(!s_boost)
			s_boost = GameObject.Find("BoostGas");
		if(!chaseBar)
			chaseBar = GameObject.Find("Chase_Bar").transform;
		if(!s_pauser)
			s_pauser = GameObject.Find("Pauser");
		
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
		
		A_Howel();
		StartCoroutine("Monster");
	}
	
	void FixedUpdate()
	{
		//StartCoroutine("UpdateSpeed");
		//REMOVE THE AFTER OR HAHA
		if(life && !s_pause && !death){
			//Gas!..
			if(Input.GetKey("right") || s_gas == 1)
			{
				isGas = true;
				if(m_backWheel.isGrounded)
					Gas(m_speed);
				
				//add spin to play in the air
				if(!m_backWheel.isGrounded){
					rigidbody.AddRelativeTorque (-m_airSpeed, 0, 0);
				}
				else //apply torque
					rigidbody.AddTorque (m_airSpeed, 0, 0);
	
			}
			if(Input.GetKey("left") || s_gas == -1) //Brake!..
			{
				isGas = false;
				if(m_backWheel.isGrounded )
					Brake(m_break);
				
				//add spin to play in the air
				if(!m_frontWheel.isGrounded)
					rigidbody.AddRelativeTorque (m_airSpeed, 0, 0);
				else
					rigidbody.AddRelativeTorque (m_airSpeed, 0, 0);
			}
			if((Input.GetKey("up") || s_gas == 2) && boostJuice > 0) //also check if we have boost juice
			{
			
				isGas = true;
				if(m_backWheel.isGrounded){
					Debug.Log("BOOSTTEST");
					Gas(m_speed*m_boost);
					boostJuice = boostJuice - Time.deltaTime;
					isBoost = true;
					if(boostJuice < 5.5){
						boostBloops[5].SetActive(false);
						if(boostJuice < 4.5){
							boostBloops[4].SetActive(false);
							if(boostJuice < 3.5){
								boostBloops[3].SetActive(false);
								if(boostJuice < 2.5){
									boostBloops[2].SetActive(false);
									if(boostJuice < 1.5){
										boostBloops[1].SetActive(false);
										if(boostJuice < 0.5){
											boostBloops[0].SetActive(false);
										}
									}
								}
							}
						}
					}
				}
				
				//add spin to play in the air
				if(!m_backWheel.isGrounded){
					rigidbody.AddRelativeTorque (-m_airSpeed, 0, 0);
				}
				else //apply torque
					rigidbody.AddTorque (m_airSpeed, 0, 0);
	
			} 
			if(s_gas == 0){
				isGas = false;	
			}
		}
	}
	
	
	void Update()
	{		
				
		if(m_init)
		{
			PlaceOnPath();
			m_init = false;
		}
		if (life && !s_pause)
			MonsterMover();
			tm_speedo.text = Mathf.Round(rigidbody.velocity.magnitude*2.0f).ToString();// ((Mathf.Round(rigidbody.velocity.magnitude*10.0f)/10.0F)*2.0f).ToString();
			//
			//var speed = Mathf.Round(rigidbody.velocity.magnitude*2.0f);
			//var topSpeed = 100;
			//var speedFraction = speed / topSpeed;
			//var needleAngle = Mathf.Lerp(70, -80, speedFraction);
			
			//
			//s_handle.transform.Rotate(0,0,needleAngle);
		
			p_distTraveled = Mathf.Round(Vector3.Distance(transform.position, p_startPosition));
			tm_dist.text = p_distTraveled.ToString();

		
		//expose
		monsterDistance = Vector3.Distance(transform.position, monster.position);
		
		//update chase bar
		if(chaseBar){
			chaseBar_scale = 4.0f*Mathf.Clamp(monsterDistance-30, 0.0f, 100.0f)*.01f;
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
		
//		//turn up volume on doom music
//		if (a_doom){
//			a_doom.volume = 1 - (monsterDistance * 0.01f);
//		}
		
		float fadeAmmount = 0.75f;
		if(!life)
			fadeAmmount = 1.0f;
		
		//if(pause_flag)
		//	fadeAmmount = 1.0f;
		
		if (sfx_frostObj){
			sfx_frostColor.a =  fadeAmmount - (monsterDistance * 0.01f);
			sfx_frostObj.renderer.material.color = sfx_frostColor;
			//sfx_frostObj.transform.localScale = new Vector3(5, 1, Mathf.Lerp(2, 30, (monsterDistance * 0.01f)));
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
					//FRONT FLIPPED!!!!
					Debug.Log("FRONT FLIPPED!!!!");
					//Instantiate(boost_trick1, transform.position, Camera.mainCamera.transform.rotation);
					//Transform boostboom;
        			//boostboom = Instantiate(boost_trick1, s_boost.transform.position, Camera.mainCamera.transform.rotation) as Transform;
					//boostboom.parent = s_boost.transform; 
					GiveCoins(1000);
					Debug.Log ("FLIPPPPPP");
					//Instantiate(bonusText);
					bonusText.SetActive( true);
					GiveBoost(2.0f);
					//instantiate particles and 
				} else {
					//BACK FLIPPED!!!!
					Debug.Log("BACK FLIPPED!!!!");
					//Transform boostboom;
			        //boostboom = Instantiate(boost_trick1, s_boost.transform.position, Camera.mainCamera.transform.rotation) as Transform;
					//boostboom.parent = s_boost.transform; 
					//Instantiate(boost_trick1, transform.position, Camera.mainCamera.transform.rotation);
					GiveCoins(1000);
					//Instantiate(bonusText);
					bonusText.SetActive ( true );
					GiveBoost(2.0f);
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
		if(m_frontWheel.isGrounded){
		//maybe check velocity is above a certain amount	
			GiveBoost(1.0f);
		}
	}
	
	
	void GiveCoins(int coins){
		//in the coin counter prefab
		Coin_Counter.AddCoins(coins);
	}
		
	
//ADD BOOST
	void GiveBoost(float addBoost){
		//Transform boostboom;
        //boostboom = Instantiate(boost_trick1, s_boost.transform.position, Camera.mainCamera.transform.rotation) as Transform;
		//boostboom.parent = s_boost.transform; 
		
		boostJuice = boostJuice + addBoost;	
		boostJuice = Mathf.Clamp(boostJuice, 0.0f, max_boost);
		if(boostJuice > 0.5){
			boostBloops[0].SetActive(true);
			if(boostJuice > 1.5){
				boostBloops[1].SetActive(true);
				if(boostJuice > 2.5){
					boostBloops[2].SetActive(true);
					if(boostJuice > 3.5){
						boostBloops[3].SetActive(true);
						if(boostJuice > 4.5){
							boostBloops[4].SetActive(true);
							if(boostJuice > 5.5){
								boostBloops[5].SetActive(true);
							}
						}
					}
				}
			}
		}
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

	#region Monster

	public IEnumerator Monster(){
	//every XX seconds increase Monsters speed by varR
		while (life){
			yield return new WaitForSeconds(monsterSpeedUpTimestep);
			m_monsterSpeed = m_monsterSpeed + 1;
		}

		
	}
	
	public void MonsterMover(){
		if(!s_pause){
			// The step size is equal to speed times frame time.
	        float step = m_monsterSpeed * Time.deltaTime;
	        
	        // Move our position a step closer to the target.
	        monster.position = Vector3.MoveTowards(monster.position, transform.position, step);
			
			//if the monster is too close life is over
			if(life && Vector3.Distance(transform.position, monster.position)<30)
			{	
				life = false;
	//			MonsterCloseIn();
				Death(true);
				txt_p1.text = "try upgrading your engine to";
				txt_p2.text = "increase speed and power";
				txt_p3.text = "";
				//play sound
				darkScreenPause.SetActive( true);
			}	
		}
	}
	
	public void MonsterCloseIn(){
		monster.position += Vector3.up *30;//.Set(monster.position.x ,transform.position.y + 30, monster.position.z);
		m_monsterSpeed = 5;
	}

	#endregion
	
//KILL TRIGGER
	public void OnTriggerEnter(Collider other){
		if(life && other.gameObject.tag == "Ground"){
			Debug.Log(other.gameObject.name);
			Death(false);
			txt_p1.text = "try upgrading your shocks";
			txt_p2.text = "for better control";
			txt_p3.text = "when landing or hitting a bump";
		}
	}
	
//DEATH
	public void Death(bool monstercause) {
		if(monstercause) deathskip=true;
		if(!death)
		{
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
				yield return null;
			}
		}

		GameObject.Find("RevmobManager").SendMessage("showRevmobFullScreen",SendMessageOptions.DontRequireReceiver);
		life=false;
		rigidbody.drag = 2;
		Debug.Log("DEATH");
		s_gas = 0;
		isGas = false;
		isBoost = false;
		
		chaseBar.parent.gameObject.SetActive(false);
		//bonus and save coins
		Coin_Counter.AddSaveCoins(250);
		
		rigidbody.isKinematic = true;
		UpdateEndMenu();
	
		if(monstercause)
			txt_DeathReason.text = "You are too slow!";
		else
			txt_DeathReason.text = "You Crashed!";

		// invention
		DeathScreen.Instance.Show(monstercause,
            (int)(Mathf.Round(Vector3.Distance(transform.position, p_startPosition)*10.0f)/10.0F),
	        (int)(Mathf.Round(PlayerPrefs.GetFloat("MaxDistance"))),
	        Coin_Counter.m_ccounter.coin_balance - Coin_Counter.m_ccounter.start_balance);

	}
	
	public void DistanceHighScore() {
		//distance highscore
		if(PlayerPrefs.HasKey("MaxDistance")){
			tm_distHigh.text = PlayerPrefs.GetFloat("MaxDistance").ToString() + "m";;
			//tm_distHigh.gameObject.renderer.material.color = Color.yellow;
		} else {
			tm_distHigh.gameObject.SetActive(false);
		}
	}	
	
	public void UpdateEndMenu() {		
		//update dist traveled
		p_distTraveled = Mathf.Round(Vector3.Distance(transform.position, p_startPosition)*10.0f)/10.0F;
		tm_distTraveled.text = p_distTraveled.ToString() + "m";
		//is it farther than ever before?!
		if(PlayerPrefs.HasKey("MaxDistance")){
			if(PlayerPrefs.GetFloat("MaxDistance") < p_distTraveled){
				//NEW RECORD!
				PlayerPrefs.SetFloat("MaxDistance", p_distTraveled);
				tm_distTraveled.gameObject.renderer.material.color = Color.yellow;	
			}
		} else {
			PlayerPrefs.SetFloat("MaxDistance", p_distTraveled);
			tm_distTraveled.gameObject.renderer.material.color = Color.yellow;				
		}
		
		
		
		//uptate highest jump
		tm_maxHeight.text = ((Mathf.Round(p_maxHeight)*10.0f)/10.0F).ToString() + "m";
		//is it higher than ever before?!
		if(PlayerPrefs.HasKey("MaxHeight")){
			if(PlayerPrefs.GetFloat("MaxHeight") < p_maxHeight){
				//NEW RECORD!
				PlayerPrefs.SetFloat("MaxHeight", p_maxHeight);
				tm_maxHeight.gameObject.renderer.material.color = Color.yellow;	
			}
		} else {
			PlayerPrefs.SetFloat("MaxHeight", p_maxHeight);
			tm_maxHeight.gameObject.renderer.material.color = Color.yellow;				
		}
		
		int coinDif;
		coinDif = Coin_Counter.m_ccounter.coin_balance - Coin_Counter.m_ccounter.start_balance;
		tm_coinsEarned.text = "+" + coinDif.ToString();
		tm_coinsEarned.gameObject.renderer.material.color = Color.yellow;
		
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
		GameObject newSled = Instantiate(Resources.Load("Bikes/Sled_" + GameManager.m_chosenSled)) as GameObject;

		if(!newSled)
		{
			newSled = Instantiate(Resources.Load("Classic")) as GameObject;
		} 
		//place selected sled
			newSled.transform.parent = transform.Find("Sled");
			newSled.transform.localPosition = Vector3.zero;
		//load selected Mobi
		///////////newMobi = newSled.transform.Find("2_Mobi").gameObject;
		//update the newMobi material appropriatly, player's choice from the main menu
		///////////	newMobi.renderer.material = mobiMats[(int)GameManager.m_chosenMobi];


		// update 1
		newSled.transform.Find("2_Mobi").gameObject.SetActive(false);
		///////////newMobi.SetActive(false);
		GameObject rg = Instantiate(Resources.Load("Ragdolls/"+GameManager.m_chosenMobi)) as GameObject;
		ragdoll = rg.GetComponent<Ragdoll>();
		ragdoll.gameObject.SetActive(false);
		ragdoll.gameObject.transform.parent = newSled.transform.Find("ragdollposition");
		//Debug.Log("name is: " + newSled.transform.Find("ragdollposition").name);
		ragdoll.gameObject.transform.localPosition = Vector3.zero;
		ragdoll.gameObject.transform.localScale = new Vector3(1f,1f,1f);
		ragdoll.gameObject.SetActive(true);
 
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
			m_airSpeed = m_airSpeed + 0f;
			acc_bonus = 12f; //low numbers
			speed_bonus = 300f;
			rigidbody.mass = rigidbody.mass + 0f;
			
		} else if(GameManager.m_chosenSled.ToString() == "Turbo") {
			//set collider
			collider.center = turbo_cc;
			//upgrades
			m_airSpeed = m_airSpeed + 0f;
			acc_bonus = 14f; //low numbers
			speed_bonus = 400f;
			rigidbody.mass = rigidbody.mass + 0f;
		} else if(GameManager.m_chosenSled.ToString() == "Track") {
			//set collider
			collider.center = track_cc;
			//upgrades
			m_airSpeed = m_airSpeed + 0f;
			acc_bonus = 16f; //low numbers
			speed_bonus = 500f;
			rigidbody.mass = rigidbody.mass + 0;
		} else if(GameManager.m_chosenSled.ToString() == "Sport") {
			//set collider
			collider.center = sport_cc;
			//upgrades
			m_airSpeed = m_airSpeed + 0f;
			acc_bonus = 18f; //low numbers
			speed_bonus = 600f;
			rigidbody.mass = rigidbody.mass + 0f;
		} else if(GameManager.m_chosenSled.ToString() == "Hybrid") {
			//set collider
			collider.center = hybrid_cc;
			//upgrades
			m_airSpeed = m_airSpeed + 0f;
			acc_bonus = 20f; //low numbers
			speed_bonus = 700f;
			rigidbody.mass = rigidbody.mass + 0f;
		}	

	//	Get the upgrades and their value and use those as multipliers for the speed and shit.
		int engine = PlayerPrefs.GetInt (GameManager.m_chosenSled.ToString () + "_" + "Engine",0);
		int suspension = PlayerPrefs.GetInt (GameManager.m_chosenSled.ToString () + "_" + "Suspension",0);
		int tread = PlayerPrefs.GetInt (GameManager.m_chosenSled.ToString () + "_" + "Tread", 0);

		Debug.Log(GameManager.m_chosenSled.ToString() + " engine: " + engine +
		          ", suspension: " + suspension + ", tread: " + tread);

		m_accelThresh = (acc_bonus) + engine;
		m_airSpeed = 100 + 10*suspension;
		//set players springness? mass?
		m_speed = speed_bonus + 25*tread;
		
	}
	
	public void PlaceOnPath()
	{
		transform.forward = Path.Current.GetNormal(transform.position);
		Vector3 startPos = transform.position;
		Path.Current.GetHeight(transform.position, out startPos.y);
		transform.position = startPos + (transform.up * transform.localScale.y);
	}

	#region Upgrade1

	public bool GamePaused{
		get{
			return s_pause;
		}
	}

	#endregion
}


