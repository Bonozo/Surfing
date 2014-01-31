using UnityEngine;
using System.Collections;

public class Window : MonoBehaviour
{
	public Rect windowRect = new Rect(20, 20, 120, 50);
    
	void OnGUI()
	{
    	windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
    }
    
	void DoMyWindow(int windowID)
	{
    	if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
    		print("Got a click");
    }
}
