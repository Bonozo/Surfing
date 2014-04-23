using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	//public TextMesh m_timer = null;
	public enum ChosenMobi {CLASSIC = 0, PREP = 1, SKATE = 2, RACING = 3, ARMY = 4, BEACH = 5, BIKER = 6};
	public enum ChosenSled {Classic = 0, Turbo = 1, Track = 2, Sport = 3, Hybrid = 4};
	public enum ChosenLevel {Level_Arctic = 0, Level_IcyTundra = 1, Level_Forest = 2, Level_Mars = 3, Level_Beach = 4};
	
	public static ChosenSled m_chosenSled = ChosenSled.Classic;
	public static ChosenMobi m_chosenMobi = ChosenMobi.CLASSIC;
	public static ChosenLevel m_chosenLevel = ChosenLevel.Level_Arctic;
	
//	private int m_seconds = 0;
//	private int m_minutes = 0;
//	private int m_hours = 0;
//	private int m_days = 0;
	
	private static GameManager m_gameManager = null;
	
	void Awake()
	{
		m_gameManager = this;
	}
	
	// Use this for initialization
	void Start()
	{
		//m_timer.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(20.0f, Screen.height - 20.0f, 1.0f));
		//m_timer.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update()
	{
		UpdateTime();
	}
	
	void UpdateTime()
	{
//    	m_seconds = (int)(Time.timeSinceLevelLoad % 60);
//    	m_minutes = (m_seconds / 60) % 60;
//    	m_hours = (m_seconds / 3600) % 24;
//    	m_days = m_seconds / 86400;
	}
	
	public int GetSeconds(float startTime)
	{
		float time = Time.timeSinceLevelLoad - startTime;
		int seconds = (int)(time % 60);
		return seconds;
	}
	
	public static GameManager Instance
    {
        get { return m_gameManager; }
    }
}
