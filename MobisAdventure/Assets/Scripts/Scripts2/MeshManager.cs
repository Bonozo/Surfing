using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshManager : MonoBehaviour
{
	[System.Serializable]
	public class MeshLayer
	{   
		public CurveMesh.MeshType m_meshType = CurveMesh.MeshType.CURVED_TERRAIN;
		public Vector2 m_uvScrollVelocity = Vector2.zero;
		public Material m_material = null;
	}
	
	public class CurveSet
	{
		public CurveMesh m_collidableMesh = null;
		public List<CurveMesh> m_groundMeshes = new List<CurveMesh>();
		
		public void ReCreateVerticesAndUVs(int curveIndex)
		{
			m_collidableMesh.RecreateVerticesAndUVs(curveIndex);
			foreach(CurveMesh curveMesh in m_groundMeshes)
				curveMesh.RecreateVerticesAndUVs(curveIndex);
		}
		
		public Vector3 GetPosition()
		{
			return m_collidableMesh.transform.position;
		}
		
		public void SetPosition(Vector3 position)
		{	
			m_collidableMesh.transform.position = position;
			foreach(CurveMesh curveMesh in m_groundMeshes)
				curveMesh.transform.position = position;
		}
	}
	
	public delegate void PosDel(int curveIndex);
	
	public int m_numCurveMeshes = 3;
	public int m_numCurvesTaken = 8;
	
	public MeshLayer[] m_groundMeshes = new MeshLayer[2];
	public bool m_createCollidableSurface = false;
	
	[HideInInspector] public int m_lastPathIndex = 1;
	
	private CurveSet[] m_curveSets = null;
	private CurveSet[] m_nextCurveSets = null;
	
	private List<PosDel> m_positionDeligates = new List<PosDel>();
	private static MeshManager m_meshManager = null;
	
	void Awake()
	{
		m_meshManager = this;
		Path.Current.m_numCurves = Path.Current.m_numCurves * m_numCurvesTaken;
		//m_curveSets = new CurveSet[Path.Current.NumCurves/m_numCurvesTaken];
		m_curveSets = new CurveSet[m_numCurveMeshes];
		m_nextCurveSets = new CurveSet[m_curveSets.Length];
	}
	
	// Use this for initialization
	void Start()
	{
		GameObject gameObj = null;
		CurveMesh curveMesh = null;
		
		//Creating ground meshes...
		for(int i = 0; i < m_curveSets.Length; ++i)
		{
			CurveSet curveSet = new CurveSet();
			Vector3 position = Path.Current.GetCurveEndPoint(Path.Current.GetPathIndex(i, m_numCurvesTaken));
			
			for(int j = 0; j < m_groundMeshes.Length; ++j)
			{	
				gameObj = GameObject.Instantiate(Resources.Load("CurveMesh"), position, transform.rotation) as GameObject;
				curveMesh = gameObj.GetComponent<CurveMesh>() as CurveMesh;
				curveMesh.m_meshType = m_groundMeshes[j].m_meshType;
				curveMesh.renderer.material = m_groundMeshes[j].m_material;
				curveMesh.m_posPointsRelativeToLocalPosition = true;
				curveMesh.m_numCurves = m_numCurvesTaken;
				curveMesh.m_depth -= j;
				curveMesh.m_curveIndex = i;
				curveMesh.m_uvAnimationRate = m_groundMeshes[j].m_uvScrollVelocity;
				curveSet.m_groundMeshes.Add(curveMesh);
				gameObj.transform.parent = transform;
			}
			
			//Creating collidable Surfaces...
			gameObj = GameObject.Instantiate(Resources.Load("CurveMesh"), position, transform.rotation) as GameObject;
			curveMesh = gameObj.GetComponent<CurveMesh>() as CurveMesh;
			curveMesh.m_meshType = CurveMesh.MeshType.COLLIDABLE_SURFACE;
			curveMesh.renderer.material = new Material(Shader.Find("Diffuse"));
			curveMesh.m_posPointsRelativeToLocalPosition = true;
			curveMesh.m_numCurves = m_numCurvesTaken;
			curveMesh.m_curveIndex = i;
			gameObj.transform.parent = transform;
			
			curveSet.m_collidableMesh = curveMesh;
			m_curveSets[i] = curveSet;
		}
	}
	
	void Update()
	{
		SwapCurves();
	}
	
	void SwapCurves()
	{
		int pathIndex = Path.Current.GetPathIndex(Player.Instance.transform.position, m_numCurvesTaken);
		
		if(pathIndex > 0)
		{
			if(m_lastPathIndex < pathIndex)
				MoveBackToFront(pathIndex);
			else if(m_lastPathIndex > pathIndex)
				MoveFrontToBack(pathIndex);
			
			m_lastPathIndex = pathIndex;
			//Debug.Log("Path Index: " + pathIndex + ", Last Index: " + m_lastPathIndex);
		}
	}
	
	void MoveFrontToBack(int curveIndex)
	{
		int index = m_curveSets.Length - 1;
		int nextIndex = 0;
		m_nextCurveSets[nextIndex] = m_curveSets[index];
		m_nextCurveSets[nextIndex].SetPosition(Path.Current.GetPathPoint(curveIndex - 1, m_numCurvesTaken));
		m_nextCurveSets[nextIndex].ReCreateVerticesAndUVs(curveIndex - 1);
		
		for(int i = 1; i < m_curveSets.Length; ++i)
			m_nextCurveSets[i] = m_curveSets[i - 1];
		
		for(int i = 0; i < m_curveSets.Length; ++i)
			m_curveSets[i] = m_nextCurveSets[i];
		
		foreach(PosDel posDel in m_positionDeligates)
			posDel(curveIndex);
	}
	
	void MoveBackToFront(int curveIndex)
	{
		int index = 0;
		int nextIndex = m_curveSets.Length - 1;
		m_nextCurveSets[nextIndex] = m_curveSets[index];
		m_nextCurveSets[nextIndex].SetPosition(Path.Current.GetPathPoint(curveIndex + 1, m_numCurvesTaken));
		m_nextCurveSets[nextIndex].ReCreateVerticesAndUVs(curveIndex + 1);
		
		for(int i = 0; i < nextIndex; ++i)
			m_nextCurveSets[i] = m_curveSets[i + 1];
		
		for(int i = 0; i < m_curveSets.Length; ++i)
			m_curveSets[i] = m_nextCurveSets[i];
		
		foreach(PosDel posDel in m_positionDeligates)
			posDel(curveIndex);
	}
	
	public void SetCurvePositions(PosDel posDel)
	{
		m_positionDeligates.Add(posDel);
		//posDel(m_pathPoints);
	}
	
	public static MeshManager Current
    {
        get { return m_meshManager; }
    }
}
