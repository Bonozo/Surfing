using UnityEngine;
using System.Collections;

public class SliceScreen : MonoBehaviour {
	
	public enum SliceDirection{Horizontal,Vertical};
	
	public UIPanel panel;
	public SliceDirection direction;
	public float min,max;
	public float speed;
	
	private float current;
	bool rising=false;
	
	public bool IsOpen{ get{ return current>min; }}
	public bool IsOpening { get { return rising; }}
	
	public void ShowSettings(bool value)
	{
		rising=value;
	}
	
	void OnClick()
	{
		ShowSettings(!rising);
	}
	
	public void SetOpened()
	{
		rising = true;
		current = max;
		UpdateStep();
	}
	
	public void SetClosed()
	{
		rising = false;
		current = min;
		UpdateStep();
	}
	
	void Awake()
	{
		current=min;
	}
	
	
	void Update()
	{	
		UpdateStep();
	}
	
	void UpdateStep()
	{
		if(rising) current += Time.fixedDeltaTime*speed;
		else current -= Time.fixedDeltaTime*speed;
		current = Mathf.Clamp(current,min,max);
		Vector4 val = panel.clipRange;
		if(direction == SliceDirection.Horizontal) val.z = current;
		else val.w = current;
		panel.clipRange = val;
		panel.gameObject.SetActive(current>min);	
	}
}
