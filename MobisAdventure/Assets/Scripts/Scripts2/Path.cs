using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour
{
	public Curve.Type m_curveType = Curve.Type.QUADRATIC;
	public int m_numCurves = 1;
	public int m_numCurvePositions = 20;
	public float m_curveWidth = 10.0f;
	public float m_minCurveHeight = 10.0f;
	public float m_maxCurveHeight = 30.0f;
	public float m_peakHeight = 10.0f;
	public float m_surfaceFrequency = 0.3f;
	
	//Note: For debug purposes only.  Advised not to use.  Will lag game around 100 curve points...
	private bool m_showControlPoints = false;
	
	private Vector3[] m_pathPoints = new Vector3[0];
//	private Vector3[] m_pathDirections = new Vector3[0];
	public Vector3[] m_pathPositions = new Vector3[0];
	private GameObject[] m_curvePoints = new GameObject[0];
	
	private float m_timeStride = 0.0f;
	
	public delegate void PosDel(Vector3[] positions);

	void Start()
	{
		m_timeStride = 1.0f/m_numCurvePositions;
		CreatePath();
	}
	
	void Update()
	{
		//Updating position of curve points...
		for(int i = 0; i < m_curvePoints.Length; ++i)
			m_curvePoints[i].transform.position = m_pathPoints[i];
	}
	
	private void CreatePath()
	{
		//Allocating space for path positions and directions...
		m_pathPoints = new Vector3[m_numCurves * ((int)m_curveType - 1) + 1];
//		m_pathDirections = new Vector3[m_pathPoints.Length];
		m_pathPositions = new Vector3[m_numCurves * m_numCurvePositions + 1];
		
		//Allocating and creating curve points, if showCurvePoints set to true...
		if(m_showControlPoints)
		{
			m_curvePoints = new GameObject[m_pathPoints.Length];
			for(int i = 0; i < m_curvePoints.Length; ++i)
			{
				m_curvePoints[i] = Instantiate(Resources.Load("CurvePoint"), transform.position, transform.rotation) as GameObject;
				m_curvePoints[i].transform.parent = transform;
			}
		}
		
		//Placing points and positions...
		PlacePoints();
		PlacePositions();
	}
	
	//Places all curve points...
	private void PlacePoints()
	{
		int curveStep = (int)m_curveType - 1;
		
		Vector3 lastPoint = m_pathPoints[0] - Vector3.right - Vector3.up;
		Vector3 direction = Vector3.zero;
		
		bool start = true;
		
		float randomizedPeakHeight = 0;
		
		for(int i = 0; i < m_pathPoints.Length; ++i)
		{
			m_pathPoints[i] = transform.position + Vector3.right * m_curveWidth * i;
			m_pathPoints[i].y += randomizedPeakHeight;
			
			if(i % curveStep != 0)
			{
				if(start)
				{
					m_pathPoints[i] = lastPoint + direction;
					start = false;
				}
				else
				{
					m_pathPoints[i].y = m_pathPoints[i - 1].y;
				}
				
				lastPoint = m_pathPoints[i];
				continue;
			}
			else
				randomizedPeakHeight += Random.Range(-m_peakHeight, m_peakHeight);//Mathf.Sin(m_pathPoints[i].x) * 10.0f;
			
			start = true;
			direction = Vector3.Normalize(m_pathPoints[i] - lastPoint) * Random.Range(m_maxCurveHeight, m_minCurveHeight);
			lastPoint = m_pathPoints[i];
		}
	}
	
	//Places all curve positions...
	private void PlacePositions()
	{
		float runningTime = 0.0f;
		
		for(int i = 0; i < m_pathPositions.Length; ++i)
		{
			m_pathPositions[i] = GetPosition(runningTime);
			runningTime += m_timeStride;
		}
		
		Vector3[] randomizedPositions = new Vector3[m_pathPositions.Length];
		
		////////////////////
		float lastFrequency = 1.0f;
		float peak = 1.0f;
		//////////////////////
		
		for(int i = 0; i < randomizedPositions.Length; ++i)
		{
			Vector3 moveDirection = GetNormal(m_pathPositions[i]);
			moveDirection = new Vector3(-moveDirection.y, moveDirection.x, moveDirection.z);
			
			////////////////////////////////////
			float surfaceFrequency = Mathf.Sin(m_pathPositions[i].x * m_surfaceFrequency) * peak;
			
			//Comparing if switched between positive and negative...
			if(lastFrequency * surfaceFrequency < 0)
				peak = Random.Range(0.0f, 1.0f);
			
			lastFrequency = surfaceFrequency;
			/////////////////////////////////////
			
			randomizedPositions[i] = m_pathPositions[i] + moveDirection * surfaceFrequency;
		}
		
		//Resetting first and last index back to default so that mesh binds smoothly when swaping meshes...
		randomizedPositions[0] = m_pathPositions[0];
		randomizedPositions[randomizedPositions.Length - 1] = m_pathPositions[randomizedPositions.Length - 1];
		m_pathPositions = randomizedPositions;
	}
	
	//Returns position on path, given any time...
	public Vector3 GetPosition(float time)
	{
		Vector3 position = Vector3.zero;
		int index = GetPathIndex(time, 1) * ((int)m_curveType - 1);
		
		if(index < 0)
			return position;
		
		switch(m_curveType)
		{
			case Curve.Type.QUADRATIC:
				if(index + 2 < m_pathPoints.Length)
					Curve.SetPosOnQuadraticCurve(out position, time % 1.0f, m_pathPoints[index], m_pathPoints[index + 1],
						m_pathPoints[index + 2]);
				else
					position = m_pathPoints[index];
				break;
			case Curve.Type.CUBIC:
				if(index + 3 < m_pathPoints.Length)
					Curve.SetPosOnCubicCurve(out position, time % 1.0f, m_pathPoints[index], m_pathPoints[index + 1],
						m_pathPoints[index + 2], m_pathPoints[index + 3]);
				else
				{
					if(index < m_pathPoints.Length)
						position = m_pathPoints[index];
				}
				break;
			default:
				break;
		}
		
		return position;
	}
	
	public Vector3 GetNormal(Vector3 position)
	{
		return GetNormal(GetRunningTime(position));
	}
	
	public Vector3 GetNormal(float time)
	{
		//Debug.Log("Time: " + time + ", Time Multiplier: " + m_timeMultiplier + " Time Stride: " + m_pathPositions[0] + " | " + m_pathPositions[m_pathPositions.Length - 1]);
		Vector3 pointA = GetPosition(time - m_timeStride);
		Vector3 pointB = GetPosition(time + m_timeStride);
		
		Vector3 normal = Vector3.Normalize(pointB - pointA);
		if(normal == Vector3.zero)
			return Vector3.right;
		
		return normal;
		
	}
	
	//Gets height of position along path...
	public bool GetHeight(Vector3 position, out float height)
	{
		height = GetPosition(GetRunningTime(position)).y;
		
		if(position.y < height)
			return true;
		
		return false;
	}
	
	public Vector3[] GetCurve(int index, int numCurves)
	{
		Vector3[] curve = new Vector3[m_numCurvePositions * numCurves + 1];
		int startingIndex = GetRunningCurveIndex(GetPathPoint(index, numCurves));
		
		for(int i = 0; i < curve.Length; ++i)
			curve[i] = m_pathPositions[startingIndex + i];
		
		return curve;
	}
	
	public int GetRunningCurveIndex(Vector3 position)
	{
		int index = (int)(GetRunningTime(position) * m_numCurvePositions);
		
		return index;
	}
	
	public int GetPathIndex(Vector3 position, int numCurvesPerMesh)
	{
		int index = (int)(GetRunningTime(position)/numCurvesPerMesh);
		
		return index;
	}
	
	public int GetPathIndex(float time, int numOfStrides)
	{
		int index = (int)time * numOfStrides;
		
		return index;
	}
	
	public float GetTimeOnPath(Vector3 position)
	{
		float time = GetDistanceFromStartOfPath(position)/GetCurveLength();
		
		return time;
	}
	
	public float GetTimeOnCurve(Vector3 position)
	{
		return GetRunningTime(position) % 1.0f;
	}
	
	public float GetRunningTime(Vector3 position)
	{
		float timeOnCurve = 1.0f/m_numCurves;
		float timeOnPath = GetTimeOnPath(position);
		float time = timeOnPath/timeOnCurve;
		
		return time;
	}
	
	public Vector3 GetCurveEndPoint(int index)
	{
		return m_pathPoints[((int)m_curveType - 1) * index];
	}
	
	public float GetCurveLength()
	{
		return m_pathPoints[m_pathPoints.Length - 1].x - m_pathPoints[0].x;
	}
	
	public float GetDistanceFromStartOfPath(Vector3 position)
	{
//		Debug.Log("blac");
		return position.x - m_pathPoints[0].x;
	}
	
	public float GetDistanceFromEndOfPath(Vector3 position)
	{
		return m_pathPoints[m_pathPoints.Length - 1].x - position.x;
	}
	
	public Vector3 GetPathPoint(int index, int numCurvesTaken)
	{
		int curveIndex = GetPathIndex(index, (int)Path.Current.m_curveType - 1) * numCurvesTaken;
//		Debug.Log(index);
		return m_pathPoints[curveIndex];
	}
	
	public int NumCurves
	{
		get { return m_numCurves; }
	}
	
	public int NumCurvePositions
    {
        get { return m_numCurvePositions; }
    }
	
	public void SetPoints(PosDel posDel)
	{
		posDel(m_pathPoints);
	}
	
	public void SetPositions(PosDel posDel)
	{
		posDel(m_pathPositions);
	}
	
	public static Path Current
    {
        get { return m_path; }
    }

	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile Path _staticInstance;	
	public static Path m_path 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(Path)) as Path;
					if (_staticInstance == null) {
						Debug.LogError("The Path instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
	
	#endregion
}
