using UnityEngine;
using System.Collections;

public class GUI_Sleds : GUISwipe
{
	//m_swipeIndex
	// Use this for initialization
	public override void Start ()
	{
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update();
//		Debug.Log(m_swipeIndex);
//		Debug.Log((GameManager.ChosenSled)(m_swipeIndex % m_guiPlanes.Length));
		GameManager.m_chosenSled = (GameManager.ChosenSled)(m_swipeIndex % m_guiPlanes.Length);

	}
}
