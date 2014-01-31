using UnityEngine;
using System.Collections;

public class VehicleSpring : MonoBehaviour
{
	public WheelCollider m_wheel = null;
	public string nameWheel = "_BackWheel";
	private float m_posDiff;
	
	// Use this for initialization
	void Start()
	{
		if(!m_wheel)
			m_wheel = GameObject.Find(nameWheel).GetComponent<WheelCollider>();
	}
	
	void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(m_wheel.transform.position, -m_wheel.transform.up, out hit, m_wheel.suspensionDistance + m_wheel.radius)){
			transform.position = hit.point + (m_wheel.transform.up * m_wheel.radius);
	
		} else {
			transform.position = m_wheel.transform.position - (m_wheel.transform.up * m_wheel.suspensionDistance);

		}

		
	}

}
