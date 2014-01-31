using UnityEngine;
using System.Collections;

public class PathSystem : MonoBehaviour
{
	public enum CurveType { LINEAR = 1, QUADRATIC = 2, CUBIC = 3};
	
	public CurveType m_curveType = CurveType.QUADRATIC;
	public int m_numWaves = 10;
	public float m_minWaveLength = 1.0f;
	public float m_maxWaveLength = 6.0f;
	
	public PathPoint m_startingPathPoint = null;
	public bool m_renderPoints = false;
	
	private float m_speedMultiplier = 0.0f;
	
	private PathPoint[] m_pathPoints = new PathPoint[4];
	
	void Awake()
	{
		GeneratePathPoints();
	}
	
	private void GeneratePathPoints()
	{
		//Creating first path point...
		m_startingPathPoint = CreatePathPoint(transform.position);
		m_startingPathPoint.m_dirToNextPathPoint = Vector3.Normalize(Vector3.right - Vector3.up);
		m_startingPathPoint.transform.parent = transform;
		PathPoint pathPoint = m_startingPathPoint;
		
		for(int i = 0; i < m_numWaves; ++i)
		{
			if(m_curveType == CurveType.QUADRATIC)
				pathPoint = CreateQuadraticSet(pathPoint, Random.Range(m_minWaveLength, m_maxWaveLength));
			else
				pathPoint = CreateCubicSet(pathPoint, Random.Range(m_minWaveLength, m_maxWaveLength));
		}
		
		SetPathPoints(m_startingPathPoint);
	}
	
	
	//Note: This is somewhat hacked!  May want to change later...
	public PathPoint CreateQuadraticSet(PathPoint pathPoint, float waveLength)
	{
		Vector3 pos = pathPoint.transform.position;
		pathPoint = CreateNextPathPoint(pathPoint, pos + pathPoint.m_dirToNextPathPoint * waveLength);
		
		pos.y += Random.Range(-m_minWaveLength, m_minWaveLength);
		pathPoint = CreateNextPathPoint(pathPoint, pos + Vector3.right * (waveLength + Random.Range(0, m_minWaveLength)));
		
		return pathPoint;
	}
	
	//Note: This is somewhat hacked!  May want to change later...
	public PathPoint CreateCubicSet(PathPoint pathPoint, float waveLength)
	{
		Vector3 pos = pathPoint.transform.position;
		pathPoint = CreateNextPathPoint(pathPoint, pos + pathPoint.m_dirToNextPathPoint * waveLength);
		
		pathPoint = CreateNextPathPoint(pathPoint, pathPoint.transform.position + Vector3.right * waveLength);
		
		pos.x = pathPoint.transform.position.x;
		pos.y += Random.Range(-waveLength, m_minWaveLength);
		pathPoint = CreateNextPathPoint(pathPoint, pos + pathPoint.m_dirToNextPathPoint * waveLength);
		
		return pathPoint;
	}
	
	private PathPoint CreateNextPathPoint(PathPoint pathPoint, Vector3 position)
	{
		PathPoint nextPathPoint = CreatePathPoint(position);
		pathPoint.NextPathPoint = nextPathPoint;
		nextPathPoint.LastPathPoint = pathPoint;

		nextPathPoint.m_dirToNextPathPoint = Vector3.Normalize(nextPathPoint.transform.position -
			pathPoint.transform.position);
		
		nextPathPoint.transform.parent = transform;
		return nextPathPoint;
	}
	
	private PathPoint CreatePathPoint(Vector3 position)
	{
		GameObject pathPoint = Instantiate(Resources.Load("PathPoint"),
			position, Quaternion.identity) as GameObject;
		
		pathPoint.renderer.enabled = m_renderPoints;
		
		return pathPoint.GetComponent<PathPoint>();
	}
	
	public Vector3 GetPosOnPath(float time)
	{
		/*
		for(int i = 0, size = (int)time; i < size; ++i)
		{
			
		}*/
		
		Vector3 pos;
		SetPosOnCurve(out pos, time);
		return pos;
	}
	
	public void SetPosOnCurve(out Vector3 pos, float time)
	{
		if(m_curveType == CurveType.QUADRATIC)
		{
			//P(t) = (1 - t)^2 P0  +  2t(1 - t) P1  +  t^2 P2
			pos = Mathf.Pow(1 - time, 2) * m_pathPoints[0].transform.position;
			pos += 2 * time * (1 - time) * m_pathPoints[1].transform.position;
			pos += Mathf.Pow(time, 2) * m_pathPoints[2].transform.position;
		}
		else
		{
			//P(t) = (1 - t)^3 P0  +  3t(1 - t)^2 P1  +  3t^2 (1 - t) P2 + t^3 P3
			pos = Mathf.Pow(1 - time, 3) * m_pathPoints[0].transform.position;
			pos += 3 * time * Mathf.Pow(1 - time, 2) * m_pathPoints[1].transform.position;
			pos += 3 * Mathf.Pow(time, 2) * (1 - time) * m_pathPoints[2].transform.position;
			pos += Mathf.Pow(time, 3) * m_pathPoints[3].transform.position;
		}
	}
	
	public void SetPathPoints(PathPoint pathPoint)
	{
		m_pathPoints[0] = pathPoint;
		m_pathPoints[1] = m_pathPoints[0].NextPathPoint;
		m_pathPoints[2] = m_pathPoints[1].NextPathPoint;
		m_pathPoints[3] = m_pathPoints[2].NextPathPoint;
		
		m_speedMultiplier = Vector3.Distance(m_pathPoints[1].transform.position, m_pathPoints[0].transform.position);
		m_speedMultiplier += Vector3.Distance(m_pathPoints[2].transform.position, m_pathPoints[1].transform.position);
		
		if(m_curveType == CurveType.CUBIC)
			m_speedMultiplier += Vector3.Distance(m_pathPoints[3].transform.position, m_pathPoints[2].transform.position);
	}
	
	public Vector3 UpdatePosOnPath(float time, out float newTime)
	{	
		newTime = time;
		
		if(newTime > 1.0f)
		{
			if(m_pathPoints[(int)m_curveType].NextPathPoint)
			{
				SetPathPoints(m_pathPoints[(int)m_curveType]);
				newTime %= 1.0f;
			}
			else
			{
				newTime = 1.0f;
			}
		}
		else if(newTime < 0.0f)
		{
			if(m_pathPoints[0].LastPathPoint != null)
			{
				if(m_curveType == CurveType.QUADRATIC)
					SetPathPoints(m_pathPoints[0].LastPathPoint.LastPathPoint);
				else
					SetPathPoints(m_pathPoints[0].LastPathPoint.LastPathPoint.LastPathPoint);
				
				newTime = 1.0f - (newTime % 1.0f);
			}
			else
			{
				newTime = 0.0f;
			}
		}
		
		return GetPosOnPath(newTime);
	}
	
	public PathPoint GetPathPoint(int index)
	{
		return m_pathPoints[Mathf.Min(index, (int)m_curveType)];
	}
	
	public int NumOfWaves
    {
        get { return m_numWaves; }
    }
	
	public float SpeedMultiplier
	{
		get { return m_speedMultiplier; }	
	}
	
	public PathPoint StartingPathPoint
    {
        get { return m_startingPathPoint; }
    }
}
