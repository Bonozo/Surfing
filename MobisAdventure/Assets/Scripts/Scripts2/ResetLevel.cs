using UnityEngine;
using System.Collections;

public class ResetLevel : MonoBehaviour
{
	
	
//	// Subscribe to events
//	void OnEnable(){
//		EasyTouch.On_SimpleTap += On_SimpleTap;
//	}
//
//	void OnDisable(){
//		UnsubscribeEvent();
//	}
//	
//	void OnDestroy(){
//		UnsubscribeEvent();
//	}
//	
//	void UnsubscribeEvent(){
//		EasyTouch.On_SimpleTap -= On_SimpleTap;	
//	}
//	
//	// Simple tap
//	private void On_SimpleTap( Gesture gesture){
//		Debug.Log(gesture.pickObject.name);
//		// Verification that the action on the object
//		if (gesture.pickObject == gameObject){
//			
//			Application.LoadLevel(GameManager.m_chosenLevel.ToString());
//		}
//	}
}