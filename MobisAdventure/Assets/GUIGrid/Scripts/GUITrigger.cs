using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MeshCollider))]
public class GUITrigger : MonoBehaviour
{
	public enum TouchAction { TAP, PRESS, RELEASE };
	
	public TouchAction m_touchAction = TouchAction.TAP;
	public int m_actionIndex = 0;
	public object[] m_arguments = null;
	public GUISwitch m_guiSwitch = null;
	
	// Subscribe to events
	void OnEnable()
	{
		collider.isTrigger = true;
		EasyTouch.On_TouchStart += On_TouchStart;
		EasyTouch.On_TouchDown += On_TouchDown;
		EasyTouch.On_TouchUp += On_TouchUp;
	}
	
	// Unsubscribe
	void OnDisable()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}
	
	// Unsubscribe
	void OnDestroy()
	{
		EasyTouch.On_TouchStart -= On_TouchStart;
		EasyTouch.On_TouchDown -= On_TouchDown;
		EasyTouch.On_TouchUp -= On_TouchUp;
	}
	
	// Touch start event
	public void On_TouchStart(Gesture gesture)
	{
		if(m_touchAction == TouchAction.TAP)
			TakeAction(gesture);
	}
	
	// Touch start event
	public void On_TouchUp(Gesture gesture)
	{
		if(m_touchAction == TouchAction.RELEASE)
			TakeAction(gesture);
	}
	
	// Touch start event
	public void On_TouchDown(Gesture gesture)
	{
		if(m_touchAction == TouchAction.PRESS)
			TakeAction(gesture);
	}
	
	public virtual bool TakeAction(Gesture gesture)
	{
		if(gesture.pickObject == gameObject)
		{
			if(m_guiSwitch)
				return m_guiSwitch.TakeAction(m_actionIndex, m_arguments);
			return true;
		}
		
		return false;
	}
}
