using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SettingsItem))]
public class OptionsButtonSound : MonoBehaviour {

	public AudioSource[] sources;
	private SettingsItem settingsItem;

	void Awake()
	{
		settingsItem = GetComponent<SettingsItem> ();
		Setup ();
	}

	void Update()
	{
		Setup();
	}

	void Setup()
	{
		foreach(var s in sources)
			s.volume = settingsItem.status?1f:0f;
		NGUITools.soundVolume = settingsItem.status?1f:0f;
	}
}
