using UnityEngine;
using System.Collections;

public abstract class ZIPTrace : ZIPLevel{
	public abstract void Complete();
}

public class Trace : MonoBehaviour {

	[SerializeField]
	public ZIPTrace levelTrace;
	public Transform traceStar;
	private int completed = 1;
	private int total = 0;
	private TraceElement[] elems;

	bool inited = false;
	void Init()
	{
		total = transform.childCount;
		elems = new TraceElement[total];
		foreach(Transform child in transform)
		{
			var cur = child.GetComponent<TraceElement>();
			elems[cur.index-1] = cur;
		}
		inited=true;
	}

	public void Reset()
	{
		if(!inited)
			Init();

		for(int i=0;i<elems.Length;i++)
		{
			elems[i].GetComponent<UISprite>().enabled = false;
			elems[i].collider.enabled = false;
		}

		completed = 0;
		elems[0].collider.enabled = true;
		
		traceStar.gameObject.SetActive(true);
		traceStar.localPosition = elems[0].transform.localPosition;
	}

	public void Progress(TraceElement elem)
	{
		if(elem.index == completed+1)
		{
			elems[completed].collider.enabled = false;
			elems[completed].GetComponent<UISprite>().enabled = true;
			completed++;
			if(Finished)
			{
				traceStar.gameObject.SetActive(false);
				levelTrace.Complete();
			}
			else
			{
				elems[completed].collider.enabled=true;
				traceStar.localPosition = elems[completed].transform.localPosition;
			}
		}
	}

	public bool Finished{ get{ return completed == total; }}

	public int editorDepth = 6;
	public Color editorColor;
	public void IndexEditor()
	{
		for(int i=0;i<transform.childCount;i++)
		{
			transform.GetChild(i).GetComponent<TraceElement>().index=i+1;
			transform.GetChild(i).GetComponent<UISprite>().depth = editorDepth;
			transform.GetChild(i).GetComponent<UISprite>().color = editorColor;
		}
	}
}
