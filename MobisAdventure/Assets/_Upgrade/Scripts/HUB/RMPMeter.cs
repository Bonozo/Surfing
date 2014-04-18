using UnityEngine;
using System.Collections;

public class RMPMeter : MonoBehaviour {

	private PlayMakerFSM fsm;
	private float smoothPeriod = 0.05f;
	private float[] length = new float[] {0.99f,2.534f,3f,5f};
	private int current = 0;
	private float tm=0f;

	void Awake()
	{
		fsm = PlayerController.Instance.transform.FindChild ("Engine_Controller").GetComponent<PlayMakerFSM> ();
	}

	void FixedUpdate()
	{
		if (fsm.ActiveStateName.Length == 5 && fsm.ActiveStateName [0] == 'G' && fsm.ActiveStateName [1] == 'e') {
			int ind = (int)(fsm.ActiveStateName[4]-'1');
			if(current!=ind)
			{
				current = ind;
				tm = 0f;
			}

			tm += Time.deltaTime;
			tm = Mathf.Min(tm,length[current]);
		}
		else
		{
			tm = 0f;
		}
		float val = 160f * tm / length [current];
		var newrot = Quaternion.Euler(0f, 0f, 90f - val);
		transform.rotation = Quaternion.Slerp(transform.rotation,newrot,smoothPeriod);
	}
}
