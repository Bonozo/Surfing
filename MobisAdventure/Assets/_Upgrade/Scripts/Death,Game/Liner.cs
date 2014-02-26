using UnityEngine;
using System.Collections;

public class Liner : MonoBehaviour {

	private LineRenderer renderer;
	private PlayerController player;

	int lenght = 350;
	int current=0;
	float next_pos = 0;

	void Awake()
	{
		renderer = GetComponent<LineRenderer> ();
		player = GameObject.FindObjectOfType<PlayerController> ();
	}

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame ();
		renderer.SetVertexCount (lenght);
		UpdateLine(0);
	}

	void Update()
	{
		if(inited && player.transform.position.x > next_pos)
			UpdateLine(current+(int)(lenght*0.65));
	}

	bool inited=false;
	void UpdateLine(int index)
	{
		inited = true;
		current = index;
		for(int i=0;i<lenght;i++)
			renderer.SetPosition(i,Path.Current.m_pathPositions[index+i]);
		next_pos = Path.Current.m_pathPositions [index + (int)(lenght*0.85f)].x;
	}
}
