using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainSystem : MonoBehaviour
{
	public LineRenderer m_lineRenderer = null;
	
	private PathSystem m_pathSystem = null;
	
	private int m_numOfPositions = 30;
	
	private float m_timeStride = 0.0f;
	
	private Vector3 m_currentPos = Vector3.zero;
	
	private List<Vector3> m_pathPositions = null;
	
	// Use this for initialization
	void Start()
	{
		m_timeStride = 1.0f/m_numOfPositions;
		m_pathSystem = GetComponent<PathSystem>();
		m_lineRenderer.SetVertexCount(m_numOfPositions * m_pathSystem.NumOfWaves);
		m_pathPositions = new List<Vector3>();
		
		RenderLine();
		CreateCurvedTerrain();
	}
	
	void RenderLine()
	{
		float fixedStride = 0.0f;
		
		for(int i = 0, size = m_numOfPositions * m_pathSystem.NumOfWaves; i < size; ++i)
		{
			m_currentPos = m_pathSystem.UpdatePosOnPath(fixedStride, out fixedStride);
			fixedStride += m_timeStride;
			
			m_currentPos.y = 0.0f;
			
			m_lineRenderer.SetPosition(i, m_currentPos + new Vector3(0.0f, 0.0f, -5.0f));
			m_pathPositions.Add(m_currentPos);
		}
	}
	
	void RenderTerrain()
	{	
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> triangles = new List<int>();
		
		float uvStride = 1/m_pathPositions.Count;
		
		for(int i = 0; i < m_pathPositions.Count - 1; ++i)
		{
			//Vertices...
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, -5.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 20.0f, -5.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 0.0f, -5.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, -5.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 20.0f, -5.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 20.0f, -5.0f));
			
			//UVs...
			uvs.Add(new Vector2(uvStride * i + uvStride, 0));
			uvs.Add(new Vector2(uvStride * i, 1));
			uvs.Add(new Vector2(uvStride * i, 0));
			uvs.Add(new Vector2(uvStride * i + uvStride, 0));
			uvs.Add(new Vector2(uvStride * i + uvStride, 1));
			uvs.Add(new Vector2(uvStride * i, 1));
		}
		
		for(int i = 0; i < vertices.Count; ++i)
		{
			normals.Add(new Vector3(0, 0, 1));
			triangles.Add(i);
		}
		
		GetComponent<MeshCreator>().BuildMesh(vertices, normals, uvs, triangles);
	}
	
	void CreateCurvedTerrain()
	{	
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> triangles = new List<int>();
		
		float curveLength = Vector3.Distance(m_pathPositions[0], m_pathPositions[m_pathPositions.Count - 1]);
		
		for(int i = 0; i < m_pathPositions.Count - 1; ++i)
		{
			//Vertices...
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, curveLength, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, curveLength, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, curveLength, 0.0f));
			
			//UVs...
			uvs.Add(new Vector2(m_timeStride * i + m_timeStride, 1));
			uvs.Add(new Vector2(m_timeStride * i, 0));
			uvs.Add(new Vector2(m_timeStride * i, 1));
			uvs.Add(new Vector2(m_timeStride * i + m_timeStride, 1));
			uvs.Add(new Vector2(m_timeStride * i + m_timeStride, 0));
			uvs.Add(new Vector2(m_timeStride * i, 0));
		}
		
		for(int i = 0; i < vertices.Count; ++i)
		{
			normals.Add(new Vector3(0, 0, 1));
			triangles.Add(i);
		}
		
		GetComponent<MeshCreator>().BuildMesh(vertices, normals, uvs, triangles);
	}
	
	void CreateTerrain()
	{	
		List<Vector3> vertices = new List<Vector3>();
		List<Vector3> normals = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<int> triangles = new List<int>();
		
		//float uvStride = 1.0f/m_pathPositions.Count;
		float uvStride = 1.0f/30;
		//Debug.Log("Position Count: " + m_pathPositions.Count + ", UV stride: " + uvStride);
		
		for(int i = 0; i < m_pathPositions.Count - 1; ++i)
		{
			//Vertices...
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 20.0f, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 20.0f, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 20.0f, 0.0f));
			
			//Top vertices...
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 0.0f, -25.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, 0.0f));
			vertices.Add(m_pathPositions[i] - new Vector3(0.0f, 0.0f, -25.0f));
			vertices.Add(m_pathPositions[i + 1] - new Vector3(0.0f, 0.0f, -25.0f));
			
			//UVs...
			uvs.Add(new Vector2(uvStride * i + uvStride, 1));
			uvs.Add(new Vector2(uvStride * i, 0));
			uvs.Add(new Vector2(uvStride * i, 1));
			uvs.Add(new Vector2(uvStride * i + uvStride, 1));
			uvs.Add(new Vector2(uvStride * i + uvStride, 0));
			uvs.Add(new Vector2(uvStride * i, 0));
			
			//UVs...
			uvs.Add(new Vector2(uvStride * i + uvStride, 0));
			uvs.Add(new Vector2(uvStride * i, 1));
			uvs.Add(new Vector2(uvStride * i, 0));
			uvs.Add(new Vector2(uvStride * i + uvStride, 0));
			uvs.Add(new Vector2(uvStride * i + uvStride, 1));
			uvs.Add(new Vector2(uvStride * i, 1));
		}
		
		for(int i = 0; i < vertices.Count; ++i)
		{
			normals.Add(new Vector3(0, 0, 1));
			triangles.Add(i);
		}
		
		GetComponent<MeshCreator>().BuildMesh(vertices, normals, uvs, triangles);
	}
}
