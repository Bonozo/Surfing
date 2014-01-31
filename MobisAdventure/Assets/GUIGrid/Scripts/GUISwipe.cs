using UnityEngine;
using System.Collections;

public class GUISwipe : GUISwitch
{
	public GUIPlane[] m_guiPlanes = new GUIPlane[0];
	public float m_swipeSpeed = 5.0f;
	public bool m_lerp = false;
	
	Vector3[] m_swipePositions = null;
	protected int m_swipeIndex = 0;
	private bool m_swipe = false;
	private bool m_swipeForward = false;
	private float m_epsilon = 0.001f;
	
	public virtual void Start()
	{
		m_swipePositions = new Vector3[m_guiPlanes.Length];
		for(int i = 0; i < m_guiPlanes.Length; ++i)
		{
			m_swipePositions[i] = m_guiPlanes[i].transform.position;
		}
	}
	
	public virtual void Update()
	{
		m_swipe = true;
		
		for(int i = 0; i < m_guiPlanes.Length; ++i)
		{
			int nextIndex = (i + m_swipeIndex) % m_guiPlanes.Length;
			if(nextIndex < 0)
				nextIndex = m_guiPlanes.Length + nextIndex;
			
			if(Vector3.Distance(m_guiPlanes[i].transform.position, m_swipePositions[nextIndex]) > m_epsilon)
				m_swipe = false;
			
			if(m_swipeForward)
			{
				if(nextIndex > 0)
					m_guiPlanes[i].transform.position = Move(m_guiPlanes[i].transform.position, m_swipePositions[nextIndex]);
				else
					m_guiPlanes[i].transform.position = m_swipePositions[nextIndex];
			}
			else
			{
				if(nextIndex < m_guiPlanes.Length - 1)
					m_guiPlanes[i].transform.position = Move(m_guiPlanes[i].transform.position, m_swipePositions[nextIndex]);
				else
					m_guiPlanes[i].transform.position = m_swipePositions[nextIndex];
			}
			
		}
	}
	
	public override bool TakeAction(int actionIndex, params object[] arguments)
	{
		if(m_disable)
			return false;
		
		switch(actionIndex)
		{
			case 0:
				SwipeForward();
				break;
			case 1:
				SwipeBack();
				break;
			default:
				break;
		}
		
		return true;
	}
	
	public Vector3 Move(Vector3 fromPos, Vector3 toPos)
	{
		Vector3 pos = fromPos;
		
		if(m_lerp)
			pos = Vector3.Lerp(pos, toPos, m_swipeSpeed * Time.deltaTime);
		else
			pos = Vector3.MoveTowards(pos, toPos, m_swipeSpeed * Time.deltaTime);
		
		return pos;
	}
	
	public void SwipeForward()
	{
		if(m_swipe)
		{
			m_swipeForward = true;
			++m_swipeIndex;
		}
	}
	
	public void SwipeBack()
	{
		if(m_swipe)
		{
			m_swipeForward = false;
			--m_swipeIndex;
		}
	}
}
