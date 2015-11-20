using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIRewardFlashSaleInfo : MonoBehaviour
{
	public enum EGetRewardType
	{
		ERT_Item,
		ERT_Pet,
		ERT_Slice
	}

	private struct RewardItem
	{
		public GUIRewardFlashSaleInfo.EGetRewardType rewardType;

		public int infoID;

		public uint count;
	}

	private GUIRewardCheckBtn mCheckBtn;

	private UILabel mRemainingTime;

	private List<GUIRewardFlashSaleItem> itemList = new List<GUIRewardFlashSaleItem>();

	private int overTime;

	private float timerRefresh;

	private List<GUIRewardFlashSaleInfo.RewardItem> mRewardItems;

	private GetPetLayer getPetLayer;

	private List<RewardData> datasList;

	private int time;

	public static bool IsVisible
	{
		get
		{
			return (ulong)Globals.Instance.Player.Data.Level >= (ulong)((long)GameConst.GetInt32(20)) && GUIRewardFlashSaleInfo.Status;
		}
	}

	public static bool Status
	{
		get
		{
			return (Globals.Instance.Player.Data.DataFlag & 1024) != 0;
		}
	}

	public static bool IsOpen
	{
		get
		{
			return false;
		}
	}

	public void InitWithBaseScene(GUIRewardCheckBtn btn)
	{
		this.mCheckBtn = btn;
		this.CreateObjects();
	}

	protected void CreateObjects()
	{
		this.mRemainingTime = GameUITools.FindUILabel("RemainingTime", base.gameObject);
		GameObject gameObject = GameUITools.FindGameObject("Items", base.gameObject);
		foreach (Transform transform in gameObject.transform)
		{
			this.itemList.Add(transform.gameObject.AddComponent<GUIRewardFlashSaleItem>());
		}
		if (this.itemList.Count != 3)
		{
			global::Debug.LogError(new object[]
			{
				string.Format("flashsale chest's num error", new object[0])
			});
			return;
		}
	}

	public void OnMsgGetFlashSaleData(MemoryStream stream)
	{
		MS2C_GetFlashSaleData mS2C_GetFlashSaleData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetFlashSaleData), stream) as MS2C_GetFlashSaleData;
		if (mS2C_GetFlashSaleData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_GetFlashSaleData.Result);
			return;
		}
		if (mS2C_GetFlashSaleData.Detail == null || mS2C_GetFlashSaleData.Detail.Count != 3)
		{
			global::Debug.LogError(new object[]
			{
				"flashsale Detail is null"
			});
			return;
		}
		if (mS2C_GetFlashSaleData.Name == null || mS2C_GetFlashSaleData.Name.Count != 3)
		{
			return;
		}
		if (mS2C_GetFlashSaleData.PreCost == null || mS2C_GetFlashSaleData.PreCost.Count != 3)
		{
			return;
		}
		if (mS2C_GetFlashSaleData.CurCost == null || mS2C_GetFlashSaleData.CurCost.Count != 3)
		{
			return;
		}
		if (mS2C_GetFlashSaleData.Count == null || mS2C_GetFlashSaleData.Count.Count != 3)
		{
			return;
		}
		this.overTime = mS2C_GetFlashSaleData.OverTime;
		this.itemList[0].InitData(this, 0, mS2C_GetFlashSaleData.Name[0], mS2C_GetFlashSaleData.Detail[0], mS2C_GetFlashSaleData.PreCost[0], mS2C_GetFlashSaleData.CurCost[0], mS2C_GetFlashSaleData.Count[0]);
		this.itemList[1].InitData(this, 1, mS2C_GetFlashSaleData.Name[1], mS2C_GetFlashSaleData.Detail[1], mS2C_GetFlashSaleData.PreCost[1], mS2C_GetFlashSaleData.CurCost[1], mS2C_GetFlashSaleData.Count[1]);
		this.itemList[2].InitData(this, 2, mS2C_GetFlashSaleData.Name[2], mS2C_GetFlashSaleData.Detail[2], mS2C_GetFlashSaleData.PreCost[2], mS2C_GetFlashSaleData.CurCost[2], mS2C_GetFlashSaleData.Count[2]);
	}

	public void OnMsgStartFlashSale(MemoryStream stream)
	{
		MS2C_StartFlashSale mS2C_StartFlashSale = Serializer.NonGeneric.Deserialize(typeof(MS2C_StartFlashSale), stream) as MS2C_StartFlashSale;
		if (mS2C_StartFlashSale.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("ActivityR", mS2C_StartFlashSale.Result);
			return;
		}
		if (mS2C_StartFlashSale.Data == null || mS2C_StartFlashSale.Data.Count == 0)
		{
			global::Debug.LogError(new object[]
			{
				"Reward is null , excel error"
			});
			return;
		}
		base.StartCoroutine(this.ShowRewards(mS2C_StartFlashSale));
		this.itemList[mS2C_StartFlashSale.Slot].Refresh(mS2C_StartFlashSale.Count);
	}

	public void FeatureCardClick()
	{
		this.getPetLayer = null;
	}

	[DebuggerHidden]
	private IEnumerator ShowRewards(MS2C_StartFlashSale reply)
	{
        return null;
        //GUIRewardFlashSaleInfo.<ShowRewards>c__Iterator6E <ShowRewards>c__Iterator6E = new GUIRewardFlashSaleInfo.<ShowRewards>c__Iterator6E();
        //<ShowRewards>c__Iterator6E.reply = reply;
        //<ShowRewards>c__Iterator6E.<$>reply = reply;
        //<ShowRewards>c__Iterator6E.<>f__this = this;
        //return <ShowRewards>c__Iterator6E;
	}

	public void SendStartFlashSale2Server(int slot)
	{
		MC2S_StartFlashSale mC2S_StartFlashSale = new MC2S_StartFlashSale();
		mC2S_StartFlashSale.Slot = slot;
		Globals.Instance.CliSession.Send(714, mC2S_StartFlashSale);
	}

	public static bool CanTakePartIn()
	{
		return GUIRewardFlashSaleInfo.IsVisible && (Globals.Instance.Player.Data.RedFlag & 16) != 0;
	}

	public void Refresh()
	{
		this.mCheckBtn.IsShowMark = GUIRewardFlashSaleInfo.CanTakePartIn();
		if (GUIRewardFlashSaleInfo.IsVisible)
		{
			MC2S_GetFlashSaleData ojb = new MC2S_GetFlashSaleData();
			Globals.Instance.CliSession.Send(712, ojb);
		}
	}

	private void Update()
	{
		if (base.gameObject.activeInHierarchy && this.mRemainingTime != null && this.overTime > 0 && Time.time - this.timerRefresh > 1f)
		{
			this.timerRefresh = Time.time;
			this.RefreshRemainingTime();
		}
	}

	private void RefreshRemainingTime()
	{
		this.mRemainingTime.text = Singleton<StringManager>.Instance.GetString("activityLuckyDrawRemainingTime", new object[]
		{
			Tools.FormatTimeStr2(this.GetRemainingTime(), false, false)
		});
	}

	private int GetRemainingTime()
	{
		this.time = this.overTime - Globals.Instance.Player.GetTimeStamp();
		if (this.time >= 0)
		{
			return this.time;
		}
		return 0;
	}
}
