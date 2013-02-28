using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	#region Screen Environments
	
	public bool HitTest(Rect rect,Vector3 point)
	{
		return rect.xMin <= point.x && point.x <= rect.xMax &&
			   rect.yMin <= point.y && point.y <= rect.yMax;
	}
	
	#endregion
	
	#region Depth Information
	
	public float depthSurfer=97;
	public float depthObstacle=99;
	public float depthCrest=99.5f;
	public float depthWindArrow=75;
	
	#endregion
	
	#region Game Hierarchy Panel Transform
	
	// Menu Items
	public Transform transformTitle;
	public Transform transformPauseMenu;
	public Transform transformHUB;
	public Transform transformScoreboard;
	public Transform transformOptions;
	public Transform transformCredits;
	public Transform transformStore;
	
	public Transform transformSurfer;
	public Transform transformObstacles;
	public Transform transformCrests;
	
	#endregion
	
	#region Classes
	
	public Generator generator;
	public WindArrow windArrow;
	public Surfer surfer;
	public HUB hub;
	
	#endregion
	
	#region Prefabs
	
	public GameObject prefabWaterSpray;
	
	#endregion
}
