using UnityEngine;
using System.Collections;

public class CoinPlop : MonoBehaviour {
	
	public Coin[] coins;
	private Vector3 plopPos;
	private int plopCount = 0;
	
	private bool plotStart = false;
	
	// Use this for initialization
	void Start () {
		plopPos = transform.position;
	}
	
	void Update () {
		if(Vector3.Distance(plopPos, transform.position) > 50){
			if(!plotStart){
				Plop();
			}
		}
	}
	
	int[] values = new int[] {1,5,10,50,100,500};
	void Plop(){
		plotStart = true;
		Vector3 firstPos;
		firstPos = transform.position;
		
		int coinSum;
		coinSum = Random.Range(3, 7);
		int[] coinValue = new int[coinSum];
		bool is_arctic_level = (Application.loadedLevelName == "Level_Arctic");
		for(int i=0;i<coinSum;i++)
			coinValue[i]=values[Random.Range(0,is_arctic_level?values.Length-1:values.Length)];
		System.Array.Sort(coinValue);
		
		//throw in random newHeight modifyer
		float hightMod;
//		hightMod = Random.Range(0.8f,1.0f);
		hightMod = 1.0f;
		
		//iterate through the pool of coins, reenable and place them ahead
		for(int i = 0; i < coinSum; i++){
			
//			firstPos.x += 1;
//			Debug.Log(firstPos);
			float newHeight;
			Path.m_path.GetHeight(firstPos + (Vector3.right*i*1.5f), out newHeight);
//			Debug.Log("placing Coin at: " + newHeight);
			coins[plopCount % coins.Length].transform.position = new Vector3(firstPos.x + (i*1.5f), newHeight+hightMod, transform.position.z); 
			coins[plopCount % coins.Length].Reset(coinValue[i]);
			plopCount ++;
		}
		plopPos = transform.position;
		plotStart = false;
		
		
//		//drops coins
//		//instantiate should be replaced with..
//		// have a pool of coin objects and iterate throug them to reenable all their coins and drop them ahead
//		
//		//send reset command to all the children of this coin!
//		Component[] coinPlops;
//	    coinPlops = coins[plopCount].GetComponentsInChildren<Coin>();
//	    foreach (Coin plop in coinPlops) {
//	        plop.Reset();
//			float newHeight;
//			Path.m_path.GetHeight(coins[plopCount].position, out newHeight);
//			coins[plopCount].position = new Vector3(transform.position.x, newHeight + 1, transform.position.z); 
//			
//	    }
	}
}
