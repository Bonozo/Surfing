using UnityEngine;
using System.Collections;

public class GUIToggle : GUISwitch
{
	public GUIPlane[] m_guiPlanes = new GUIPlane[0];
	
	private int m_toggleIndex = 0;
	
	public void Awake()
	{
		SetActive();
	}
	
	public override bool TakeAction(int actionIndex, params object[] arguments)
	{
		if(m_disable)
			return false;
		
		switch(actionIndex)
		{
			case 0:
				ToggleThrough();
				break;
			case 1:
				return ToggleUp();
			case 2:
				return ToggleDown();
			default:
				break;
		}
		
		return true;
	}
	
	public void ToggleThrough()
	{
		m_toggleIndex = (m_toggleIndex + 1) % m_guiPlanes.Length;
		SetActive();
	}
	
	public bool ToggleUp()
	{
		if(m_toggleIndex == m_guiPlanes.Length - 1)
			return false;
		
		m_toggleIndex = Mathf.Min(m_toggleIndex + 1, m_guiPlanes.Length - 1);
		SetActive();
		
		return true;
	}
	
	public bool ToggleDown()
	{
		if(m_toggleIndex == 0)
			return false;
		
		m_toggleIndex = Mathf.Max(m_toggleIndex - 1, 0);
		SetActive();
		
		return true;
	}
	
	public void SetActive()
	{
		for(int i = 0; i < m_guiPlanes.Length; ++i)
		{
			if(i == m_toggleIndex)
				m_guiPlanes[i].gameObject.SetActive(true);
			else
				m_guiPlanes[i].gameObject.SetActive(false);
		}
	}
}
