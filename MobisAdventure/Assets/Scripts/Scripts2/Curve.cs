using UnityEngine;
using System.Collections;

public class Curve
{
	public enum Type { LINEAR = 2, QUADRATIC = 3, CUBIC = 4};
	
	public static void SetPosOnCurve(out Vector3 pos, float time, params Vector3[] positions)
	{
		switch(positions.Length)
		{
			case (int)Type.QUADRATIC:
				SetPosOnQuadraticCurve(out pos, time, positions);
				break;
			case (int)Type.CUBIC:
				SetPosOnCubicCurve(out pos, time, positions);
				break;
			default:
				pos = Vector3.zero;
				break;
		}
	}
	
	public static void SetPosOnQuadraticCurve(out Vector3 pos, float time, params Vector3[] positions)
	{
		//P(t) = (1 - t)^2 P0  +  2t(1 - t) P1  +  t^2 P2
		pos = Mathf.Pow(1 - time, 2) * positions[0];
		pos += 2 * time * (1 - time) * positions[1];
		pos += Mathf.Pow(time, 2) * positions[2];
	}
	
	public static void SetPosOnCubicCurve(out Vector3 pos, float time, params Vector3[] positions)
	{
		//P(t) = (1 - t)^3 P0  +  3t(1 - t)^2 P1  +  3t^2 (1 - t) P2 + t^3 P3
		pos = Mathf.Pow(1 - time, 3) * positions[0];
		pos += 3 * time * Mathf.Pow(1 - time, 2) * positions[1];
		pos += 3 * Mathf.Pow(time, 2) * (1 - time) * positions[2];
		pos += Mathf.Pow(time, 3) * positions[3];
	}
}
