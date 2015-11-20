using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GUIVIPRewardInfo : MonoBehaviour
{
	private VIPRewardGrid mVIPRewardGrid;

	private List<VIPRewardData> VipDatas;

	public void Init()
	{
		this.mVIPRewardGrid = base.transform.FindChild("rewardPanel/rewardContents").gameObject.AddComponent<VIPRewardGrid>();
		this.mVIPRewardGrid.maxPerLine = 1;
		this.mVIPRewardGrid.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mVIPRewardGrid.cellWidth = 645f;
		this.mVIPRewardGrid.cellHeight = 138f;
		if (this.VipDatas == null)
		{
			this.VipDatas = new List<VIPRewardData>();
			foreach (VipLevelInfo current in Globals.Instance.AttDB.VipLevelDict.Values)
			{
				if (current != null)
				{
					VIPRewardData item = new VIPRewardData(current);
					this.VipDatas.Add(item);
				}
			}
		}
	}

	public void Refresh()
	{
		if (Globals.Instance.Player != null)
		{
			GameCache.Data.VIPRewardStamp = Globals.Instance.Player.GetTimeStamp();
			GameCache.UpdateNow = true;
		}
		LocalPlayer player = Globals.Instance.Player;
		this.mVIPRewardGrid.ClearData();
		for (int i = 0; i < this.VipDatas.Count; i++)
		{
			VIPRewardData vIPRewardData = this.VipDatas[i];
			if (!vIPRewardData.IsPayVipRewardTaken())
			{
				if ((long)Tools.GetVipLevel(vIPRewardData.VipInfo) <= (long)((ulong)(player.Data.VipLevel + 2u)))
				{
					this.mVIPRewardGrid.AddData(vIPRewardData);
				}
			}
		}
		this.mVIPRewardGrid.repositionNow = true;
	}

	public void OnMsgBuyVipReward(MemoryStream stream)
	{
		MS2C_BuyVipReward mS2C_BuyVipReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_BuyVipReward), stream) as MS2C_BuyVipReward;
		if (mS2C_BuyVipReward.Type != 0)
		{
			return;
		}
		if (mS2C_BuyVipReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PlayerR", mS2C_BuyVipReward.Result);
			return;
		}
		GameAnalytics.BuyVipRewardEvent(mS2C_BuyVipReward.VipInfoID);
		base.StartCoroutine(GameUIVip.DoReward(mS2C_BuyVipReward.VipInfoID));
		this.Refresh();
	}

	public static bool ShouldShowTab()
	{
		LocalPlayer player = Globals.Instance.Player;
		foreach (VipLevelInfo current in Globals.Instance.AttDB.VipLevelDict.Values)
		{
			if (current != null)
			{
				if (!player.IsPayVipRewardTaken(current))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool CanBuyRewardMark()
	{
		if (GameCache.Data != null && GameCache.Data.VIPRewardStamp != 0 && Time.time - (float)GameCache.Data.VIPRewardStamp < 86400f)
		{
			return false;
		}
		LocalPlayer player = Globals.Instance.Player;
		foreach (VipLevelInfo current in Globals.Instance.AttDB.VipLevelDict.Values)
		{
			if (current != null)
			{
				if ((ulong)player.Data.VipLevel >= (ulong)((long)Tools.GetVipLevel(current)) && !player.IsPayVipRewardTaken(current))
				{
					return true;
				}
			}
		}
		return false;
	}
}
