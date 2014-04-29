using UnityEngine;
using System.Collections;

[AddComponentMenu("EndItems/Base")]
public abstract class EndItem : MonoBehaviour {

	public abstract void Reset();
	public abstract void Work();
}
