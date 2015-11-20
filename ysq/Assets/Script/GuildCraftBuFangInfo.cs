using Att;
using Proto;
using System;
using UnityEngine;

public class GuildCraftBuFangInfo : MonoBehaviour
{
	private UILabel mBeginTime;

	private UILabel mTeamPos0;

	private float mRefreshTimer;

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.mRefreshTimer = Time.time;
		this.RefreshCDTime();
	}

	private void CreateObjects()
	{
		this.mBeginTime = base.transform.Find("txt0/name").GetComponent<UILabel>();
		this.mTeamPos0 = base.transform.Find("txt1/name").GetComponent<UILabel>();
		GameObject gameObject = base.transform.Find("teamBtn").gameObject;
		UIEventListener expr_52 = UIEventListener.Get(gameObject);
		expr_52.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_52.onClick, new UIEventListener.VoidDelegate(this.OnTeamBtnClick));
	}

	public void Refresh()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
		{
			GuildWarStronghold selfStrongHold = Globals.Instance.Player.GuildSystem.GetSelfStrongHold();
			if (selfStrongHold != null)
			{
				GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(selfStrongHold.ID);
				if (info != null)
				{
					this.mTeamPos0.text = info.StrongholdName;
				}
				else
				{
					this.mTeamPos0.text = Singleton<StringManager>.Instance.GetString("guildCraft5");
				}
			}
			else
			{
				this.mTeamPos0.text = Singleton<StringManager>.Instance.GetString("guildCraft5");
			}
			this.RefreshCDTime();
		}
	}

	private void RefreshCDTime()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
		{
			int num = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num <= 0)
			{
				num = 0;
			}
			string text = Tools.FormatTime(num);
			this.mBeginTime.text = text;
		}
	}

	private void Update()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare) && Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			this.RefreshCDTime();
		}
	}

	private void OnTeamBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
		{
			GameUIManager.mInstance.uiState.IsLocalPlayer = true;
			GameUIManager.mInstance.uiState.CombatPetSlot = 0;
			GameUIManager.mInstance.ChangeSession<GUITeamManageSceneV2>(null, false, true);
		}
	}
}
