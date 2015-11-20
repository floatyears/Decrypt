using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GuildCraftMapTopCD : MonoBehaviour
{
	private UILabel mBidCdTime;

	private float mRefreshTimer;

	private UILabel mBidCdDesc;

	private int mLastCdTime = -1;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.Refresh();
		this.mRefreshTimer = Time.time;
	}

	private void CreateObjects()
	{
		this.mBidCdDesc = base.transform.GetComponent<UILabel>();
		this.mBidCdTime = base.transform.Find("time").GetComponent<UILabel>();
	}

	private void OnEnterStateNormal()
	{
		this.mBidCdDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft51");
	}

	private void OnEnterStateFFHalfHourBefore()
	{
		this.mBidCdDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft52");
	}

	private void OnEnterStateFinalFourPrepare()
	{
		this.mBidCdDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft53");
	}

	private void OnEnterStateFinalFourGoing()
	{
		this.mBidCdDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft54");
	}

	private void OnEnterStateFinalFourEnd()
	{
		this.mBidCdDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft55");
	}

	private void OnEnterStateFinalPrepare()
	{
		this.mBidCdDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft56");
	}

	private void OnEnterStateFinalGoing()
	{
		this.mBidCdDesc.text = Singleton<StringManager>.Instance.GetString("guildCraft57");
	}

	private void RefreshCDTime()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo != null)
		{
			int num = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num < 0)
			{
				num = 0;
			}
			this.mBidCdTime.text = Tools.FormatTimeStr2(num, false, false);
			if (this.mLastCdTime != num)
			{
				this.mLastCdTime = num;
				if (this.mLastCdTime == 0)
				{
					Globals.Instance.Player.GuildSystem.RequestQueryWarState();
				}
			}
		}
	}

	[DebuggerHidden]
	private IEnumerator RefreshCDCor()
	{
        return null;
        //GuildCraftMapTopCD.<RefreshCDCor>c__Iterator59 <RefreshCDCor>c__Iterator = new GuildCraftMapTopCD.<RefreshCDCor>c__Iterator59();
        //<RefreshCDCor>c__Iterator.<>f__this = this;
        //return <RefreshCDCor>c__Iterator;
	}

	private void Refresh()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo != null)
		{
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_Normal)
			{
				this.OnEnterStateNormal();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_SelectFourTeam)
			{
				this.OnEnterStateFFHalfHourBefore();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare)
			{
				this.OnEnterStateFinalFourPrepare();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
			{
				this.OnEnterStateFinalFourGoing();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd)
			{
				this.OnEnterStateFinalFourEnd();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
			{
				this.OnEnterStateFinalPrepare();
			}
			else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				this.OnEnterStateFinalGoing();
			}
			base.StartCoroutine(this.RefreshCDCor());
		}
	}

	private void Update()
	{
		if (Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			this.RefreshCDTime();
		}
	}
}
