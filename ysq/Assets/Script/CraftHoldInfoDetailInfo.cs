using Att;
using Proto;
using System;
using UnityEngine;

public class CraftHoldInfoDetailInfo : MonoBehaviour
{
	private UILabel mGetScore;

	private UILabel mJiShaNum;

	private UILabel mDefendPos;

	private void CreateObjects()
	{
		this.mGetScore = base.transform.Find("txt0/name").GetComponent<UILabel>();
		this.mJiShaNum = base.transform.Find("txt1/name").GetComponent<UILabel>();
		this.mDefendPos = base.transform.Find("txt2/name").GetComponent<UILabel>();
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	public void Refresh()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		bool flag = Globals.Instance.Player.GuildSystem.IsGuanZhanMember(mGWEnterData.WarID);
		if (flag)
		{
			this.mGetScore.text = "0";
			this.mJiShaNum.text = "0";
			this.mDefendPos.text = Singleton<StringManager>.Instance.GetString("guildCraft5");
		}
		else
		{
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				int num = 0;
				if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
				{
					for (int i = 0; i < mGWEnterData.Strongholds.Count; i++)
					{
						if (mGWEnterData.Strongholds[i].Status == EGuildWarStrongholdState.EGWPS_Own && mGWEnterData.Strongholds[i].OwnerId == selfTeamFlag)
						{
							GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(mGWEnterData.Strongholds[i].ID);
							if (info != null)
							{
								num += info.StrongholdScore;
							}
						}
					}
				}
				this.mGetScore.text = Singleton<StringManager>.Instance.GetString("guildCraft64", new object[]
				{
					num
				});
				this.mJiShaNum.text = ((selfTeamFlag != EGuildWarTeamId.EGWTI_Red) ? mGWEnterData.Blue.KillNum.ToString() : mGWEnterData.Red.KillNum.ToString());
				GuildWarStronghold selfStrongHold = Globals.Instance.Player.GuildSystem.GetSelfStrongHold();
				if (selfStrongHold != null)
				{
					GuildInfo info2 = Globals.Instance.AttDB.GuildDict.GetInfo(selfStrongHold.ID);
					if (info2 != null)
					{
						this.mDefendPos.text = info2.StrongholdName;
					}
					else
					{
						this.mDefendPos.text = Singleton<StringManager>.Instance.GetString("guildCraft5");
					}
				}
				else
				{
					this.mDefendPos.text = Singleton<StringManager>.Instance.GetString("guildCraft5");
				}
			}
			else
			{
				this.mGetScore.text = "0";
				this.mJiShaNum.text = "0";
				this.mDefendPos.text = Singleton<StringManager>.Instance.GetString("guildCraft5");
			}
		}
	}
}
