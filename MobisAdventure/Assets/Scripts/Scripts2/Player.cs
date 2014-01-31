using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public GUIPlane[] m_scrollingGUIPlanes = new GUIPlane[0];
	
	private static Player m_player = null;
	
	private bool m_touchDown = false;
	
	private Quaternion m_startingRotation = Quaternion.identity;
	
	void Awake()
	{
		m_player = this;
		m_startingRotation = transform.rotation;
	}
	
	void Update()
	{
		foreach(GUIPlane guiPlane in m_scrollingGUIPlanes)
			guiPlane.m_uvAnimationRate.x = Velocity.x * Time.deltaTime * 0.2f;
	}
	
	// Subscribe to events
	void OnEnable()
	{
		EasyTouch.On_TouchStart += On_TouchStart;
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
	}
	
	// Unsubscribe
	void OnDisable()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}
	
	// Unsubscribe
	void OnDestroy()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}
	
	// Touch start event
	public void On_TouchStart(Gesture gesture)
	{
		//Debug.Log( "On " + gesture.position);
	}
	
	// Touch start event
	public void On_TouchUp(Gesture gesture)
	{
		//Debug.Log( "Off " + gesture.position);
		m_touchDown = false;
	}
	
	// Touch start event
	public void On_TouchDown(Gesture gesture)
	{
		//Debug.Log( "Off " + gesture.position);
		
		m_touchDown = true;
		
		//m_velocitySpeed += m_speed * Time.fixedDeltaTime;
	}
	
	public static Player Instance
    {
        get { return m_player; }
    }
	
	public Vector2 Velocity
    {
        get { return new Vector2(rigidbody.velocity.x, rigidbody.velocity.y); }
    }
	
	public bool TouchDown
    {
        get { return m_touchDown; }
    }
	
	public Quaternion StartingRotation
    {
        get { return m_startingRotation; }
    }
}
