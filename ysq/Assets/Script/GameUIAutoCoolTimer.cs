using System;
using UnityEngine;

public class GameUIAutoCoolTimer : MonoBehaviour
{
	private UISprite mCoolSprite;

	private float mCoolTimeSec;

	private float mCurrentCoolTimeSec;

	private void Awake()
	{
		this.mCoolSprite = null;
		this.mCoolTimeSec = 0f;
		this.mCurrentCoolTimeSec = 0f;
	}

	public void PlayCool(UISprite coolSprite, float current, float maxDuration)
	{
		this.mCoolSprite = coolSprite;
		this.mCoolTimeSec = maxDuration;
		this.mCurrentCoolTimeSec = current;
	}

	private void Update()
	{
		if (null == this.mCoolSprite)
		{
			return;
		}
		if (this.mCoolTimeSec == 0f)
		{
			this.mCoolSprite.fillAmount = 0f;
			return;
		}
		if (this.mCurrentCoolTimeSec > this.mCoolTimeSec)
		{
			return;
		}
		this.mCurrentCoolTimeSec += Time.deltaTime;
		if (this.mCurrentCoolTimeSec > this.mCoolTimeSec)
		{
			this.mCurrentCoolTimeSec = this.mCoolTimeSec;
		}
		this.mCoolSprite.fillAmount = this.mCurrentCoolTimeSec / this.mCoolTimeSec;
	}
}
