using UnityEngine;
using System.Collections;

public class PathPoint : MonoBehaviour
{
	public PathPoint m_nextPathPoint = null;
	public PathPoint m_lastPathPoint = null;
	public Vector3 m_dirToNextPathPoint = Vector3.right;
	
	public PathPoint NextPathPoint
    {
        get { return m_nextPathPoint; }
        set { m_nextPathPoint = value; }
    }
	
	public PathPoint LastPathPoint
    {
        get { return m_lastPathPoint; }
        set { m_lastPathPoint = value; }
    }
}
