using UnityEngine;
using System.Collections;

public class PhysicsFPS : MonoBehaviour {

	float time = 0f;
	int frames=0;

	IEnumerator Start()
	{
		while(true)
		{
			yield return new WaitForSeconds(1f);
			GetComponent<UILabel>().text = "FPS: " + frames;
			frames = 0;
		}
	}

	void FixedUpdate()
	{
		frames++;
	}
}
