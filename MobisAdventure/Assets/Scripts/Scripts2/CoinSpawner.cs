using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
	public static int m_minNumCoins = 5;
	public static int m_maxNumCoins = 10;
	public static int m_minHeightOffGround = 1;
	public static int m_maxHeightOffGround = 1;
	public static int m_minSpawnsPerCurve = 2;
	public static int m_maxSpawnsPerCurve = 4;
	
	private static Vector3 m_offset = Vector3.zero;
	private static List<Coin> m_inactiveCoins = new List<Coin>();
	
	public static void CreateCoins()
	{
		int size = MeshManager.Current.m_numCurveMeshes * MeshManager.Current.m_numCurvesTaken * Path.Current.m_numCurvePositions;
		m_inactiveCoins = new List<Coin>(size);
		
		for(int i = 0; i < size; ++i)
		{
			GameObject obj = GameObject.Instantiate(Resources.Load("Coin"), Vector3.zero, Quaternion.identity) as GameObject;
			DestroyCoin(obj.GetComponent<Coin>());
		}
	}
	
	public static void SpawnCoins(List<Coin> coinsOUT, Vector3[] positionsIN)
	{
		if(m_inactiveCoins.Count == 0)
			CreateCoins();
		
		DestroyCoins(coinsOUT);
		
		int spawnsPerCurve = Random.Range(m_minSpawnsPerCurve, m_maxSpawnsPerCurve);
		int size = (positionsIN.Length/spawnsPerCurve) - m_maxNumCoins - 1;
		int startingIndex = 0;
		
		for(int i = 0; i < spawnsPerCurve; ++i)
		{
			startingIndex = Random.Range(startingIndex, startingIndex + size);
			int numCoins = Random.Range(m_minNumCoins, m_maxNumCoins);
			
			m_offset.y = Random.Range(m_minHeightOffGround, m_maxHeightOffGround);
			
			for(int j = startingIndex, length = startingIndex + numCoins; j < length; ++j)
				coinsOUT.Add(InstantiateCoin(positionsIN[j] + m_offset, Quaternion.identity));
		}
	}
	
	public static void DestroyCoins(List<Coin> coins)
	{
		for(int i = 0; i < coins.Count; ++i)
			DestroyCoin(coins[i]);
		
		coins.Clear();
	}
	
	public static void DestroyCoin(Coin coin)
	{
		m_inactiveCoins.Add(coin);
		coin.transform.parent = MeshManager.Current.transform;
		coin.gameObject.SetActive(false);
	}
			
	public static Coin InstantiateCoin(Vector3 position, Quaternion rotation)
	{
		Coin coin = m_inactiveCoins[0];
		m_inactiveCoins.RemoveAt(0);
		coin.gameObject.SetActive(true);
		coin.transform.position = position;
		coin.transform.rotation = rotation;
		
		return coin;
	}
}
