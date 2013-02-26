using UnityEngine;
using System.Collections;

public class Powerup : SpawnedItem {
	
	public Powerups powerupType;
	
	protected override void Start () {
		base.Start();
		tag = "Powerup";
	}
	
	/*protected override void Update ()
	{
		base.Update();
	}*/
	
}
