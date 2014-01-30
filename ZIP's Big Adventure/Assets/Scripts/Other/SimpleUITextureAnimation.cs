using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UITexture))]
public class SimpleUITextureAnimation : MonoBehaviour {

	public Texture[] textures;
	public float frameRate=1f;
	
	private float timer = 0f;
	private float checkTime = 0f;
	private int index=0;
	private UITexture uiTexture;
	
	void Awake()
	{
		uiTexture = GetComponent<UITexture>();
		if(textures.Length == 0)
			Debug.Log("TitleScreenAnimation texture count is 0.");
	}
	
	void OnEnable()
	{
		timer = 0f;
		checkTime = frameRate;
		index=0;
		uiTexture.mainTexture = textures[0];
	}
	
	void Update()
	{
		timer+=Time.deltaTime;
		if(timer>=checkTime)
		{
			if(++index==textures.Length) index=0;
			uiTexture.mainTexture = textures[index];
			checkTime+=frameRate;
		}
	}
}
