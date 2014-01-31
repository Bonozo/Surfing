using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour
{
	public Transform m_target = null;
	
	private Vector3 m_offset = Vector3.zero;
	
	// Use this for initialization
	void Start()
	{
		m_offset = transform.position - m_target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		transform.position = m_target.position + m_offset;
	}
}
