using UnityEngine;
using System.Collections;

public class PlaneCreator : MonoBehaviour
{
	public Texture2D m_texture = null;
	
	private Mesh m_mesh = null;
	
	// Use this for initialization
	void Start()
	{
		BuildPlane();
		renderer.material.SetTexture("_MainTex", m_texture);
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	
	private void BuildPlane()
	{
		m_mesh = new Mesh();

		m_mesh.vertices =  new Vector3[]
		{
			Camera.main.ScreenToWorldPoint(new Vector3(0.0f, 0.0f, 15.0f)),
			Camera.main.ScreenToWorldPoint(new Vector3(0.0f, Screen.height, 15.0f)),
			Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 15.0f)),
			Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 15.0f))
		};
		
		m_mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};
        m_mesh.normals = new Vector3[] {new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1)};
		m_mesh.triangles = new int[] {0, 1, 2, 2, 3, 0};
		
		GetComponent<MeshFilter>().mesh = m_mesh;
	}
}
