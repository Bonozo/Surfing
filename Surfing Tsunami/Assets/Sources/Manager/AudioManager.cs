using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	public AudioSource audioLoop;
	public AudioSource audioPlayer;
	
	public AudioClip clipMenu;
	public AudioClip clipStageMusic;
	public AudioClip clipStageEnd;
	
	public AudioClip clipCollectable;
	public AudioClip clipBump;
	public AudioClip clipWipeOut;
	
	public AudioClip clipPowerupInvincibility;
	
	public void PlayMenuLoop() { audioLoop.Stop(); audioLoop.clip = clipMenu; audioLoop.Play(); } 
	public void PlayGameLoop() { audioLoop.Stop(); audioLoop.clip = clipStageMusic; audioLoop.Play(); }
	
	public void PauseLoop() { audioLoop.Pause(); }
	public void PlayLoop() { audioLoop.Play(); }
	
	public void StopPlayer() { audioPlayer.Stop(); }
	
	
}
