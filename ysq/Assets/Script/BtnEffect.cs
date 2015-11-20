using System;
using System.Collections.Generic;
using UnityEngine;

public class BtnEffect : MonoBehaviour
{
	public int FPS;

	public string NamePrefix;

	public float DelayTime;

	private List<string> mSpriteNames = new List<string>();

	private UISprite mSprite;

	private float mDelta;

	private float mDelayDelta;

	private int mCurNameIndex;

	private bool mActive = true;

	private bool mHide;

	private void Awake()
	{
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		if (this.mSprite == null)
		{
			this.mSprite = base.GetComponent<UISprite>();
		}
		this.mSpriteNames.Clear();
		if (this.mSprite != null && this.mSprite.atlas != null)
		{
			List<UISpriteData> spriteList = this.mSprite.atlas.spriteList;
			int i = 0;
			int count = spriteList.Count;
			while (i < count)
			{
				UISpriteData uISpriteData = spriteList[i];
				if (string.IsNullOrEmpty(this.NamePrefix) || uISpriteData.name.StartsWith(this.NamePrefix))
				{
					this.mSpriteNames.Add(uISpriteData.name);
				}
				i++;
			}
			this.mSpriteNames.Sort();
		}
		this.Play();
	}

	protected virtual void Update()
	{
		if (this.mDelayDelta > 0f)
		{
			this.mDelayDelta -= RealTime.deltaTime;
			return;
		}
		if (this.mHide)
		{
			this.mHide = false;
			this.mSprite.enabled = true;
		}
		if (this.mActive && Application.isPlaying && this.mSprite != null && this.mSpriteNames.Count > 1 && this.FPS > 0)
		{
			this.mDelta += RealTime.deltaTime;
			float num = 1f / (float)this.FPS;
			if (num < this.mDelta)
			{
				this.mDelta = 0f;
				if (++this.mCurNameIndex >= this.mSpriteNames.Count)
				{
					this.mCurNameIndex = 0;
					if (this.DelayTime > 0f)
					{
						this.mDelayDelta = this.DelayTime;
						this.mSprite.enabled = false;
						this.mHide = true;
					}
				}
				this.mSprite.spriteName = this.mSpriteNames[this.mCurNameIndex];
			}
		}
	}

	private void ReInit()
	{
		this.mCurNameIndex = 0;
		if (this.mSprite != null && this.mSpriteNames.Count > 1)
		{
			this.mSprite.spriteName = this.mSpriteNames[0];
		}
	}

	public void Play()
	{
		this.ReInit();
		this.mActive = true;
	}

	public void Stop()
	{
		this.mActive = false;
	}
}
