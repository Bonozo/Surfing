using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
 
public class GUIGrid : EditorWindow
{
	public enum GUIPosition {EDGE, CENTER, CORNER};
	
	private static GUIPosition m_guiPosition = GUIPosition.EDGE;
	private static bool m_onlyLockGUIPlanes = true;
    private static float m_snapAmount = 0.5f;
	
	private static GameObject m_guiPlaneParent = null;
	private static Dictionary<int, GUIPlane> m_guiPlanes = new Dictionary<int, GUIPlane>();
	
	private static float m_viewWidth = 0.0f;
	private static float m_viewHeight = 0.0f;
 
    [MenuItem( "Edit/GUIGrid" )]
    static void Init()
    {
		GUIGrid window = (GUIGrid)EditorWindow.GetWindow(typeof(GUIGrid));
		window.minSize = new Vector2(350, 100);
		window.title = "GUIGrid";
		m_guiPlaneParent = GameObject.Find("GUIPlanes") as GameObject;
		
		//Storing all GUI Planes in m_guiPlanes...
		GameObject[] guiPlanes = FindObjectsOfType(typeof(GameObject)) as GameObject[];
		GUIPlane currentGUIPlane = null;
		
		//Debug.Log(m_guiPlanes.Count);
		//Debug.Log(guiPlanes.Length);
		for(int i = 0; i < guiPlanes.Length; ++i)
		{
			currentGUIPlane = guiPlanes[i].GetComponent<GUIPlane>() as GUIPlane;
			if(currentGUIPlane && !m_guiPlanes.ContainsKey(guiPlanes[i].GetInstanceID()))
				m_guiPlanes.Add(guiPlanes[i].GetInstanceID(), currentGUIPlane);
		}
		
		//Debug.Log(m_guiPlanes.Count);
		
		//EditorApplication.update += OnUpdate;
		SceneView.onSceneGUIDelegate += OnScene;
    }
 
    public void OnGUI()
    {
		m_guiPosition = (GUIPosition)EditorGUILayout.EnumPopup("Position From:", m_guiPosition);
		
		//GUIPlane selectedGUIPlane = Selection.activeGameObject
		
		if(GUILayout.Button("Create GUI Plane"))
		{
			if(!m_guiPlaneParent)
			{
				m_guiPlaneParent = new GameObject("GUIPlanes");
			}
			
			Object obj = AssetDatabase.LoadAssetAtPath("Assets/GUIGrid/Exports/GUIPlane.prefab", typeof(GameObject));
			GameObject activeObj = PrefabUtility.InstantiatePrefab(obj) as GameObject;
			activeObj.transform.parent = m_guiPlaneParent.transform;
			Selection.activeTransform = activeObj.transform;
		}
		
		if(GUILayout.Button("Remove All GUI Planes"))
		{
			if(EditorUtility.DisplayDialog("Remove All GUI Planes?", "Are you sure you want to remove " +
		 		"all GUI Planes from the scene?", "Remove", "Cancel"))
			{
				GameObject[] guiPlanes = FindObjectsOfType(typeof(GameObject)) as GameObject[];
				for(int i = 0; i < guiPlanes.Length; ++i)
				{
					if(!guiPlanes[i])
						continue;
					
					if(guiPlanes[i].GetComponent<GUIPlane>())
					{
						if(guiPlanes[i].transform.parent)
							DestroyImmediate(guiPlanes[i].transform.parent.gameObject);
						else
							DestroyImmediate(guiPlanes[i]);
						
					}
				}
			}
		}
		
		m_snapAmount = EditorGUILayout.Slider("Snap Amount", m_snapAmount, 0.1f, 10.0f);
		
		bool lockGUIPlanes = m_onlyLockGUIPlanes;
		lockGUIPlanes = EditorGUILayout.Toggle("Lock GUI Planes Only:", lockGUIPlanes);
		
		if(lockGUIPlanes != m_onlyLockGUIPlanes)
		{
			if(!lockGUIPlanes)
			{
				if(EditorUtility.DisplayDialog("Lock Everything?", "Are you sure you want to lock everything to the grid?", "Yes", "Cancel"))
					m_onlyLockGUIPlanes = lockGUIPlanes;
			}
			else
				m_onlyLockGUIPlanes = lockGUIPlanes;
		}
    }
	
