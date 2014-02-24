using UnityEngine;
using System.Collections;

public class FingersHandler : MonoBehaviour {

	void OnDrag( DragGesture gesture ) 
	{
		if(gesture.Selection != null && gesture.Selection.tag == "Ant")
			gesture.Selection.GetComponent<Ant>().AddForce(gesture.DeltaMove);
	}
	
	void OnFingerDown(FingerDownEvent e)
	{ 
		if(e.Selection != null && e.Selection.tag == "Ant")
			e.Selection.GetComponent<Ant>().Squash();
	}
}
