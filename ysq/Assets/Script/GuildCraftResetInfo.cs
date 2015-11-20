using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GuildCraftResetInfo : MonoBehaviour
{
	private GameObject mState0;

	private UILabel mCDTime;

	private UILabel mCostNum;

	private GameObject mState1;

	private UILabel mAttAdd;

	private UILabel mLeftNum;

	private GameObject mZhuLiBtn;

	private float mRefreshTimer;

	private StringBuilder mSb = new StringBuilder(10);

	public void InitWithBaseScene()
	{
		this.CreateObjects();
		this.Refresh();
		this.mRefreshTimer = Time.time;
	}

	private void CreateObjects()
	{
		this.mState0 = base.transform.Find("state0").gameObject;
		this.mCDTime = this.mState0.transform.Find("cdTime").GetComponent<UILabel>();
		this.mCostNum = this.mState0.transform.Find("priceBg/price").GetComponent<UILabel>();
		GameObject gameObject = this.mState0.transform.Find("resetBtn").gameObject;
		UIEventListener expr_7C = UIEventListener.Get(gameObject);
		expr_7C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_7C.onClick, new UIEventListener.VoidDelegate(this.OnResetBtnClick));
		this.mState1 = base.transform.Find("state1").gameObject;
		this.mAttAdd = this.mState1.transform.Find("txt0/num").GetComponent<UILabel>();
		this.mLeftNum = this.mState1.transform.Find("txt1/num").GetComponent<UILabel>();
		GameObject gameObject2 = this.mState1.transform.Find("useBtn").gameObject;
		UIEventListener expr_119 = UIEventListener.Get(gameObject2);
		expr_119.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_119.onClick, new UIEventListener.VoidDelegate(this.OnUseBtnClick));
		this.mZhuLiBtn = this.mState1.transform.Find("leaveBtn").gameObject;
		UIEventListener expr_165 = UIEventListener.Get(this.mZhuLiBtn);
		expr_165.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_165.onClick, new UIEventListener.VoidDelegate(this.OnZhuLiBtnClick));
	}

	public void Refresh()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
			{
				return;
			}
			GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
			if (localClientMember == null)
			{
				return;
			}
			if (localClientMember.Member == null)
			{
				return;
			}
			if (localClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Dead)
			{
				this.mState0.SetActive(true);
				this.mState1.SetActive(false);
				int num = localClientMember.Member.Para1 - Globals.Instance.Player.GetTimeStamp();
				if (num <= 0)
				{
					num = 0;
				}
				this.mCDTime.text = Tools.FormatTime(num);
				int b = 1;
				foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
				{
					if (current.GuildWarReviveCost == 0)
					{
						break;
					}
					b = current.ID;
				}
				int id = Mathf.Min((int)(localClientMember.Member.KilledNum + 1u), b);
				MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(id);
				int num2 = (info == null) ? 100 : info.GuildWarReviveCost;
				this.mCostNum.text = num2.ToString();
				this.mCostNum.color = ((Globals.Instance.Player.Data.Diamond >= num2) ? Color.green : Color.red);
			}
			else if (localClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Empty || localClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Guard)
			{
				this.mState0.SetActive(false);
				this.mState1.SetActive(true);
				this.mAttAdd.text = this.mSb.Remove(0, this.mSb.Length).Append(Mathf.RoundToInt((float)(GameConst.GetInt32(175) / 100))).Append("%").ToString();
				this.mLeftNum.text = localClientMember.Member.RecoverTimes.ToString();
				if (localClientMember.Member.RecoverTimes <= 0)
				{
					this.mLeftNum.color = Color.red;
				}
				else
				{
					this.mLeftNum.color = Color.green;
				}
				this.mZhuLiBtn.SetActive(Globals.Instance.Player.GuildSystem.IsGuarding());
			}
		}
	}

	private void OnResetBtnClick(GameObject go)
	{
		GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
		if (localClientMember == null)
		{
			return;
		}
		if (localClientMember.Member == null)
		{
			return;
		}
		int b = 1;
		foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
		{
			if (current.GuildWarReviveCost == 0)
			{
				break;
			}
			b = current.ID;
		}
		int id = Mathf.Min((int)(localClientMember.Member.KilledNum + 1u), b);
		MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(id);
		if (info != null)
		{
			if (Tools.MoneyNotEnough(ECurrencyType.ECurrencyT_Diamond, info.GuildWarReviveCost, 0))
			{
				return;
			}
			Globals.Instance.Player.GuildSystem.RequestGuildWarFuHuo();
		}
	}

	private void OnUseBtnClick(GameObject go)
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
			{
				return;
			}
			GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
			if (mGWEnterData == null)
			{
				return;
			}
			GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
			if (localClientMember == null)
			{
				return;
			}
			if (localClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Empty || localClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Guard)
			{
				if (localClientMember.Member.RecoverTimes <= 0)
				{
					GameUIManager.mInstance.ShowMessageTip("EGR", 114);
					return;
				}
				if (localClientMember.Member.HealthPct == 10000)
				{
					GameUIManager.mInstance.ShowMessageTip("EGR", 118);
					return;
				}
				MC2S_GuildWarRecoverHP mC2S_GuildWarRecoverHP = new MC2S_GuildWarRecoverHP();
				mC2S_GuildWarRecoverHP.WarID = mGWEnterData.WarID;
				mC2S_GuildWarRecoverHP.TeamID = selfTeamFlag;
				Globals.Instance.CliSession.Send(1005, mC2S_GuildWarRecoverHP);
			}
		}
	}

	private void OnZhuLiSureClick(object go)
	{
		int guardIndex = Globals.Instance.Player.GuildSystem.GetGuardIndex();
		if (guardIndex <= 0)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.LocalClientMember == null)
		{
			return;
		}
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
		{
			return;
		}
		MC2S_GuildWarQuitHold mC2S_GuildWarQuitHold = new MC2S_GuildWarQuitHold();
		mC2S_GuildWarQuitHold.WarID = mGWEnterData.WarID;
		mC2S_GuildWarQuitHold.TeamID = selfTeamFlag;
		mC2S_GuildWarQuitHold.StrongholdID = Globals.Instance.Player.GuildSystem.LocalClientMember.Member.StrongholdId;
		mC2S_GuildWarQuitHold.SlotIndex = guardIndex;
		Globals.Instance.CliSession.Send(1003, mC2S_GuildWarQuitHold);
	}

	private void OnChongZhiSureClick(object go)
	{
		GameMessageBox.ShowRechargeMessageBox();
	}

	private void OnZhuLiBtnClick(GameObject go)
	{
		int @int = GameConst.GetInt32(176);
		if (Globals.Instance.Player.Data.Diamond < @int)
		{
			string @string = Singleton<StringManager>.Instance.GetString("guildCraft68", new object[]
			{
				@int
			});
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OKCancel, null);
			GameMessageBox expr_4E = gameMessageBox;
			expr_4E.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_4E.OkClick, new MessageBox.MessageDelegate(this.OnChongZhiSureClick));
		}
		else
		{
			string string2 = Singleton<StringManager>.Instance.GetString("guildCraft63", new object[]
			{
				"[00ff00]",
				@int
			});
			GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(string2, MessageBox.Type.OKCancel, null);
			GameMessageBox expr_A7 = gameMessageBox2;
			expr_A7.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_A7.OkClick, new MessageBox.MessageDelegate(this.OnZhuLiSureClick));
		}
	}

	private void Update()
	{
		if (this.mState0.activeInHierarchy && Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			this.Refresh();
		}
	}
}
