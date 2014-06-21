using UnityEngine;
using System.Collections;

public class ButtonRestoreIAP : MonoBehaviour {

	void OnClick(){
		MobiIAB.Instance.RestoreTransactions ();
	}
}
