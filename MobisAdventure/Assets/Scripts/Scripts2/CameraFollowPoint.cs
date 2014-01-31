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

	private PlayerController playerController;

	void Awake()
	{
		playerController = GameObject.FindObjectOfType(typeof(PlayerController)) as PlayerController;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if(playerController.GamePaused)
		{
			Debug.Log("paused");
			return;
		}

		float path_y;
		Path.Current.GetHeight(m_target.position, out path_y);
//		z_offsetRatio = Mathf.Abs(m_target.position.y - path_y);
		//float zerp = Mathf.Abs(m_target.position.y - path_y);
		/*if(zerp > 2.0){
			z_offsetRatio = Mathf.MoveTowards(z_offsetRatio, zerp, 2 * Time.deltaTime);
			followCam.orthographicSize = 10 + z_offsetRatio;
		} else {
			z_offsetRatio = Mathf.MoveTowards(z_offsetRatio, 0, 2 * Time.deltaTime);
			followCam.orthographicSize = 10 + z_offsetRatio;
		}*/
			
		
		m_targetPos.Set(m_target.position.x + 6.0f, m_target.position.y + y_offset + (z_offsetRatio/2), -15);
		transform.position = m_targetPos;
		
		followCam.transform.position = Vector3.Lerp(followCam.transform.position,
			transform.position, m_followSpeed * Time.fixedDeltaTime);
	}
}


