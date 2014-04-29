using UnityEngine;
using System.Collections;

[AddComponentMenu("EndItems/Multiple")]
public class EndItemMultipleEndItem : EndItem {

	private EndItemChangeTexture changeTexture = null;
	private EndItemColor color = null;
	private EndItemLeaveScreen leaveScreen = null;
	private EndItemMoveTo moveTo = null;
	private EndItemScaleTo scaleTo = null;

	void Awake()
	{
		if (GetComponent<EndItemChangeTexture> () != null)
			changeTexture = GetComponent<EndItemChangeTexture> ();

		if( GetComponent<EndItemColor>() != null)
			color = GetComponent<EndItemColor>();

		if( GetComponent<EndItemLeaveScreen>() != null)
			leaveScreen = GetComponent<EndItemLeaveScreen>();

		if( GetComponent<EndItemMoveTo>() != null)
			moveTo = GetComponent<EndItemMoveTo>();

		if( GetComponent<EndItemScaleTo>() != null)
			scaleTo = GetComponent<EndItemScaleTo>();
	}

	public override void Reset ()
	{
		if(changeTexture!=null) changeTexture.Reset ();
		if(color!=null) color.Reset ();
		if(leaveScreen!=null) leaveScreen.Reset ();
		if(moveTo!=null) moveTo.Reset ();
		if(scaleTo!=null) scaleTo.Reset ();
	}
	
	public override void Work ()
	{
		if(changeTexture!=null) changeTexture.Work ();
		if(color!=null) color.Work ();
		if(leaveScreen!=null) leaveScreen.Work ();
		if(moveTo!=null) moveTo.Work ();
		if(scaleTo!=null) scaleTo.Work ();
	}
}
