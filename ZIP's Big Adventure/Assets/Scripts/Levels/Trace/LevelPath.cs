using UnityEngine;
using System.Collections;

public class LevelPath : MonoBehaviour {

	public int totalSteps;
	public string pathName;
	public float onePathReachTime=4f;
	public Transform stars;
	public GameObject zip;
	public UIAnimation zipAnimation;
	public UIAnimation mooseAnimation;

	private Vector3 beginPosition;
	private int step=1;

	void Awake()
	{
		beginPosition = zip.transform.localPosition;
	}

	void Update()
	{
		#if UNITY_EDITOR
		/*if( Input.GetKeyDown(KeyCode.Space))
			OneStepGo();
		if(Input.GetKeyDown(KeyCode.P)) 
			zipAnimation.Pause();
		if(Input.GetKeyDown(KeyCode.R)) 
			zipAnimation.Resume();
		if(Input.GetKeyDown(KeyCode.S)) 
			zipAnimation.Pause();
		if(Input.GetKeyDown(KeyCode.A)) 
			zipAnimation.Play();*/
		#endif
	}

	public void Reset()
	{
		StopAllCoroutines ();

		step=1;
		ismoving = false;
		zip.transform.localPosition = beginPosition;
	}

	public void PlayFirstAnims()
	{
		iTween.MoveTo(zip,iTween.Hash("path",iTweenPath.GetPath(pathName+0),"time",onePathReachTime));
		zipAnimation.Play();
		mooseAnimation.Play();
	}

	public void PauseAnims()
	{
		zipAnimation.Pause();
		mooseAnimation.Pause();
	}

	public bool Finished{ get{ return step>totalSteps; }}

	public void OneStepGo()
	{
		StartCoroutine (GoNextStar ());
	}

	private bool ismoving = false;
	public IEnumerator GoNextStar()
	{
		yield return StartCoroutine (GoNextStar (1.5f));
	}

	public IEnumerator GoNextStar(float moveDelay)
	{
		if (!ismoving && !Finished)
		{
			ismoving = true;

			// Start animations
			zipAnimation.Play();
			mooseAnimation.Play();
			for(int i=0;i<8;i++)
				StartCoroutine("RotateStart",stars.FindChild("star" + i));

			yield return new WaitForSeconds(moveDelay);

			iTween.MoveTo(zip,iTween.Hash("path",iTweenPath.GetPath(pathName+step),"time",onePathReachTime));
			step++;

			// 1 second delay (iTween.MoveTo issue)
			yield return new WaitForSeconds(onePathReachTime);

			// Stoping animations
			zipAnimation.Pause();
			mooseAnimation.Pause();
			StopCoroutine("RotateStart");

			ismoving = false;
		}
	}
	
	IEnumerator RotateStart(Transform t){
		float speed = Random.Range(300f,600f);
		while(true){
			t.Rotate(0f,0f,-speed*Time.deltaTime);
			yield return null;
		}
	}

	#region Editor

	public Transform pathRoot;
	public void Initialize()
	{
		int n = 0;
		foreach(Transform child in pathRoot){
			if( child.GetComponent<iTweenPath>() != null){
				child.GetComponent<iTweenPath>().pathName = pathName+n;
				n++;
			}
		}
	}

	#endregion
}
