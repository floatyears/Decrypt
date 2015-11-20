using Att;
using Proto;
using ProtoBuf;
using System;
using System.IO;

public class MagicLoveSubSystem : ISubSystem
{
	public enum ELastResult
	{
		ELR_Null,
		ELR_Win,
		ELR_Draw,
		ELR_Lose
	}

	public delegate void VoidCallback();

	public MagicLoveSubSystem.VoidCallback GetMagicLoveDataEvent;

	public uint Version
	{
		get;
		private set;
	}

	public MagicLoveData Data
	{
		get;
		private set;
	}

	public int MaxLoveValue
	{
		get
		{
			return MagicLoveTable.MaxLoveValue;
		}
	}

	public int MaxLoveValueRewardID
	{
		get
		{
			return MagicLoveTable.MaxLoveValueRewardID;
		}
	}

	public MagicLoveSubSystem.ELastResult LastResult
	{
		get;
		private set;
	}

	public void Init()
	{
		Globals.Instance.CliSession.Register(1100, new ClientSession.MsgHandler(this.OnMsgUpdateMagicLoveData));
		Globals.Instance.CliSession.Register(1102, new ClientSession.MsgHandler(this.OnMsgGetMagicLoveData));
	}

	private void OnMsgUpdateMagicLoveData(MemoryStream stream)
	{
		MS2C_UpdateMagicLoveData mS2C_UpdateMagicLoveData = Serializer.NonGeneric.Deserialize(typeof(MS2C_UpdateMagicLoveData), stream) as MS2C_UpdateMagicLoveData;
		if (this.Data == null)
		{
			return;
		}
		if (mS2C_UpdateMagicLoveData.Index.Count > 0)
		{
			for (int i = 0; i < mS2C_UpdateMagicLoveData.Index.Count; i++)
			{
				if (mS2C_UpdateMagicLoveData.PetID.Count > 0)
				{
					this.Data.PetID[mS2C_UpdateMagicLoveData.Index[i]] = mS2C_UpdateMagicLoveData.PetID[i];
				}
				if (mS2C_UpdateMagicLoveData.LoveValue.Count > 0)
				{
					this.Data.LoveValue[mS2C_UpdateMagicLoveData.Index[i]] = mS2C_UpdateMagicLoveData.LoveValue[i];
				}
				if (mS2C_UpdateMagicLoveData.RewardFlag.Count > 0)
				{
					this.Data.RewardFlag[mS2C_UpdateMagicLoveData.Index[i]] = mS2C_UpdateMagicLoveData.RewardFlag[i];
				}
			}
		}
		if (mS2C_UpdateMagicLoveData.MagicMatchCount != 0)
		{
			if (mS2C_UpdateMagicLoveData.MagicMatchCount == -1)
			{
				this.Data.MagicMatchCount = 0;
			}
			else
			{
				this.Data.MagicMatchCount = mS2C_UpdateMagicLoveData.MagicMatchCount;
			}
		}
		if (mS2C_UpdateMagicLoveData.MagicMatchBuyCount != 0)
		{
			if (mS2C_UpdateMagicLoveData.MagicMatchBuyCount == -1)
			{
				this.Data.MagicMatchBuyCount = 0;
			}
			else
			{
				this.Data.MagicMatchBuyCount = mS2C_UpdateMagicLoveData.MagicMatchBuyCount;
			}
		}
		if (mS2C_UpdateMagicLoveData.LoseCount != 0)
		{
			if (mS2C_UpdateMagicLoveData.LoseCount == -1)
			{
				this.Data.LoseCount = 0;
			}
			else
			{
				this.Data.LoseCount = mS2C_UpdateMagicLoveData.LoseCount;
			}
		}
		if (mS2C_UpdateMagicLoveData.LastSelfMagicType != 0)
		{
			if (mS2C_UpdateMagicLoveData.LastSelfMagicType == -1)
			{
				this.Data.LastSelfMagicType = 0;
			}
			else
			{
				this.Data.LastSelfMagicType = mS2C_UpdateMagicLoveData.LastSelfMagicType;
			}
			this.RefreshResult();
		}
		if (mS2C_UpdateMagicLoveData.LastTargetMagicType != 0)
		{
			if (mS2C_UpdateMagicLoveData.LastTargetMagicType == -1)
			{
				this.Data.LastTargetMagicType = 0;
			}
			else
			{
				this.Data.LastTargetMagicType = mS2C_UpdateMagicLoveData.LastTargetMagicType;
			}
			this.RefreshResult();
		}
		if (mS2C_UpdateMagicLoveData.Bout != 0)
		{
			if (mS2C_UpdateMagicLoveData.Bout == -1)
			{
				this.Data.Bout = 0;
			}
			else
			{
				this.Data.Bout = mS2C_UpdateMagicLoveData.Bout;
			}
		}
		if (mS2C_UpdateMagicLoveData.LastIndex != 0)
		{
			if (mS2C_UpdateMagicLoveData.LastIndex == -1)
			{
				this.Data.LastIndex = 0;
			}
			else
			{
				this.Data.LastIndex = mS2C_UpdateMagicLoveData.LastIndex;
			}
		}
	}

