using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour
{
	public Transform m_target = null;
	public float m_kSpring = 100.0f;
	public float m_bDamper = 5.0f;
	
	private Vector3 m_offset = Vector3.zero;
	
	// Use this for initialization
	void Start()
	{
		m_offset = transform.position - m_target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{ 
		Vector3 restingPosition = m_target.position + m_offset;
		Vector3 restingOffset = transform.position - restingPosition;
		
		Vector3 springForce = -m_kSpring * restingOffset;
		Vector3 dampingForce = m_bDamper * rigidbody.velocity;
		
		rigidbody.AddForce(springForce - dampingForce);
	}
}
