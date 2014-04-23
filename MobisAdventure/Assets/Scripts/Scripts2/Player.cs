using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{	
	private static Player m_player = null;
	
	private Quaternion m_startingRotation = Quaternion.identity;
	
	void Awake()
	{
		m_player = this;
		m_startingRotation = transform.rotation;
	}

	public static Player Instance
    {
        get { return m_player; }
    }
	
	public Vector2 Velocity
    {
        get { return new Vector2(rigidbody.velocity.x, rigidbody.velocity.y); }
    }
	
	public Quaternion StartingRotation
    {
        get { return m_startingRotation; }
    }
}
