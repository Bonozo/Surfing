using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public MenuConfirmationPopup confirmationPopup;
	public UILabel[] labelCoinCount;
	public MenuToggle GetMoreCoinsToggle;
	
	void Awake()
	{
		#if UNITY_IPHONE
		Application.targetFrameRate = 60;
		#endif
		
		coinshow = PlayerPrefs.GetInt ("pp_coins");
	}

	void Update()
	{
		UpdateCoins ();

		if(Input.GetKey(KeyCode.Escape))
			Application.Quit();
	}

	float cintrvstep = 0.005f;
	float cintrv = 0f;
	int coinshow;
	void UpdateCoins()
	{
		int realcoins = PlayerPrefs.GetInt ("pp_coins");

		int dif = Mathf.Abs (realcoins - coinshow);
		cintrv -= Time.deltaTime;
		if (cintrv<=0 && dif != 0) 
		{
			cintrv=cintrvstep;
			int def=dif;
			/**/ if(dif<100) dif=8;
			else if(dif<1000) dif=87;
			else if(dif<10000) dif=876;
			else if(dif<100000) dif=8765;
			else if(dif<1000000) dif=87654;
			else if(dif<10000000) dif=876543;
			else /*             */ dif=8765432;

			dif = Mathf.Min(dif,def);
			if(coinshow<realcoins) coinshow += dif;
			else coinshow -= dif;
		}
		
		string coinstext = PutCommas (coinshow);
		foreach (UILabel label in labelCoinCount)
			label.text = coinstext;

	}
	
	#region Helpful
	
	public static string PutCommas(int _number)	
	{
		int[] part = new int[11];
		int index=0;
		do{
			part[index++]=_number%10;
			_number/=10;
		} while(_number!=0);
		
		string result="";
		
		do{
			index--;
			result += part[index].ToString();
			if(index!=0&&index%3==0) result += ',';
		} while(index!=0);
		return result;
	}
	
	#endregion

	#region Static Instance

	// Multithreaded Safe Singleton Pattern
	// URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
	private static readonly object _syncRoot = new Object();
	private static volatile MainMenu _staticInstance;	
	public static MainMenu Instance 
	{
		get {
			if (_staticInstance == null) {				
				lock (_syncRoot) {
					_staticInstance = FindObjectOfType (typeof(MainMenu)) as MainMenu;
					if (_staticInstance == null) {
						Debug.LogError("The MainMenu instance was unable to be found, if this error persists please contact support.");						
					}
				}
			}
			return _staticInstance;
		}
	}

	#endregion
}