	private static void OnScene(SceneView sceneview)
    {
		GUIPlane.SetCameraFrustrumPoints
		(
			Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 20.0f)),
			Camera.main.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, 20.0f)),
			Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 20.0f)),
			Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 20.0f))
		);
		
		if(m_guiPosition == GUIPosition.EDGE)
			DrawGridFromEdge();
		else
			EditorUtility.SetDirty(Camera.main);
    }
 
    public void Update()
    {
		if(m_guiPosition == GUIPosition.EDGE)
			SnapToGridFromEdge();
		else
			SnapToGrid();
		
		//Setting planes, if not added...
		if(Selection.activeGameObject)
		{
			GUIPlane active = Selection.activeGameObject.GetComponent<GUIPlane>();
			if(active && !m_guiPlanes.ContainsKey(Selection.activeGameObject.GetInstanceID()))
				m_guiPlanes.Add(Selection.activeGameObject.GetInstanceID(), active);
		}
    }
 
    private void SnapToGrid()
    {	
		if(!EditorApplication.isPlaying && Selection.transforms.Length > 0)
		{
			foreach (Transform selection in Selection.transforms)
			{
				if(selection == Camera.main.transform)
					continue;
				
				selection.position = new Vector3(Round(selection.position.x),
					Round(selection.position.y), Round(selection.position.z));
			}
		}
    }
	
	private void SnapToGridFromEdge()
    {
		if(!EditorApplication.isPlaying && Selection.transforms.Length > 0)
		{
			foreach(Transform selection in Selection.transforms)
			{
				if(selection == Camera.main.transform)
					continue;
				if(m_onlyLockGUIPlanes)
				{
					if(m_guiPlanes.ContainsKey(selection.gameObject.GetInstanceID()))
					{
						if(!m_guiPlanes[selection.gameObject.GetInstanceID()].m_lockToGrid)
							continue;
					}
					else
						continue;
				}
				
				Vector3 viewPortPos = Camera.main.WorldToViewportPoint(selection.position);
				float halfView = 0.5f;
				
				if(viewPortPos.x < halfView)
				{
					if(viewPortPos.y < halfView)
						selection.position = GetPositionOnGrid(selection.position, GUIPlane.BottomLeftFrustrumPoint);
					else
						selection.position = GetPositionOnGrid(selection.position, GUIPlane.TopLeftFrustrumPoint);
				}
				else
				{
					if(viewPortPos.y < halfView)
						selection.position = GetPositionOnGrid(selection.position, GUIPlane.BottomRightFrustrumPoint);
					else
						selection.position = GetPositionOnGrid(selection.position, GUIPlane.TopRightFrustrumPoint);
				}
			}
		}
    }
	
	private Vector3 GetPositionOnGrid(Vector3 objPos, Vector3 gridPos)
	{
		Vector3 newPos = new Vector3(gridPos.x + Round(objPos.x - gridPos.x),
			gridPos.y + Round(objPos.y - gridPos.y), objPos.z);
		
		return newPos;
	}
	
	private static void DrawGridFromEdge()
	{	
		if(!Camera.main || m_snapAmount <= 0.0f)
			return;
		
		m_viewWidth = Vector3.Distance(Camera.main.ViewportToWorldPoint(Vector3.zero),
			Camera.main.ViewportToWorldPoint(Vector3.right));
		m_viewHeight = Vector3.Distance(Camera.main.ViewportToWorldPoint(Vector3.zero),
			Camera.main.ViewportToWorldPoint(Vector3.up));
		
		float width = 0.0f;
		float height = 0.0f;
		
		//Left to right...
		while(width <= m_viewWidth/2.0f)
		{
			Debug.DrawLine(GUIPlane.BottomLeftFrustrumPoint + Camera.main.transform.right * width,
				GUIPlane.TopLeftFrustrumPoint + Camera.main.transform.right * width, Color.green);
			width += m_snapAmount;
		}
		width = 0.0f;
		
		//Right to Left...
		while(width <= m_viewWidth/2.0f)
		{
			Debug.DrawLine(GUIPlane.BottomRightFrustrumPoint - Camera.main.transform.right * width,
				GUIPlane.TopRightFrustrumPoint - Camera.main.transform.right * width, Color.blue);
			width += m_snapAmount;
		}
		
		//Bottom to top...
		while(height <= m_viewHeight/2.0f)
		{
			Debug.DrawLine(GUIPlane.BottomLeftFrustrumPoint + Camera.main.transform.up * height,
				GUIPlane.BottomRightFrustrumPoint + Camera.main.transform.up * height, Color.red);
			height += m_snapAmount;
		}
		height = 0.0f;
		
		//Top to bottom...
		while(height <= m_viewHeight/2.0f)
		{
			Debug.DrawLine(GUIPlane.TopLeftFrustrumPoint - Camera.main.transform.up * height,
				GUIPlane.TopRightFrustrumPoint - Camera.main.transform.up * height, Color.yellow);
			height += m_snapAmount;
		}
		
		EditorUtility.SetDirty(Camera.main);
	}
 
	private float Round(float input)
	{
		if(m_snapAmount == 0)
			return input;
		
		return m_snapAmount * Mathf.Round(input/m_snapAmount);
	}
	
	void OnDestroy()
	{
		SceneView.onSceneGUIDelegate -= OnScene;
		EditorUtility.SetDirty(Camera.main);
		//EditorApplication.update -= OnUpdate;
	}
}
