using UnityEngine;
using System.Collections;

public class RestartGame : MonoBehaviour {
	
	public GameObject game;

	private GameObject current;

	void Awake()
	{
		current = Instantiate (game) as GameObject;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Destroy(current);
			current = Instantiate (game) as GameObject;
		}
	}
}
