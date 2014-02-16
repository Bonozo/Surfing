using UnityEngine;
using System.Collections;

public class GUIPlane : MonoBehaviour
{
	public bool m_lockToGrid = true;
	public Vector2 m_uvAnimationRate = Vector2.zero;
	
	private Vector2 m_uvOffset = Vector2.zero;
	
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
	}
	
	// Update is called once per frame
	void Update()
	{
		if(renderer)
		{
			/*Vector2 current = m_uvOffset;
			Vector2 upd = current + (m_uvAnimationRate * Time.deltaTime);
			current.x = Mathf.SmoothStep(current.x,upd.x,1f);
			current.y = Mathf.SmoothStep(current.y,upd.y,1f);
			m_uvOffset = current;
			Debug.Log(current-upd);
			renderer.material.SetTextureOffset("_MainTex", m_uvOffset);*/

			m_uvOffset += (m_uvAnimationRate * Time.deltaTime);
			renderer.material.SetTextureOffset("_MainTex", m_uvOffset);
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
