using UnityEngine;
using System.Collections;

public class SurferAnimations : MonoBehaviour {
	
	public tk2dAnimatedSprite surfer;
	public tk2dAnimatedSprite board;
	
	public tk2dButton button;
	public tk2dTextMesh textmesh;
	public string[] anims;
	
	private int current=0;
	
	void OnEnable()
	{
		button.ButtonPressedEvent += HandleButtonButtonPressedEvent;
		if(anims.Length==0)
		{
			Debug.LogError("Surfer anim test animations size is 0");
		}
		UpdateText();
	}
	
	void OnDisable()
	{
		button.ButtonPressedEvent -= HandleButtonButtonPressedEvent;		
	}
	
	void HandleButtonButtonPressedEvent (tk2dButton source)
	{
		current = (current + 1) % anims.Length;
		surfer.Play("surfer2_" + anims[current]);
		board.Play("board1_" + anims[current]);
		UpdateText();
	}
	
	void UpdateText()
	{
		textmesh.text = anims[current].ToUpper();
		textmesh.Commit();
	}
	
	void Update()
	{
		if(!surfer.Playing)
		{
			surfer.Play("surfer2_" + anims[current]);
			board.Play("board1_" + anims[current]);	
		}
	}
	

}
