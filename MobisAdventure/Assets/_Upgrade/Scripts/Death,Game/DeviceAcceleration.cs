using UnityEngine;
using System.Collections;

public class DeviceAcceleration : MonoBehaviour {

	#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
	public static Vector3 Acceleration { get {
			Vector3 dir = Vector3.zero;
			dir.x = -Input.acceleration.y;
			dir.z = Input.acceleration.x;
			
			if(dir.sqrMagnitude > 1)
				dir.Normalize();
			dir.y = -dir.x;
			dir.x = dir.z;
			dir.z = 0;
			
			return dir;
		}}
	
	#else
	
	public static Vector3 Acceleration { get {
			return new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0f);
		}}
	#endif
}
