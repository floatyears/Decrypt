using Proto;
using System;
using System.Collections.Generic;

public class GuildWarStateInfo
{
	public List<GuildWarClientCity> mTowerDatas = new List<GuildWarClientCity>();

	public List<GuildWarClient> mWarDatas = new List<GuildWarClient>();

	public EGuildWarState mWarState
	{
		get;
		set;
	}

	public int mTimeStamp
	{
		get;
		set;
	}

	public void ResetStateInfo(EGuildWarState state, int timeStamp, List<GuildWarClientCity> tDatas, List<GuildWarClient> warDatas)
	{
		this.mWarState = state;
		if (this.mWarState == EGuildWarState.EGWS_FinalFourPrepare || this.mWarState == EGuildWarState.EGWS_FinalFourGoing || this.mWarState == EGuildWarState.EGWS_FinalPrepare || this.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			GameUIManager.mInstance.uiState.IsShowedGuildWarResult = false;
		}
		else
		{
			Globals.Instance.Player.GuildSystem.ClearGuildWarPromptMsg();
		}
		this.mTimeStamp = timeStamp;
		if (tDatas != null && this.mTowerDatas != null)
		{
			this.mTowerDatas.Clear();
			for (int i = 0; i < tDatas.Count; i++)
			{
				this.mTowerDatas.Add(tDatas[i]);
			}
		}
		if (warDatas != null && this.mWarDatas != null)
		{
			this.mWarDatas.Clear();
			for (int j = 0; j < warDatas.Count; j++)
			{
				this.mWarDatas.Add(warDatas[j]);
			}
		}
	}
}
