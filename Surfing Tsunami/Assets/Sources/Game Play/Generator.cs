using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	public GameObject[] obstacles;
	public GameObject[] coin;
	public GameObject[] powerups;
	public GameObject[] crests;
	public float spawnIntervalCoin = 3f;
	public float spawnIntervalObstacle = 5.4f;
	public float spawnIntervalPowerup = 7.4f;
	public float spawnIntervalCrestMin = 0.5f,spawnIntervalCrestMax=1.5f;
	// Update is called once per frame
	void Update () 
	{
		if(LevelInfo.State.state == GameState.Play)
			SpawnUpdates();
		if(LevelInfo.State.IsAnimatedState)
			CrestUpdates();
	}
	
	private float spawnintcoin=3f;
	private float spawnintobstacle=0f;
	private float spawnintpowerup=10f;	
	private void SpawnUpdates()
	{
		spawnintcoin -= Time.deltaTime;
		if(spawnintcoin <= 0f)
		{
			SpawnCoin();
			spawnintcoin=spawnIntervalCoin;
		}
		
		spawnintobstacle -= Time.deltaTime;
		if(spawnintobstacle <= 0f)
		{
			SpawnObstacle();
			spawnintobstacle=spawnIntervalObstacle;
		}
		
		spawnintpowerup -= Time.deltaTime;
		if(spawnintpowerup <= 0f)
		{
			SpawnPowerup();
			spawnintpowerup=spawnIntervalObstacle;
		}
	}
	
	private void SpawnObstacle()
	{
		int index = Random.Range(0,obstacles.Length);
		GameObject c = (GameObject)Instantiate(obstacles[index]);//,RandomSpawnPosition(obstacles[index].GetComponent<SpawnedItem>().Depth),Quaternion.identity);
		c.transform.parent = LevelInfo.Environments.transformObstacles;
	}	
	private void SpawnCoin()
	{
		int index = Random.Range(0,coin.Length);
		GameObject c = (GameObject)Instantiate(coin[index]);//,RandomSpawnPosition(LevelInfo.Environments.depthCoins),Quaternion.identity);
		c.transform.parent = LevelInfo.Environments.transformObstacles;
	}
	
	private void SpawnPowerup()
	{
		int index = Random.Range(0,powerups.Length);
		GameObject c = (GameObject)Instantiate(powerups[index]);//,RandomSpawnPosition(LevelInfo.Environments.depthCoins),Quaternion.identity);
		c.transform.parent = LevelInfo.Environments.transformObstacles;
	}
	
	public Vector3 RandomSpawnPosition(float depth)
	{
		float delta = 0f;
		float offset = LevelInfo.Camera.Height*0.2f;
		switch(LevelInfo.State.wind)
		{	
		case WindState.Down:
			return new Vector3(Random.Range(0f,LevelInfo.Camera.Width-delta),-offset,depth);
		case WindState.Left:
			return new Vector3(-offset,Random.Range(0f,LevelInfo.Camera.Height-delta),depth);
		case WindState.Right:
			return new Vector3(LevelInfo.Camera.Width+offset,Random.Range(0f,LevelInfo.Camera.Height-delta),depth);
		case WindState.DownLeft:
			if(Random.Range(0,2)==1)
				return new Vector3(-offset,Random.Range(0f,LevelInfo.Camera.Height-delta),depth);
			return new Vector3(Random.Range(0f,LevelInfo.Camera.Width-delta),-offset,depth);
		case WindState.DownRight:
			if(Random.Range(0,2)==1)
				return new Vector3(LevelInfo.Camera.Width+offset,Random.Range(0f,LevelInfo.Camera.Height-delta),depth);
			return new Vector3(Random.Range(0f,LevelInfo.Camera.Width-delta),-offset,depth);
		}
		Debug.LogError("Objects spawn is not determined for " + LevelInfo.State.wind + " state");
		return Vector3.zero;
	}
	
	private float cresttime = 1f;
	private void CrestUpdates()
	{
		cresttime -= Time.deltaTime;
		if(cresttime <= 0f)
		{
			Instantiate(crests[Random.Range(0,crests.Length)]);
			cresttime = Random.Range(spawnIntervalCrestMin,spawnIntervalCrestMax);
		}
	}
}
