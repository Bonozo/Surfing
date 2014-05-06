using UnityEngine;
using System.Collections;

public class GUISwitch : MonoBehaviour
{
	[HideInInspector]
	public bool m_disable = false;
	
	public virtual bool TakeAction(int actionIndex, params object[] arguments)
	{
		return true;
	}
}
