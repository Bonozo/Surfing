using UnityEngine;
using System.Collections;

public class CameraFollowPoint : MonoBehaviour
{
	public Camera followCam;
	public Transform m_target = null;
	public float m_followSpeed = 10.0f;
	public float y_offset = 0.0f;
	public float z_offsetRatio;
	
	private Vector3 m_targetPos = Vector3.zero;
	
	void FixedUpdate()
	{
		if(PlayerController.Instance.GamePaused)
			return;
		float path_y;
		Path.Current.GetHeight(m_target.position, out path_y);			
		
		m_targetPos.Set(m_target.position.x + 6.0f, m_target.position.y + y_offset + (z_offsetRatio/2), -15);
		m_targetPos = Vector3.Lerp(followCam.transform.position,
		                           m_targetPos, m_followSpeed * Time.fixedDeltaTime);
				followCam.transform.position = m_targetPos;
	}

	public float maxAllowedAcc = 0.0015f;
	private Vector3 lastPosition = Vector3.zero;
	private float lastDist=0f;
	bool SmoothPlayer()
	{
		// check player's last move
		float dist = Vector3.Distance (lastPosition, m_target.position);
		lastPosition = m_target.position;
		if (dist > lastDist + maxAllowedAcc)
		{
			Debug.Log ("Jerk");
			m_target.position = Vector3.MoveTowards(lastPosition,m_target.position,lastDist);
			lastDist += maxAllowedAcc;
			return true;
		}
		else
			lastDist = dist;
		return false;
	}
}


