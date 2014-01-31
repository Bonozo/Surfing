using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
	private LineRenderer m_lineRenderer = null;
	private bool m_initialize = true;
	
	private int m_numPoints = 0;
	
	void Update()
	{
		//Ran once in update to avoid conflicts with execution order of other scripts...
		InitializeOnce();
	}
	
	public void UpdatePositions(int curveIndex)
	{
		Vector3[] positions = Path.Current.GetCurve(curveIndex, m_numPoints);
		for(int i = 0; i < positions.Length; ++i)
			m_lineRenderer.SetPosition(i, positions[i]);
	}
	
	void InitializeOnce()
	{
		if(m_initialize)
		{
			m_lineRenderer = GetComponent<LineRenderer>();
			
			m_numPoints = MeshManager.Current.m_numCurveMeshes * MeshManager.Current.m_numCurvesTaken;
			Vector3[] positions = Path.Current.GetCurve(0, m_numPoints);
			m_lineRenderer.SetVertexCount(positions.Length);
			for(int i = 0; i < positions.Length; ++i)
				m_lineRenderer.SetPosition(i, positions[i]);
			
			MeshManager.Current.SetCurvePositions(UpdatePositions);
			m_initialize = false;
		}
	}
}
