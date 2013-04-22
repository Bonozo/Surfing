using UnityEngine;
using System.Collections;

public class AccelerationWithCalibrate : MonoBehaviour {
	
	private static float _currentCalibration = float.PositiveInfinity;
	private static float currentCalibration{
		get{
			if(float.PositiveInfinity == _currentCalibration)
				_currentCalibration = PlayerPrefs.GetFloat("acceleration_calibration",0f);
			return _currentCalibration;
		}
		set{
			_currentCalibration = value;
		}
	}
	public static int CurrentCalibrationPositive{
		get{
			var c = currentCalibration;
			if(c<0) c+=360f;
			return Mathf.RoundToInt(c);
		}
	}
	
	public static int DeviceAnglePositive{
		get{
			var c = DeviceAngle;
			if(c<0) c+=360f;
			return Mathf.RoundToInt(c);
		}
	}
	
	private static float calibratedelta()
	{
		float current = DeviceAngle;
		current -= currentCalibration;
		if(current > 180f) current-=360f;
		if(current < -180f) current+=360f;
		return current;
	}
	
	public static void Calibrate()
	{
		currentCalibration = DeviceAngle;
		PlayerPrefs.SetFloat("acceleration_calibration",currentCalibration);
	}
	
	public static Vector3 Acceleration{
		get{
			Vector3 dir = InputAxis;
			dir.y = -calibratedelta()/90f;
			return dir;
		}
	}
	
	#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
	private static float DeviceAngle{ 
		get{
			var acc = Input.acceleration;
			if(acc.sqrMagnitude>1) acc.Normalize();
			if(acc.y<=0f&&acc.z<=0f) return Mathf.Lerp(0f,90f,-acc.y);
			if(acc.y<=0f&&acc.z>=0f) return Mathf.Lerp(90f,180f,acc.z);
			if(acc.y>=0f&&acc.z>=0f) return Mathf.Lerp(180f,270f,acc.y);
			if(acc.y>=0f&&acc.z<=0f) return Mathf.Lerp(270,360f,-acc.z);
			return 0.0f;
			}
	}
	
	private static Vector3 InputAxis { get {
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
	private static float DeviceAngle{ 
		get{
			float yaxis = 90f*Input.GetAxis("Vertical");
			if(yaxis<0f) yaxis=360f+yaxis;
			return -yaxis;
			}
	}
	
	private static Vector3 InputAxis { get {
		return new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0f);
	}}
	#endif
}
