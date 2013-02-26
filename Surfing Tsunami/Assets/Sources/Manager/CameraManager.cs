using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraManager : MonoBehaviour {
	
	public Camera level;
	public int Width=240;
	public int Height=160;
	
	public Vector3 Center { get { return new Vector3(Width*0.5f,Height*0.5f); } }
	
}
