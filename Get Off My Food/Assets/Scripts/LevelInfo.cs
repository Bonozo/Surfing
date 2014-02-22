using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour {

	public UILabel labelWave;
	public UILabel labelAnts;
	public UILabel labelShowWave;

	int currentWave=0;
	int currentAnts=0;
	int antCount = 10;

	public int CurrentWave{ get { return currentWave; } }

	public void AddAnt()
	{
		currentAnts++;
		labelAnts.text = "Ant: " + currentAnts;
	}

	public GameObject antPrefab;
	void NewAnt()
	{
		Instantiate (antPrefab);
		//ant.transform.parent = gameObject.transform;
		//ant.transform.localScale = new Vector3 (1f, 1f, 1f);
	}

	IEnumerator ShowWave()
	{
		labelWave.text = "Wave: " + currentWave;
		labelShowWave.text = "WAVE " + currentWave;
		labelShowWave.gameObject.SetActive (true);
		yield return new WaitForSeconds (4f);
		labelShowWave.gameObject.SetActive (false);

	}

	IEnumerator Start()
	{
		for(currentWave=1;;++currentWave)
		{
			yield return new WaitForSeconds(1f);
			yield return StartCoroutine(ShowWave());
			labelAnts.text = "Ant: 0";
			currentAnts = 0;
			for(int i=0;i<antCount;++i)
			{
				NewAnt();
				float minTime = Mathf.Max (0.1f,1f-0.1f*currentWave);
				float maxTime = Mathf.Max (1f,3f-0.4f*currentWave);
				yield return new WaitForSeconds(Random.Range(minTime,maxTime));
			}
			while(currentAnts != antCount ) yield return null;
		}
	}

	void Update()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
			Application.Quit();
	}


	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile LevelInfo _staticInstance;	
	public static LevelInfo Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(LevelInfo)) as LevelInfo;
					if (_staticInstance == null) {
						Debug.LogError("The LevelInfo instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}
}
