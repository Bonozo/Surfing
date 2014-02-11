using UnityEngine;
using System.Collections;

public class EndItemMultipleEndItem : EndItem {

	public EndItemChangeTexture changeTexture;
	public EndItemColor color;
	public EndItemLeaveScreen leaveScreen;
	public EndItemMoveTo moveTo;
	public EndItemScaleTo scaleTo;

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
