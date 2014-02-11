using UnityEngine;
using System.Collections;

public class LevelCorrectDrag : ZIPLevel {

	public GameBlock gameBlock;
	public DragZIP dragZIP;
	public GameObject rightAnswer;
	public float happyEndScale = 2f;

	public GameObject guiAction;
	public GameObject guiEnd;
	public GameObject goodEndZip;
	public GameObject goodEndSprite;

	public AudioClip clipCorrectAnswer;
	public AudioClip clipWrongAnswer;

	private float happyEndTime = 3f;

	private Vector3 zipPos;
	private Vector3 zipSprite;

	void Awake()
	{
		guiEnd.transform.localScale = new Vector3(happyEndScale,happyEndScale,happyEndScale);
		zipPos = goodEndZip.transform.position;
		zipSprite = goodEndSprite.transform.position;

		// Setting up drag area parameters
		var panel = GetComponentInChildren<UIPanel> ();
		float aspectRation = (float)Screen.width / (float)Screen.height;
		Vector4 clipRange = panel.clipRange;
		float currentRation = clipRange.z / clipRange.w;
		clipRange.z *= aspectRation / currentRation;

		// adding margins to the borders
		/*clipRange.z *= 0.95f;
		clipRange.w *= 0.95f;*/

		panel.clipRange = clipRange;
	}

	void OnEnable()
	{
		Reset();
	}

	public void Answered (GameObject other)
	{
		if(other == rightAnswer)
		{
			AudioSource.PlayClipAtPoint(clipCorrectAnswer,transform.position);
			StartCoroutine(HappyEnd());
		}
		else
		{
			dragZIP.Reset();
			AudioSource.PlayClipAtPoint(clipWrongAnswer,transform.position);
		}
	}

	private IEnumerator HappyEnd()
	{
		yield return new WaitForEndOfFrame();
		dragZIP.DisableCollider();

		goodEndZip.transform.localPosition = dragZIP.transform.localPosition;
		goodEndSprite.transform.localPosition = rightAnswer.transform.localPosition;

		goodEndZip.GetComponent<UISpriteAnimation>().enabled = false;
		goodEndZip.GetComponent<UISprite>().spriteName = dragZIP.GetComponent<UISprite>().spriteName;

		guiAction.SetActive(false);
		guiEnd.SetActive(true);

		iTween.MoveTo(goodEndZip,zipPos,happyEndTime);
		iTween.MoveTo(goodEndSprite,zipSprite,happyEndTime);

		guiEnd.transform.localScale = new Vector3(1f,1f,1f);
		iTween.ScaleTo(guiEnd,new Vector3(happyEndScale,happyEndScale,happyEndScale),1f);

		
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
