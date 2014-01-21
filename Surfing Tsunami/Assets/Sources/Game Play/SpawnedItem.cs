using UnityEngine;
using System.Collections;

public class SpawnedItem : MonoBehaviour {
	
	
	private const float LifeTimeMin=10f,LifeTimeMax=20f;
	
	#region Public Field
	
	public float ExtraDepth = 0.0f;
	public float Mass = 1f;
	public Vector3 defaultSize;
	public bool isObstacle = true;
	
	public bool FaceToPlayer = false;
	public bool ReversedTexture = false;
	
	public bool MoveToPlayer = false;
	public float MoveToPlayerForce = 0.15f;
	
	public bool RandomDirection = false;
	public float RandomDirectionForce = 0.12f;
	
	public bool Reversed = false;
	
	#endregion
	
	#region Private Field
	
	private Vector3 realScale;
	private float lifeTime;
	private bool scaleOrigin = true;
	private Vector3 randomDirection;
	
	#endregion
	
	#region Virtual Awake, Start, Update
	
	protected virtual void Awake()
	{
		realScale = transform.localScale;
		lifeTime = Random.Range(LifeTimeMin,LifeTimeMax);
		
		if(isObstacle)
		{
			bound = gameObject.GetComponent<tk2dSprite>().GetBounds();
			Mass = (bound.size.x*bound.size.y)*0.001f;
		}
		else
		{
			bound = new Bounds(Vector3.zero,defaultSize);
		}
		
		transform.position = RandomSpawnPosition(Depth);
		
		if(Reversed)
		{
			Vector3 center = LevelInfo.Camera.Center;
			transform.position = new Vector3(2*center.x-transform.position.x,2*center.y-transform.position.y,transform.position.z);
			Mass *= 0.25f;
		}
		
		if(RandomDirection)
			randomDirection = Random.insideUnitCircle;
	}
	
	private Vector3 RandomSpawnPosition(float depth)
	{
		Vector3 vec = Vector3.zero;
		
		float offset = LevelInfo.Camera.Height*0.5f;
		
		switch(LevelInfo.State.wind)
		{	
		case WindState.Down:
			vec = new Vector3(Random.Range(0f,LevelInfo.Camera.Width-bound.size.x),-offset,depth);
			break;
		case WindState.Left:
			vec = new Vector3(-offset,Random.Range(0f,LevelInfo.Camera.Height-bound.size.y),depth);
			break;
		case WindState.Right:
			vec = new Vector3(LevelInfo.Camera.Width+offset,Random.Range(0f,LevelInfo.Camera.Height-bound.size.y),depth);
			break;
		case WindState.DownLeft:
			if(Random.Range(0,2)==1)
				vec = new Vector3(-offset,Random.Range(-offset,LevelInfo.Camera.Height-offset-bound.size.y),depth);
			else
				vec = new Vector3(Random.Range(-offset,LevelInfo.Camera.Width-offset-bound.size.x),-offset,depth);
			break;
		case WindState.DownRight:
			if(Random.Range(0,2)==1)
				vec = new Vector3(LevelInfo.Camera.Width+offset,Random.Range(-offset,LevelInfo.Camera.Height-offset-bound.size.y),depth);
			else
				vec = new Vector3(Random.Range(offset,LevelInfo.Camera.Width+offset-bound.size.x),-offset,depth);
			break;
		default:
			Debug.LogError("Objects spawn is not determined for " + LevelInfo.State.wind + " state");
			break;
		}
		return vec;
	}
	
	private Bounds bound;
	
	protected virtual void Start()
	{
	}
	
	private void ReverseTexture(bool right)
	{
		if(right)
		{
			transform.Translate(bound.size.x,0f,0f,Space.World);
			transform.Rotate(0f,180f,0f,Space.World);		
		}
		else
		{
			transform.Translate(-bound.size.x,0f,0f,Space.World);
			transform.Rotate(0f,-180f,0f,Space.World);	
		}
	}
	
