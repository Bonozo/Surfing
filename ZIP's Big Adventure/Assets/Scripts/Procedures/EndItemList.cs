using UnityEngine;
using System.Collections;

[AddComponentMenu("EndItems/List")]
public class EndItemList : EndItem {

	public EndItem[] list;

	public override void Reset ()
	{
		foreach(var item in list) item.Reset();
	}
	
	public override void Work ()
	{
		foreach(var item in list) item.Work();
	}

	#region Editor

	public void EditorAddChilds()
	{
		int count = 0;
		foreach(Transform item in transform)
			if(item.GetComponent<EndItem>()!=null)
				count++;
		list = new EndItem[count];
		count = 0;
		foreach(Transform item in transform)
			if(item.GetComponent<EndItem>()!=null)
				list[count++]=item.GetComponent<EndItem>();;
	}

	#endregion
}
