using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {
	
	public tk2dButton buttonMainScreen;
	public tk2dButton buttonShop;
	public tk2dButton buttonPlayAgain;
	public tk2dTextMesh textDistance;
	public tk2dTextMesh textCoins;
	
	public float firstWaitTime = 0.5f;
	public float tickTime = 0.05f;
	
	void OnEnable()
	{
		buttonMainScreen.ButtonPressedEvent += HandleButtonMainScreenButtonPressedEvent;
		buttonShop.ButtonPressedEvent += HandleButtonShopButtonPressedEvent;
		buttonPlayAgain.ButtonPressedEvent += HandleButtonPlayAgainButtonPressedEvent;
		
		textDistance.text = textCoins.text = "0";
		textDistance.Commit(); textCoins.Commit();
		
		StartScorboard();
	}
	
	void OnDisable()
	{
		buttonMainScreen.ButtonPressedEvent -= HandleButtonMainScreenButtonPressedEvent;
		buttonShop.ButtonPressedEvent -= HandleButtonShopButtonPressedEvent;
		buttonPlayAgain.ButtonPressedEvent -= HandleButtonPlayAgainButtonPressedEvent;		
	}

	void HandleButtonMainScreenButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Title;
	}
	
	void HandleButtonShopButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.state = GameState.Store;
	}
	
	void HandleButtonPlayAgainButtonPressedEvent (tk2dButton source)
	{
		LevelInfo.State.StartNewGame();
	}
	
	public void StartScorboard()
	{
		StartCoroutine(ScorboardThread());
	}
	
	private IEnumerator ScorboardThread()
	{
		LevelInfo.Audio.audioPlayer.PlayOneShot(LevelInfo.Audio.clipStageEnd);
		
		textDistance.text = textCoins.text = "0";
		textDistance.Commit(); textCoins.Commit();
		
		
		//yield return new WaitForSeconds(firstWaitTime);
		
		int distance = (int)LevelInfo.Environments.surfer.distanceTravelled;
		int coins = LevelInfo.Environments.surfer.coins;
		Store.Instance.Coins += coins;
		
		int x=0,remainingCoins;
		while(x<distance)
		{
			remainingCoins = distance-x;
			if ( remainingCoins >= 4000 )
			{		
				x += 1000;
			}
			else if ( remainingCoins >= 200)
			{
				x += 100;
			}
			else if ( remainingCoins >= 10 )
			{
				x += 10;
			}
			else
			{
				x++;
			}
			
			textDistance.text = "" + x;
			textDistance.Commit();
			LevelInfo.Audio.audioPlayer.PlayOneShot(LevelInfo.Audio.clipCollectable);
			yield return new WaitForSeconds(tickTime);
		}
			
		x=0;
		while(x<coins)
		{
			remainingCoins = coins-x;
			if ( remainingCoins >= 4000 )
			{		
				x += 1000;
			}
			else if ( remainingCoins >= 200)
			{
				x += 100;
			}
			else if ( remainingCoins >= 10 )
			{
				x += 10;
			}
			else
			{
				x++;
			}
			
			textCoins.text = "" + x;
			textCoins.Commit();
			LevelInfo.Audio.audioPlayer.PlayOneShot(LevelInfo.Audio.clipCollectable);
			yield return new WaitForSeconds(tickTime);
		}
	}
	
	void Update()
	{
		if(Input.GetKeyUp(KeyCode.H))
			StartScorboard();
	}
}