	private void OnMsgGetMagicLoveData(MemoryStream stream)
	{
		MS2C_GetMagicLoveData mS2C_GetMagicLoveData = Serializer.NonGeneric.Deserialize(typeof(MS2C_GetMagicLoveData), stream) as MS2C_GetMagicLoveData;
		if (mS2C_GetMagicLoveData.Result == 13)
		{
			Globals.Instance.Player.ShowFrozenFunctionMsg();
			return;
		}
		if (mS2C_GetMagicLoveData.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EMLR", mS2C_GetMagicLoveData.Result);
			return;
		}
		if (mS2C_GetMagicLoveData.Version != 0u && this.Version != mS2C_GetMagicLoveData.Version)
		{
			this.Version = mS2C_GetMagicLoveData.Version;
			this.Data = mS2C_GetMagicLoveData.Data;
			this.RefreshResult();
		}
		if (this.GetMagicLoveDataEvent != null)
		{
			this.GetMagicLoveDataEvent();
		}
	}

	private void RefreshResult()
	{
		if (this.Data == null || this.Data.LastSelfMagicType == 0 || this.Data.LastTargetMagicType == 0)
		{
			this.LastResult = MagicLoveSubSystem.ELastResult.ELR_Null;
			return;
		}
		int lastSelfMagicType = this.Data.LastSelfMagicType;
		int lastTargetMagicType = this.Data.LastTargetMagicType;
		if (lastSelfMagicType == lastTargetMagicType)
		{
			this.LastResult = MagicLoveSubSystem.ELastResult.ELR_Draw;
		}
		else if (lastSelfMagicType % 3 + 1 == lastTargetMagicType)
		{
			this.LastResult = MagicLoveSubSystem.ELastResult.ELR_Win;
		}
		else
		{
			this.LastResult = MagicLoveSubSystem.ELastResult.ELR_Lose;
		}
	}

	public void SendGetMagicLoveData()
	{
		MC2S_GetMagicLoveData mC2S_GetMagicLoveData = new MC2S_GetMagicLoveData();
		mC2S_GetMagicLoveData.version = this.Version;
		Globals.Instance.CliSession.Send(1101, mC2S_GetMagicLoveData);
	}

	public void SendRefreshPet()
	{
		MC2S_RefreshPet ojb = new MC2S_RefreshPet();
		Globals.Instance.CliSession.Send(1115, ojb);
	}

	public int GetPassCost()
	{
		if (this.Data == null)
		{
			return 0;
		}
		MagicLoveInfo info = Globals.Instance.AttDB.MagicLoveDict.GetInfo(this.Data.LoseCount);
		if (info == null)
		{
			Debug.LogErrorFormat("MagicLoveDict get info error , ID : {0} ", new object[]
			{
				this.Data.LoseCount
			});
			return 0;
		}
		return info.Cost;
	}

	public void GetCurPetLoveValue(int index, out int loveValue, out int rewardID, out bool hasTakenAll)
	{
		loveValue = 0;
		rewardID = 0;
		hasTakenAll = false;
		if (this.Data == null)
		{
			return;
		}
		if (index < 0 || index >= this.Data.PetID.Count)
		{
			return;
		}
		for (int i = 0; i < MagicLoveTable.LoveValueList.Count; i++)
		{
			if (this.Data.LoveValue[index] >= MagicLoveTable.LoveValueList[i] && rewardID == 0 && (this.Data.RewardFlag[index] & 1 << i + 1) == 0)
			{
				rewardID = i + 1;
			}
			if (loveValue == 0 && (this.Data.RewardFlag[index] & 1 << i + 1) == 0)
			{
				loveValue = MagicLoveTable.LoveValueList[i];
			}
		}
		if (loveValue == 0)
		{
			loveValue = MagicLoveTable.LoveValueList[MagicLoveTable.LoveValueList.Count - 1];
		}
		hasTakenAll = (this.Data.LoveValue[index] >= loveValue);
	}

	public bool CanTakeReward(int index)
	{
		for (int i = 0; i < MagicLoveTable.LoveValueList.Count; i++)
		{
			if (this.Data.LoveValue[index] >= MagicLoveTable.LoveValueList[i] && (this.Data.RewardFlag[index] & 1 << i + 1) == 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool LoveValueIsFull(int index)
	{
		return index < this.Data.LoveValue.Count && index >= 0 && this.Data.LoveValue[index] >= this.MaxLoveValue;
	}

	public void Destroy()
	{
		this.Data = null;
		this.Version = 0u;
		this.LastResult = MagicLoveSubSystem.ELastResult.ELR_Null;
	}

	public void Update(float elapse)
	{
	}
}
