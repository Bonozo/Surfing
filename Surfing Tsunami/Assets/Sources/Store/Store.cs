using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	
	#region Player Prefabs
	
	private int _coins;
	public int Coins{
		get{
			return _coins;
		}
		set{
			_coins=value;
			PlayerPrefs.SetInt("coins",_coins);
			guiCoins.text = "" + _coins;
		}
	}
	
	void Awake()
	{
		Coins = PlayerPrefs.GetInt("coins",0);
		
		FingerGestures.OnDragMove += HandleFingerGesturesOnDragMove;
		
		ShowStore=false;
	}
	
	#endregion
	
	#region References
	
	public GameObject gui;
	public Camera camera2d;
	public Transform buttonsDelta;
	public UILabel guiCoins;
	public GameObject buttonGame;
	
	public GameObject popup;
	public UILabel popupName;
	public UISprite popupIcon;
	public UILabel popupCost;
	
	#endregion
	
	#region Public Field
	
	private bool _showStore = false;
	public bool ShowStore{
		get{
			return _showStore;
		}
		set{
			_showStore = value;
			gui.SetActive(_showStore);
			if(_showStore)
			{
				buttonGame.SetActive(false);
				popup.SetActive(false);
			}
		}
	}
	
	[System.NonSerializedAttribute]
	public UpdateablePowerup _currentPowerup;
	
	#endregion

	#region Input Events
	
	void HandleFingerGesturesOnDragMove (Vector2 fingerPos, Vector2 delta)
	{
		if(!ShowStore) return;
		fingerPos = camera2d.transform.worldToLocalMatrix * camera2d.ScreenToWorldPoint( fingerPos );
		fingerPos += new Vector2(camera2d.pixelWidth*0.5f,camera2d.pixelHeight*0.5f);
		if( fingerPos.x >= 30f && fingerPos.x <= 350 && fingerPos.y >= 10f && fingerPos.y <= 410f )
			buttonsDelta.localPosition = new Vector3(buttonsDelta.localPosition.x,Mathf.Clamp(buttonsDelta.localPosition.y+delta.y,0f,80f),buttonsDelta.localPosition.z);
	}
	
	#endregion
	
	#region Powerups
	
	/*public UpdateablePowerup powerupSureShot;
	public UpdateablePowerup powerupMagned;
	public UpdateablePowerup powerupLighenUp;
	public UpdateablePowerup powerupFreeze;
	public UpdateablePowerup powerupShazam;
	public UpdateablePowerup powerupIntergalactic;*/
	
	
	public readonly int[] costs = {30,100,300,1000,3000};
	public void Activate(UpdateablePowerup powerup)
	{
		_currentPowerup = powerup;
		
		popupName.text = powerup.powerupName.ToUpper()+"!";
		popupCost.text = powerup.level==4?"FULLY UPGRATED":"LEVEL " + (powerup.level+1) + " (" + costs[powerup.level]+")";
		popup.SetActive(true);
	}
	
	#endregion
	
	#region  Safe Store
	
	//Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile Store _staticInstance;	
    public static Store Instance {
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(Store)) as Store;
                    if (_staticInstance == null) {
                       Debug.LogError("The Store instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion
}
