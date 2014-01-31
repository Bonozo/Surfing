using UnityEngine;
using System.Collections;

public class ReloadLevel : MonoBehaviour
{
	public int m_levelIndex = 0;
	public int m_loadTime = 3;
	
	private float m_startTime = 0.0f;
	
	void OnTriggerEnter(Collider other)
	{
		m_startTime = Time.timeSinceLevelLoad;
    }
	
	//New Code...
	void Update()
	{
		if(m_startTime > 0.0f)
		{
			int secondsToLoad = GameManager.Instance.GetSeconds(m_startTime);
		
			if(secondsToLoad == m_loadTime)
				Application.LoadLevel(m_levelIndex);
		}
		
		//transform.Rotate(rotProduct * 1.0f, 0.0f, 0.0f);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(1.0f, 0.0f, 0.0f), 90.0f);
	}
	
	/*
	//Old Code...
	void OnTriggerStay(Collider other)
	{
		int secondsToLoad = GameManager.Instance.GetSeconds(m_startTime);
		
		if(secondsToLoad == m_loadTime)
			Application.LoadLevel(m_levelIndex);
    }*/
}
