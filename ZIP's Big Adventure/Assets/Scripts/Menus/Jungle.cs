using UnityEngine;
using System.Collections;

public class Jungle : MonoBehaviour {

	public float onePathReachTime=3f;
	public GameObject zip;
	public GameObject toggleFrom,toggleTo;
	
	private bool openprocessing=false;
	private int step=1;
	
	void OnEnable()
	{
		openprocessing=false;
		step=1;
		zip.transform.localPosition = new Vector3(-400f,-315f,0f);
	}
	
	void OnClick()
	{
		if(openprocessing) return;
		if(step<=10){
			StartCoroutine(Move());
			step++;
		}
		else{
			GameController.Instance.Toggle(toggleFrom,toggleTo);
		}
	}
	
	private IEnumerator Move()
	{
		openprocessing=true;
		iTween.MoveTo(zip,iTween.Hash("path",iTweenPath.GetPath("path"+step),"time",onePathReachTime));
		yield return new WaitForSeconds(onePathReachTime);
		openprocessing=false;
	}
}
