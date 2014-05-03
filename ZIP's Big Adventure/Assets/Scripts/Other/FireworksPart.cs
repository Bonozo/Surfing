using UnityEngine;
using System.Collections;

public class FireworksPart : MonoBehaviour {

	public AudioClip[] clips;

	void Start()
	{
		Destroy (this.gameObject,4f);
		audio.volume = 0.5f;
		audio.clip = clips [Random.Range (0, clips.Length)];
		audio.Play ();
	}
}
