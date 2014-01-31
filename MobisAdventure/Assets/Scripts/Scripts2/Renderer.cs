using UnityEngine;
using System.Collections;

public class Renderer : MonoBehaviour
{
	public bool m_on = false;
	
	// Use this for initialization
	void Start()
	{
		renderer.enabled = m_on;
	}
}
