using UnityEngine;
using System.Collections;

public class PathPlatform : MonoBehaviour
{
	public Transform m_target = null;
	public bool m_followTarget = false;
	public float m_platformSpeed = 60.0f;
	public PathSystem m_pathSystem = null;
	
	private float m_time = 1.0f;
	
	private Vector3 m_lastPos = Vector3.zero;
	
	public static PathPlatform m_pathPlatform = null;
	
	void Awake()
	{
		m_pathPlatform = this;
	}
	
	// Use this for initialization
	void Start()
	{
		m_lastPos = transform.position;
		transform.position = m_pathSystem.GetPosOnPath(m_time);
		transform.forward = Vector3.right;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if(m_followTarget)
			FollowTarget();
		else
			MovePlatform();
		
		m_target.transform.right = transform.forward;
	}
	
	void MovePlatform()
	{
		transform.position = m_pathSystem.UpdatePosOnPath(m_time, out m_time);

		m_time += (Input.GetAxis("Horizontal") * m_platformSpeed * Time.fixedDeltaTime)/m_pathSystem.SpeedMultiplier;
		
		if(transform.position != m_lastPos && Input.GetAxis("Horizontal") != 0)
			transform.forward = Vector3.Normalize((transform.position - m_lastPos));
		
		m_lastPos = transform.position;
	}
	
	void FollowTarget()
	{
		transform.position = m_pathSystem.UpdatePosOnPath(m_time, out m_time);
		
		float distance = Mathf.Abs(transform.position.x - m_target.position.x);
		
		if(distance > 0.1f)
		{
			if(transform.position.x < m_target.position.x)
				m_time += (Mathf.Pow(distance, 10.0f) * Time.fixedDeltaTime)/m_pathSystem.SpeedMultiplier;
			else
				m_time -= (Mathf.Pow(distance, 10.0f) * Time.fixedDeltaTime)/m_pathSystem.SpeedMultiplier;
		}
		
		Vector3 forward = Vector3.Normalize((transform.position - m_lastPos));
		
		if(forward.x > 0.0f)
			transform.forward = forward;
		
		m_lastPos = transform.position;
	}
}
