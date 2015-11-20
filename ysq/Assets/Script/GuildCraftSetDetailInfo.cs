using Att;
using Proto;
using System;
using UnityEngine;

public class GuildCraftSetDetailInfo : MonoBehaviour
{
	private GameObject mState1;

	private UILabel mGetScore;

	private UILabel mJiShaNum;

	private UILabel mDefendPos;

	private void CreateObjects()
	{
		this.mState1 = base.transform.Find("state1").gameObject;
		this.mGetScore = this.mState1.transform.Find("txt0/name").GetComponent<UILabel>();
		this.mJiShaNum = this.mState1.transform.Find("txt1/name").GetComponent<UILabel>();
		this.mDefendPos = this.mState1.transform.Find("txt2/name").GetComponent<UILabel>();
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
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			if (Globals.Instance.Player.GuildSystem.mGWEnterData == null)
			{
				return;
			}
			this.mState1.SetActive(true);
			int num = 0;
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				for (int i = 0; i < Globals.Instance.Player.GuildSystem.mGWEnterData.Strongholds.Count; i++)
				{
					if (Globals.Instance.Player.GuildSystem.mGWEnterData.Strongholds[i].Status == EGuildWarStrongholdState.EGWPS_Own && Globals.Instance.Player.GuildSystem.mGWEnterData.Strongholds[i].OwnerId == selfTeamFlag)
					{
						GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(Globals.Instance.Player.GuildSystem.mGWEnterData.Strongholds[i].ID);
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
			this.mJiShaNum.text = ((selfTeamFlag != EGuildWarTeamId.EGWTI_Red) ? Globals.Instance.Player.GuildSystem.mGWEnterData.Blue.KillNum.ToString() : Globals.Instance.Player.GuildSystem.mGWEnterData.Red.KillNum.ToString());
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
	}
}
