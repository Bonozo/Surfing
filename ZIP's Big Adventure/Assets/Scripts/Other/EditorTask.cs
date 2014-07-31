using UnityEngine;
using System.Collections;

public class EditorTask : MonoBehaviour {

	public void Work()
	{
		foreach(Transform t in transform)
			t.GetComponent<LevelRightAnswer>().Initialize();
	}
}
