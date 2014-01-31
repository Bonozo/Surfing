using UnityEngine;
using System.Collections;

public class LoadLevel : GUITrigger
{
	public int m_nextLevel = 0;
	
	public override bool TakeAction(Gesture gesture)
	{
		if(gesture.pickObject != gameObject)
			return false;
		if(m_nextLevel !=0){
			Application.LoadLevel(m_nextLevel);
		} else
			Application.LoadLevel(GameManager.m_chosenLevel.ToString());
		
		return true;
	}
}

