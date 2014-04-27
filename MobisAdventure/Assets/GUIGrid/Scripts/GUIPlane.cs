using UnityEngine;
using System.Collections;

public class GUIPlane : MonoBehaviour
{
	public bool m_lockToGrid = true;
	
	//private Vector2 m_uvOffset = Vector2.zero;
	
	//Camera info...
	private static Vector3 m_bottomLeft = Vector3.zero;
	private static Vector3 m_topLeft = Vector3.zero;
	private static Vector3 m_bottomRight = Vector3.zero;
	private static Vector3 m_topRight = Vector3.zero;
	
	// Use this for initialization
	void Awake()
	{
		SetCameraFrustrumPoints
		(
			Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 20.0f)),
			Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 20.0f)),
			Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 20.0f)),
			Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 20.0f))
		);

		initialCameraPos = transform.position.x;
	}
	
	float initialCameraPos=0;
	void Update()
	{
		if(renderer)
		{
			float offset = ((transform.position.x-initialCameraPos)*0.0012f)%1f;
			if(offset>0.5f) offset-=1f;
			renderer.material.SetTextureOffset("_MainTex", new Vector2(offset,0f));
		}
	}
	
	public static void SetCameraFrustrumPoints(Vector3 bottomeLeft, Vector3 topLeft, Vector3 bottomRight, Vector3 topRight)
	{
		m_bottomLeft = bottomeLeft;
		m_topLeft = topLeft;
		m_bottomRight = bottomRight;
		m_topRight = topRight;
	}
	
	public static Vector3 BottomLeftFrustrumPoint
	{
		get{ return m_bottomLeft;}
	}
	
	public static Vector3 BottomRightFrustrumPoint
	{
		get{ return m_bottomRight;}
	}
	
	public static Vector3 TopLeftFrustrumPoint
	{
		get{ return m_topLeft;}
	}
	
	public static Vector3 TopRightFrustrumPoint
	{
		get{ return m_topRight;}
	}
}