	protected virtual void Update()
	{
		if(LevelInfo.State.state != GameState.Play)
			return;
		
		
		// Face to Player
		if(FaceToPlayer)
		{


			if(ReversedTexture)
			{
				if(!scaleOrigin && transform.position.x - bound.center.x + 2f  < LevelInfo.Environments.surfer.Position.x )
				{
					scaleOrigin = !false;
					transform.Translate(-bound.size.x,0f,0f,Space.World);
					//transform.Rotate(0f,-180f,0f,Space.World);
				}
				else if(scaleOrigin && transform.position.x + bound.center.x -2f > LevelInfo.Environments.surfer.Position.x )
				{
					scaleOrigin = !true;
					transform.Translate(bound.size.x,0f,0f,Space.World);
					//transform.Rotate(0f,180f,0f,Space.World);
				}			
			}
			else
			{
				if(scaleOrigin && transform.position.x + bound.center.x + 2f < LevelInfo.Environments.surfer.Position.x )
				{
					scaleOrigin = false;
					transform.Translate(bound.size.x,0f,0f,Space.World);
					//transform.Rotate(0f,180f,0f,Space.World);
				}
				else if(!scaleOrigin && transform.position.x - bound.center.x -2f > LevelInfo.Environments.surfer.Position.x )
				{
					scaleOrigin = true;
					transform.Translate(-bound.size.x,0f,0f,Space.World);
					//transform.Rotate(0f,-180f,0f,Space.World);
				}
			}

			var rot = transform.rotation.eulerAngles;
			rot.y = scaleOrigin?0f:180f;
			transform.rotation = Quaternion.Euler(rot);
		}
		
		lifeTime = Mathf.Clamp(lifeTime-Time.deltaTime,0f,float.PositiveInfinity);
		if(lifeTime <= 0f && IsInScreenOff )
			Destroy(this.gameObject);
		
		var move = LevelInfo.State.force/Mass;
		
		if( Reversed ) { move.x = -move.x; move.y = -move.y; }
		
		transform.Translate(move*Time.deltaTime,Space.World);
		
		// Move To Player
		if( MoveToPlayer )
		{
			Vector3 delta = Vector3.zero;
			if(LevelInfo.State.wind == WindState.Down )
				delta = new Vector3(LevelInfo.Environments.transformSurfer.position.x>Center.x?1f:-1f,0f,0f);
			else
				delta = new Vector3(0f,LevelInfo.Environments.transformSurfer.position.y>Center.y?1f:-1f,0f);

			transform.Translate(delta*MoveToPlayerForce,Space.World);
		}
		
		if(RandomDirection)
			transform.Translate(randomDirection*RandomDirectionForce,Space.World);
	}
	
	#endregion
	
	#region Methods
	
	public void AddForce(Vector3 force)
	{
		StartCoroutine(AddForceThread(force));
	}
	
	private bool IsInScreenOff{
		get
		{
			float offset = LevelInfo.Camera.Height;
			return transform.position.x <= -offset || transform.position.x >= LevelInfo.Camera.Width+offset ||
				   transform.position.y <= -offset || transform.position.y >= LevelInfo.Camera.Height+offset;
		}
	}
	
	IEnumerator AddForceThread(Vector3 force)
	{
		while(force.magnitude>2f)
		{
			transform.Translate( (force/Mass)*Time.deltaTime,Space.World);
			force *= 0.95f;	
			yield return null;
		}
	}
	
	public Vector3 Center
	{
		get
		{
			if(!scaleOrigin)
				return new Vector3(transform.position.x - bound.center.x,transform.position.y + bound.center.y,transform.position.z);
			else
				return new Vector3(transform.position.x + bound.center.x,transform.position.y + bound.center.y,transform.position.z);
		}
	}
	
	public float Depth
	{
		get
		{
			return LevelInfo.Environments.depthObstacle-ExtraDepth;
		}
	}
	
	#endregion
}
