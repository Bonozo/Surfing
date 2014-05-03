using UnityEngine;
using System.Collections;

public class LevelCorrectDrag : ZIPLevel {
	
	public DragZIP dragZIP;
	public GameObject rightAnswer;

	public GameObject guiAction;
	public GameObject guiEnd;
	public GameObject goodEndZip;
	public GameObject goodEndSprite;

	private float happyEndTime = 1.4f;
	private iTween.EaseType easyType = 0;

	private Vector3 zipPos;
	private Vector3 zipSprite;

	void Awake()
	{
		zipPos = goodEndZip.transform.position;
		zipSprite = goodEndSprite.transform.position;

		// Setting up drag area parameters
		var panel = GetComponentInChildren<UIPanel> ();
		float aspectRation = (float)Screen.width / (float)Screen.height;
		Vector4 clipRange = panel.clipRange;
		float currentRation = clipRange.z / clipRange.w;
		clipRange.z *= aspectRation / currentRation;

		panel.clipRange = clipRange;
	}

	void OnEnable()
	{
		Reset();
		SendMessage ("PlayStart",SendMessageOptions.DontRequireReceiver);
	}

	public void Answered (GameObject other)
	{
		//SendMessage ("PlayAnswer", other == rightAnswer);
		if(other == rightAnswer)
		{
			StartCoroutine(HappyEnd());
		}
		else
		{
			dragZIP.Reset();
			GameController.Instance.PlayWrongAnswer();
		}
	}

	private IEnumerator HappyEnd()
	{
		GameController.Instance.PlayCorrectAnswer();
		SendMessage ("PlayFinish",SendMessageOptions.DontRequireReceiver);

		yield return new WaitForEndOfFrame();
		dragZIP.DisableCollider();

		goodEndZip.transform.localPosition = dragZIP.transform.localPosition;
		goodEndSprite.transform.localPosition = rightAnswer.transform.localPosition;

		goodEndZip.GetComponent<UISpriteAnimation>().enabled = false;
		goodEndZip.GetComponent<UISprite>().spriteName = dragZIP.GetComponent<UISprite>().spriteName;

		guiAction.SetActive(false);
		guiEnd.SetActive(true);

		iTween.MoveTo(goodEndZip,iTween.Hash("position",zipPos,"time",happyEndTime,"easetype",easyType,"islocal",false));
		iTween.MoveTo(goodEndSprite,iTween.Hash("position",zipSprite,"time",happyEndTime,"easetype",easyType,"islocal",false));

		guiEnd.transform.localScale = new Vector3(1f,1f,1f);

		yield return new WaitForSeconds(0.5f);
		gameBlock.path.OneStepGo();

		yield return new WaitForSeconds(6f);
		Reset();
		gameBlock.LevelComplete();
	}

	public void Reset()
	{
		guiAction.SetActive(true);
		guiEnd.SetActive(false);
		dragZIP.Reset();
	}

	public override void StartGame()
	{
		gameObject.SetActive(true);
	}
}
