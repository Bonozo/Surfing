using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CurveMesh : MonoBehaviour
{
	public enum MeshType { PLANE, FLAT_TERRAIN, CURVED_TERRAIN, COLLIDABLE_SURFACE };
	
	public MeshType m_meshType = MeshType.CURVED_TERRAIN;
	public Vector2 m_uvAnimationRate = Vector2.zero;
	public bool m_posPointsRelativeToLocalPosition = false;
	public int m_depth = 0;
	
	[HideInInspector]public int m_numCurves = 1;
	[HideInInspector]public int m_curveIndex = 0;
	[HideInInspector]public float m_meshSize = 0.0f;
	
	private Mesh m_mesh = null;
	
	List<Vector3> m_vertices = new List<Vector3>();
	List<Vector3> m_normals = new List<Vector3>();
	List<Vector2> m_uvs = new List<Vector2>();
	List<int> m_triangles = new List<int>();
	
	private Vector2 m_uvOffset = Vector2.zero;
	
	// Use this for initialization
	void Start()
	{
		switch(m_meshType)
		{
			case MeshType.PLANE:
				break;
			case MeshType.FLAT_TERRAIN:
				CreateFlatTerrain();
				break;
			case MeshType.CURVED_TERRAIN:
				CreateCurvedTerrain();
				break;
			case MeshType.COLLIDABLE_SURFACE:
				CreateCollidableSurface();
				break;
			default:
				break;
		}
	}
	
	void Update()
	{
		m_uvOffset += (m_uvAnimationRate * Time.deltaTime);
		renderer.material.SetTextureOffset("_MainTex", m_uvOffset);
	}
	
	public void BuildMesh()
	{
		m_mesh = MeshMaker.Make(m_vertices.ToArray(), m_normals.ToArray(), m_uvs.ToArray(), m_triangles.ToArray());
		GetComponent<MeshFilter>().mesh = m_mesh;
	}
	
	public void RecreateVerticesAndUVs(int curveIndex)
	{
		m_curveIndex = curveIndex;
		
		if(CreateVerticesAndUVs())
		{
			m_mesh.vertices = m_vertices.ToArray();
			m_mesh.uv = m_uvs.ToArray();
			
			if(m_meshType == MeshType.COLLIDABLE_SURFACE)
				gameObject.AddComponent(typeof(MeshCollider));
		}
	}
	
	bool CreateVerticesAndUVs()
	{
		Vector3[] pathPositions = Path.Current.GetCurve(m_curveIndex, m_numCurves);
		if(pathPositions == null)
			return false;
		
		m_vertices.Clear();
		m_uvs.Clear();
		
		m_meshSize = Vector3.Distance(pathPositions[0], pathPositions[pathPositions.Length - 1]);
		
		if(m_meshType == MeshType.FLAT_TERRAIN)
		{
			float uStride = 1.0f/(pathPositions.Length - 1);

			Vector3 bottomPos = (-Vector3.up * m_meshSize)/2.0f;
			float[] vStride = new float[2];
			
			//Makes sure that mesh points start at local positions origin...
			Vector3 offsetFromOrigin = Vector3.zero;
			if(m_posPointsRelativeToLocalPosition)
				offsetFromOrigin = -transform.position;
			
			//Creating Vertices...
			for(int i = 0; i < pathPositions.Length - 1; ++i)
			{ 
				vStride[0] = (bottomPos.y - pathPositions[i].y)/m_meshSize;
				vStride[1] = (bottomPos.y - pathPositions[i + 1].y)/m_meshSize;
				
				//Vertices...
				m_vertices.Add(pathPositions[i + 1] + new Vector3(0.0f, 0.0f, m_depth) + offsetFromOrigin);
				m_vertices.Add(new Vector3(pathPositions[i].x, bottomPos.y, pathPositions[i].z + m_depth) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i] + new Vector3(0.0f, 0.0f, m_depth) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i + 1] + new Vector3(0.0f, 0.0f, m_depth) + offsetFromOrigin);
				m_vertices.Add(new Vector3(pathPositions[i + 1].x, bottomPos.y, pathPositions[i + 1].z + m_depth) + offsetFromOrigin);
				m_vertices.Add(new Vector3(pathPositions[i].x, bottomPos.y, pathPositions[i].z + m_depth) + offsetFromOrigin);
				
				//UVs...
				m_uvs.Add(new Vector2(uStride * i + uStride, vStride[1]));
				m_uvs.Add(new Vector2(uStride * i, 0));
				m_uvs.Add(new Vector2(uStride * i, vStride[0]));
				m_uvs.Add(new Vector2(uStride * i + uStride, vStride[1]));
				m_uvs.Add(new Vector2(uStride * i + uStride, 0));
				m_uvs.Add(new Vector2(uStride * i, 0));
			}
			
			return true;
		}
		else if(m_meshType == MeshType.CURVED_TERRAIN)
		{
			float uStride = 1.0f/(pathPositions.Length - 1);
			float vStride = 1.0f;
			
			//Makes sure that mesh points start at local positions origin...
			Vector3 offsetFromOrigin = Vector3.zero;
			if(m_posPointsRelativeToLocalPosition)
				offsetFromOrigin = -transform.position;
			
			//Creating Vertices...
			for(int i = 0; i < pathPositions.Length - 1; ++i)
			{
				//Vertices...
				m_vertices.Add(pathPositions[i + 1] + new Vector3(0.0f, 0.0f, m_depth) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i] + new Vector3(0.0f, -m_meshSize, m_depth) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i] + new Vector3(0.0f, 0.0f, m_depth) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i + 1] + new Vector3(0.0f, 0.0f, m_depth) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i + 1] + new Vector3(0.0f, -m_meshSize, m_depth) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i] + new Vector3(0.0f, -m_meshSize, m_depth) + offsetFromOrigin);
				
				//UVs...
				m_uvs.Add(new Vector2(uStride * i + uStride, vStride));
				m_uvs.Add(new Vector2(uStride * i, 0));
				m_uvs.Add(new Vector2(uStride * i, vStride));
				m_uvs.Add(new Vector2(uStride * i + uStride, vStride));
				m_uvs.Add(new Vector2(uStride * i + uStride, 0));
				m_uvs.Add(new Vector2(uStride * i, 0));
			}
			
			return true;
		}
		else if(m_meshType == MeshType.COLLIDABLE_SURFACE)
		{
			//Makes sure that mesh points start at local positions origin...
			Vector3 offsetFromOrigin = Vector3.zero;
			if(m_posPointsRelativeToLocalPosition)
				offsetFromOrigin = -transform.position;
			
			//Creating Vertices...
			for(int i = 0; i < pathPositions.Length - 1; ++i)
			{
				//Vertices...
				m_vertices.Add(pathPositions[i + 1] + offsetFromOrigin);
				m_vertices.Add(pathPositions[i] + offsetFromOrigin);
				m_vertices.Add(pathPositions[i] + new Vector3(0.0f, 0.0f, 25.0f) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i + 1] + offsetFromOrigin);
				m_vertices.Add(pathPositions[i] + new Vector3(0.0f, 0.0f, 25.0f) + offsetFromOrigin);
				m_vertices.Add(pathPositions[i + 1] + new Vector3(0.0f, 0.0f, 25.0f) + offsetFromOrigin);
				
				//UVs...
				m_uvs.Add(Vector2.zero);
				m_uvs.Add(Vector2.zero);
				m_uvs.Add(Vector2.zero);
				m_uvs.Add(Vector2.zero);
				m_uvs.Add(Vector2.zero);
				m_uvs.Add(Vector2.zero);
			}
			
			DestroyImmediate(GetComponent<MeshCollider>());
			
			return true;
		}
		
		return false;
	}
	
	void CreateFlatTerrain()
	{	
		if(!CreateVerticesAndUVs())
			return;
		
		for(int i = 0; i < m_vertices.Count; ++i)
		{
			m_normals.Add(new Vector3(0, 0, 1));
			m_triangles.Add(i);
		}
		
		BuildMesh();
	}
	
	void CreateCurvedTerrain()
	{	
		if(!CreateVerticesAndUVs())
			return;
		
		for(int i = 0; i < m_vertices.Count; ++i)
		{
			m_normals.Add(new Vector3(0, 0, 1));
			m_triangles.Add(i);
		}
		
		BuildMesh();
	}
	
	void CreateCollidableSurface()
	{	
		if(!CreateVerticesAndUVs())
			return;
		
		for(int i = 0; i < m_vertices.Count; ++i)
		{
			m_normals.Add(Path.Current.GetNormal(m_vertices[i]));
			m_triangles.Add(i);
		}
		
		BuildMesh();
		gameObject.layer = 10;
		gameObject.AddComponent(typeof(MeshCollider));
	}
}
