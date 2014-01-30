//----------------------------------------------
// Edit: Aharon Smbatyan
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(UISprite))]
public class UIAnimation : MonoBehaviour
{
	public int mFPS = 30;
	public string mPrefix = "";
	public bool mLoop = true;
	public bool mPixelPerfect = true;
	
	UISprite mSprite;
	float mDelta = 0f;
	int mIndex = 0;
	bool mActive = true;
	bool mPause = false;
	List<string> mSpriteNames = new List<string>();
	
	/// <summary>
	/// Number of frames in the animation.
	/// </summary>
	
	public int frames { get { return mSpriteNames.Count; } }
	
	/// <summary>
	/// Animation framerate.
	/// </summary>
	
	public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }
	
	/// <summary>
	/// Set the name prefix used to filter sprites from the atlas.
	/// </summary>
	
	public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }
	
	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>
	
	public bool loop { get { return mLoop; } set { mLoop = value; } }
	
	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>
	
	public bool isPlaying { get { return mActive; } }
	
	/// <summary>
	/// Rebuild the sprite list first thing.
	/// </summary>
	
	void Start () { RebuildSpriteList(); }
	
	/// <summary>
	/// Advance the sprite animation process.
	/// </summary>
	
	void Update ()
	{
		if (!mPause && mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0f)
		{
			mDelta += Time.deltaTime;
			float rate = 1f / mFPS;
			
			if (rate < mDelta)
			{
				
				mDelta = (rate > 0f) ? mDelta - rate : 0f;
				if (++mIndex >= mSpriteNames.Count)
				{
					mIndex = 0;
					mActive = loop;
				}
				
				if (mActive || mStopped)
				{
					mSprite.spriteName = mSpriteNames[mIndex];

					if(mPixelPerfect)
						mSprite.MakePixelPerfect();
				}
			}
		}
	}
	
	/// <summary>
	/// Rebuild the sprite list after changing the sprite name.
	/// </summary>
	
	void RebuildSpriteList ()
	{
		if (mSprite == null) mSprite = GetComponent<UISprite>();
		mSpriteNames.Clear();
		
		if (mSprite != null && mSprite.atlas != null)
		{
			List<UIAtlas.Sprite> sprites = mSprite.atlas.spriteList;
			
			for (int i = 0, imax = sprites.Count; i < imax; ++i)
			{
				UIAtlas.Sprite sprite = sprites[i];
				
				if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
				{
					mSpriteNames.Add(sprite.name);
				}
			}
			mSpriteNames.Sort();
		}
	}
	
	/// <summary>
	/// Reset the animation to frame 0 and activate it.
	/// </summary>
	
	public void Reset()
	{
		mActive = true;
		mIndex = 0;
		mPause = false;
		
		if (mSprite != null && mSpriteNames.Count > 0)
		{
			mSprite.spriteName = mSpriteNames[mIndex];
	
			if(mPixelPerfect)
				mSprite.MakePixelPerfect();
		}
	}

	// Additional Functions

	private bool mStopped = false;

	public void Pause()
	{
		mPause = true;
	}

	public void Resume()
	{
		mStopped = false;
		mPause = false;
	}

	public void Stop()
	{
		mStopped = true;
		loop = false;
	}

	public void Play()
	{
		mStopped = false;
		loop = true;
		Reset ();
	}

	public void StopImmediatly()
	{
		mPause = true;
		Reset ();
	}
}
