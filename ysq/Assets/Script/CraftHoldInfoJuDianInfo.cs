using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class CraftHoldInfoJuDianInfo : MonoBehaviour
{
	private UILabel mJuDianName;

	private UILabel mOwnedGuild;

	private UILabel mOutPutScore;

	private UILabel mAttAdd;

	private string mPointStr;

	private StringBuilder mSb = new StringBuilder(42);

	private void CreateObjects()
	{
		this.mJuDianName = base.transform.Find("txt1/name").GetComponent<UILabel>();
		this.mOwnedGuild = base.transform.Find("txt2/name").GetComponent<UILabel>();
		this.mOutPutScore = base.transform.Find("txt3/name").GetComponent<UILabel>();
		this.mAttAdd = base.transform.Find("txt4/name").GetComponent<UILabel>();
		this.mPointStr = Singleton<StringManager>.Instance.GetString("point");
	}

	public void InitWithBaseScene()
	{
		this.CreateObjects();
	}

	public void Refresh()
	{
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(strongHold.ID);
		if (info != null)
		{
			this.mJuDianName.text = info.StrongholdName;
			this.mOutPutScore.text = Singleton<StringManager>.Instance.GetString("guildCraft64", new object[]
			{
				info.StrongholdScore
			});
			BuffInfo info2 = Globals.Instance.AttDB.BuffDict.GetInfo(info.StrongholdBuffID);
			if (info2 != null)
			{
				if (info2.Value3[0] == 1)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("EAID_1")).Append("+").Append(info2.Value4[0] / 100).Append("%");
				}
				else if (info2.Value3[0] == 2)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("EAID_2")).Append("+").Append(info2.Value4[0] / 100).Append("%");
				}
				else if (info2.Value3[0] == 9)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("EAID_9")).Append("+").Append(info2.Value4[0]).Append(this.mPointStr);
				}
				else if (info2.Value3[0] == 10)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("EAID_10")).Append("+").Append(info2.Value4[0]).Append(this.mPointStr);
				}
				else if (info2.Value3[0] == 20)
				{
					this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("EAID_20")).Append("+").Append(info2.Value4[0] / 100).Append("%");
				}
				this.mAttAdd.text = this.mSb.ToString();
			}
		}
		if (strongHold.OwnerId == EGuildWarTeamId.EGWTI_None)
		{
			this.mOwnedGuild.text = Singleton<StringManager>.Instance.GetString("guildCraft5");
		}
		else if (strongHold.OwnerId == EGuildWarTeamId.EGWTI_Red)
		{
			this.mOwnedGuild.text = mGWEnterData.Red.GuildName;
		}
		else if (strongHold.OwnerId == EGuildWarTeamId.EGWTI_Blue)
		{
			this.mOwnedGuild.text = mGWEnterData.Blue.GuildName;
		}
	}
}
