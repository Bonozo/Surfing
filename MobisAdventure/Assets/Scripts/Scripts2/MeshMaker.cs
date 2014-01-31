using UnityEngine;
using System.Collections;

public class MeshMaker
{
	//Makes and returns a Mesh...
	public static Mesh Make(Vector3[] vertices, Vector3[] normals, Vector2[] uvs, int[] triangles)
	{
		Mesh mesh = new Mesh();
		
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.uv = uvs;
		mesh.triangles = triangles;
		
		return mesh;
	}
}
