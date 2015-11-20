using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class CraftHoldInfoMemberItem : MonoBehaviour
{
	private const int mPetNum = 3;

	private int mIndex;

	private GameObject mState0;

	private UISprite mRankIcon;

	private UISprite mRankIconFrame;

	private UISlider mRankHp;

	private CraftHoldInfoPetIcon[] mPets = new CraftHoldInfoPetIcon[3];

	private UILabel mLvName;

	private UILabel mBattleNum;

	private GameObject mBattleBtn;

	private UILabel mBattleBtnLb;

	private GameObject mBattleMark;

	private UILabel mStateTip;

	private GameObject mState1;

	private StringBuilder mSb = new StringBuilder(42);

	private void CreateObjects()
	{
		this.mState0 = base.transform.Find("stat0").gameObject;
		this.mRankIcon = this.mState0.transform.Find("rankIcon").GetComponent<UISprite>();
		this.mRankIconFrame = this.mRankIcon.transform.Find("Frame").GetComponent<UISprite>();
		this.mRankHp = this.mRankIcon.transform.Find("hp").GetComponent<UISlider>();
		this.mPets[0] = this.mRankIcon.transform.Find("Pet0").gameObject.AddComponent<CraftHoldInfoPetIcon>();
		this.mPets[1] = this.mRankIcon.transform.Find("Pet1").gameObject.AddComponent<CraftHoldInfoPetIcon>();
		this.mPets[2] = this.mRankIcon.transform.Find("Pet2").gameObject.AddComponent<CraftHoldInfoPetIcon>();
		this.mPets[0].Init();
		this.mPets[1].Init();
		this.mPets[2].Init();
		this.mLvName = this.mState0.transform.Find("LvName").GetComponent<UILabel>();
		this.mBattleNum = this.mState0.transform.Find("score").GetComponent<UILabel>();
		this.mBattleMark = this.mState0.transform.Find("battleMark").gameObject;
		this.mBattleMark.SetActive(false);
		this.mStateTip = this.mState0.transform.Find("stateTip").GetComponent<UILabel>();
		this.mStateTip.gameObject.SetActive(false);
		this.mBattleBtn = this.mState0.transform.Find("battleBtn").gameObject;
		UIEventListener expr_1DF = UIEventListener.Get(this.mBattleBtn);
		expr_1DF.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1DF.onClick, new UIEventListener.VoidDelegate(this.OnBattleBtnClick));
		this.mBattleBtnLb = this.mBattleBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mBattleBtn.SetActive(false);
		this.mState1 = base.transform.Find("stat1").gameObject;
		GameObject gameObject = this.mState1.transform.Find("zhanLingBtn").gameObject;
		UIEventListener expr_268 = UIEventListener.Get(gameObject);
		expr_268.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_268.onClick, new UIEventListener.VoidDelegate(this.OnZhanLingBtnClick));
	}

	public void InitWithBaseScene(int index)
	{
		this.mIndex = index;
		this.CreateObjects();
	}

	private void HideMemberIconInSlot()
	{
		this.mRankIcon.gameObject.SetActive(false);
		this.mLvName.gameObject.SetActive(false);
		this.mBattleNum.gameObject.SetActive(false);
	}

	private void ShowMemberIconInSlot(GuildWarClientTeamMember clientMember)
	{
		if (clientMember != null)
		{
			this.mState0.SetActive(true);
			this.mState1.SetActive(false);
			this.mRankIcon.gameObject.SetActive(true);
			this.mLvName.gameObject.SetActive(true);
			this.mBattleNum.gameObject.SetActive(true);
			this.mRankIcon.spriteName = Tools.GetPlayerIcon(clientMember.Data.FashionID);
			this.mRankIconFrame.spriteName = Tools.GetItemQualityIcon(LocalPlayer.GetQuality(clientMember.Data.ConstellationLevel));
			this.mRankHp.value = Mathf.Clamp01((float)clientMember.Member.HealthPct / 10000f);
			int i;
			for (i = 0; i < clientMember.Data.PetInfoID.Count; i++)
			{
				this.mPets[i].Refresh(clientMember.Data.PetInfoID[i], (float)clientMember.Member.HealthPct);
			}
			while (i < 3)
			{
				this.mPets[i].Refresh(0, 0f);
				i++;
			}
			this.mLvName.text = this.mSb.Remove(0, this.mSb.Length).Append("Lv").Append(clientMember.Data.Level).Append(" ").Append(clientMember.Data.Name).ToString();
			this.mBattleNum.text = this.mSb.Remove(0, this.mSb.Length).Append(Singleton<StringManager>.Instance.GetString("BillboardFighting")).Append(":").Append(clientMember.Data.CombatValue).ToString();
		}
	}

	public void RefreshBySlot()
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
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Neutrality)
		{
			this.mState0.SetActive(false);
			if (flag)
			{
				this.mState1.SetActive(false);
			}
			else
			{
				this.mState1.SetActive(true);
			}
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Own)
		{
			GuildWarStrongholdSlot guildWarStrongholdSlot = strongHold.Slots[this.mIndex - 1];
			if (guildWarStrongholdSlot != null)
			{
				if (guildWarStrongholdSlot.Status == EGuardWarStrongholdSlotState.EGWPSS_War)
				{
					GuildWarClientTeamMember strongHoldMember = Globals.Instance.Player.GuildSystem.GetStrongHoldMember(guildWarStrongholdSlot.PlayerID);
					if (strongHoldMember != null && strongHoldMember.Member.Status != EGuardWarTeamMemState.EGWTMS_Empty)
					{
						this.ShowMemberIconInSlot(strongHoldMember);
						this.mBattleMark.SetActive(true);
						this.mBattleBtn.SetActive(false);
						this.mStateTip.gameObject.SetActive(true);
						this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guildCraft34");
					}
				}
				else if (guildWarStrongholdSlot.Status == EGuardWarStrongholdSlotState.EGWPSS_Guard)
				{
					GuildWarClientTeamMember strongHoldMember2 = Globals.Instance.Player.GuildSystem.GetStrongHoldMember(guildWarStrongholdSlot.PlayerID);
					if (strongHoldMember2 != null && strongHoldMember2.Member.Status != EGuardWarTeamMemState.EGWTMS_Empty)
					{
						this.ShowMemberIconInSlot(strongHoldMember2);
						this.mBattleMark.SetActive(false);
						if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
						{
							if (strongHoldMember2.Data.GUID == Globals.Instance.Player.Data.ID)
							{
								this.mBattleBtn.SetActive(true);
								this.mBattleBtnLb.text = Singleton<StringManager>.Instance.GetString("guildCraft36");
							}
							else
							{
								int selfGuildJob = Tools.GetSelfGuildJob();
								if (selfGuildJob == 1 || selfGuildJob == 2)
								{
									this.mBattleBtn.SetActive(true);
									this.mBattleBtnLb.text = Singleton<StringManager>.Instance.GetString("guildCraft72");
								}
								else
								{
									this.mBattleBtn.SetActive(false);
								}
							}
						}
						else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
						{
							if (strongHoldMember2.Data.GUID == Globals.Instance.Player.Data.ID)
							{
								this.mBattleBtn.SetActive(true);
								this.mBattleBtnLb.text = Singleton<StringManager>.Instance.GetString("guildCraft36");
							}
							else
							{
								EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
								if (!flag && selfTeamFlag != strongHold.OwnerId)
								{
									this.mBattleBtn.SetActive(true);
									this.mBattleBtnLb.text = Singleton<StringManager>.Instance.GetString("guildCraft23");
								}
								else
								{
									this.mBattleBtn.SetActive(false);
								}
							}
						}
						this.mStateTip.gameObject.SetActive(true);
						this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guildCraft35");
					}
				}
				else if (guildWarStrongholdSlot.Status == EGuardWarStrongholdSlotState.EGWPSS_Empty)
				{
					this.mState0.SetActive(true);
					this.mState1.SetActive(false);
					this.HideMemberIconInSlot();
					if (strongHold.OwnerId == Globals.Instance.Player.GuildSystem.GetSelfTeamFlag())
					{
						this.mBattleMark.SetActive(false);
						this.mBattleBtn.SetActive(true);
						this.mBattleBtnLb.text = Singleton<StringManager>.Instance.GetString("guildCraft33");
						this.mStateTip.gameObject.SetActive(true);
						this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guildCraft32");
					}
					else
					{
						this.mBattleMark.SetActive(false);
						this.mBattleBtn.SetActive(false);
						this.mStateTip.gameObject.SetActive(true);
						this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guildCraft32");
					}
				}
			}
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Protected)
		{
			this.mState0.SetActive(false);
			if (flag)
			{
				this.mState1.SetActive(false);
			}
			else
			{
				this.mState1.SetActive(true);
			}
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_OwnerChanging)
		{
			GuildWarStrongholdSlot guildWarStrongholdSlot2 = strongHold.Slots[this.mIndex - 1];
			if (guildWarStrongholdSlot2 != null)
			{
				GuildWarClientTeamMember strongHoldMember3 = Globals.Instance.Player.GuildSystem.GetStrongHoldMember(guildWarStrongholdSlot2.PlayerID);
				if (strongHoldMember3 != null && strongHoldMember3.Member.Status != EGuardWarTeamMemState.EGWTMS_Empty)
				{
					this.ShowMemberIconInSlot(strongHoldMember3);
					this.mBattleMark.SetActive(false);
					this.mStateTip.gameObject.SetActive(true);
					this.mStateTip.text = Singleton<StringManager>.Instance.GetString("guildCraft26");
					this.mBattleBtn.SetActive(false);
				}
				else if (strongHold.OwnerId == Globals.Instance.Player.GuildSystem.GetSelfTeamFlag())
				{
					this.mState0.SetActive(false);
					this.mState1.SetActive(true);
				}
				else
				{
					this.mState0.SetActive(false);
					this.mState1.SetActive(false);
				}
			}
		}
	}

	private void OnFuHuoBtnClick(object obj)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		Globals.Instance.Player.GuildSystem.RequestGuildWarFuHuo();
	}

	private void OnChongZhiSureClick(object go)
	{
		GameMessageBox.ShowRechargeMessageBox();
	}

	private void OnBattleBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Own)
		{
			EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
			GuildWarStrongholdSlot guildWarStrongholdSlot = strongHold.Slots[this.mIndex - 1];
			if (guildWarStrongholdSlot == null)
			{
				return;
			}
			if (guildWarStrongholdSlot.Status == EGuardWarStrongholdSlotState.EGWPSS_Guard)
			{
				GuildWarClientTeamMember strongHoldMember = Globals.Instance.Player.GuildSystem.GetStrongHoldMember(guildWarStrongholdSlot.PlayerID);
				if (strongHoldMember != null && strongHoldMember.Member.Status != EGuardWarTeamMemState.EGWTMS_Empty)
				{
					if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
					{
						if (strongHoldMember.Data.GUID == Globals.Instance.Player.Data.ID)
						{
							GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft41"), MessageBox.Type.OKCancel, null);
							GameMessageBox expr_121 = gameMessageBox;
							expr_121.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_121.OkClick, new MessageBox.MessageDelegate(this.OnLiKaiBtnClick));
						}
						else
						{
							int selfGuildJob = Tools.GetSelfGuildJob();
							if (selfGuildJob == 1 || selfGuildJob == 2)
							{
								GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft73"), MessageBox.Type.OKCancel, null);
								GameMessageBox expr_178 = gameMessageBox2;
								expr_178.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_178.OkClick, new MessageBox.MessageDelegate(this.OnQingLiBtnClick));
							}
						}
					}
					else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
					{
						if (strongHoldMember.Data.GUID != Globals.Instance.Player.Data.ID)
						{
							GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
							if (mGWEnterData == null)
							{
								return;
							}
							if (Globals.Instance.Player.GuildSystem.LocalClientMember != null && Globals.Instance.Player.GuildSystem.LocalClientMember.Member != null && Globals.Instance.Player.GuildSystem.LocalClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Dead)
							{
								int b = 1;
								foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
								{
									if (current.GuildWarReviveCost == 0)
									{
										break;
									}
									b = current.ID;
								}
								int id = Mathf.Min((int)(Globals.Instance.Player.GuildSystem.LocalClientMember.Member.KilledNum + 1u), b);
								MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(id);
								int num = (info == null) ? 100 : info.GuildWarReviveCost;
								string text = (Globals.Instance.Player.Data.Diamond >= num) ? "[00ff00]" : "[ff0000]";
								string @string = Singleton<StringManager>.Instance.GetString("guildCraft66", new object[]
								{
									text,
									(info == null) ? 100 : info.GuildWarReviveCost
								});
								GameMessageBox gameMessageBox3 = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OKCancel, null);
								GameMessageBox expr_382 = gameMessageBox3;
								expr_382.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_382.OkClick, new MessageBox.MessageDelegate(this.OnFuHuoBtnClick));
							}
							else
							{
								MC2S_GuildWarFightBegin mC2S_GuildWarFightBegin = new MC2S_GuildWarFightBegin();
								mC2S_GuildWarFightBegin.WarID = mGWEnterData.WarID;
								mC2S_GuildWarFightBegin.TeamID = selfTeamFlag;
								mC2S_GuildWarFightBegin.StrongholdID = strongHold.ID;
								mC2S_GuildWarFightBegin.SlotIndex = this.mIndex;
								GameUIManager.mInstance.uiState.GuildWarFightSlotIndex = this.mIndex;
								GameUIManager.mInstance.uiState.GuildWarFightHoldIndex = strongHold.ID;
								Globals.Instance.CliSession.Send(983, mC2S_GuildWarFightBegin);
							}
						}
						else
						{
							int @int = GameConst.GetInt32(176);
							if (Globals.Instance.Player.Data.Diamond < @int)
							{
								string string2 = Singleton<StringManager>.Instance.GetString("guildCraft68", new object[]
								{
									@int
								});
								GameMessageBox gameMessageBox4 = GameMessageBox.ShowMessageBox(string2, MessageBox.Type.OKCancel, null);
								GameMessageBox expr_47A = gameMessageBox4;
								expr_47A.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_47A.OkClick, new MessageBox.MessageDelegate(this.OnChongZhiSureClick));
							}
							else
							{
								string text2 = "[00ff00]";
								if (Globals.Instance.Player.Data.Diamond < GameConst.GetInt32(176))
								{
									text2 = "[ff0000]";
								}
								string string3 = Singleton<StringManager>.Instance.GetString("guildCraft63", new object[]
								{
									text2,
									GameConst.GetInt32(176)
								});
								GameMessageBox gameMessageBox5 = GameMessageBox.ShowMessageBox(string3, MessageBox.Type.OKCancel, null);
								GameMessageBox expr_50C = gameMessageBox5;
								expr_50C.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_50C.OkClick, new MessageBox.MessageDelegate(this.OnZhuLiBtnClick));
							}
						}
					}
				}
			}
			else if (guildWarStrongholdSlot.Status == EGuardWarStrongholdSlotState.EGWPSS_Empty && strongHold.OwnerId == Globals.Instance.Player.GuildSystem.GetSelfTeamFlag())
			{
				if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare) && Globals.Instance.Player.GuildSystem.IsGuarding())
				{
					GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
					if (localClientMember != null && localClientMember.Member != null)
					{
						if (localClientMember.Member.StrongholdId == strongHold.ID)
						{
							this.OnSureZhanLingClick(null);
						}
						else
						{
							GameMessageBox gameMessageBox6 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft65"), MessageBox.Type.OKCancel, null);
							GameMessageBox expr_5F4 = gameMessageBox6;
							expr_5F4.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_5F4.OkClick, new MessageBox.MessageDelegate(this.OnSureZhanLingClick));
						}
					}
				}
				else
				{
					GameMessageBox gameMessageBox7 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft39"), MessageBox.Type.OKCancel, null);
					gameMessageBox7.TextOK = Singleton<StringManager>.Instance.GetString("guildCraft37");
					GameMessageBox expr_64A = gameMessageBox7;
					expr_64A.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_64A.OkClick, new MessageBox.MessageDelegate(this.OnSureZhanLingClick));
				}
			}
		}
	}

	private void OnSureZhanLingClick(object obj)
	{
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
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
		MC2S_GuildWarHold mC2S_GuildWarHold = new MC2S_GuildWarHold();
		mC2S_GuildWarHold.WarID = mGWEnterData.WarID;
		mC2S_GuildWarHold.TeamID = selfTeamFlag;
		mC2S_GuildWarHold.StrongholdID = strongHold.ID;
		mC2S_GuildWarHold.SlotIndex = this.mIndex;
		Globals.Instance.CliSession.Send(989, mC2S_GuildWarHold);
	}

	private void DoLeaveHold()
	{
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
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
		mC2S_GuildWarQuitHold.StrongholdID = strongHold.ID;
		mC2S_GuildWarQuitHold.SlotIndex = this.mIndex;
		Globals.Instance.CliSession.Send(1003, mC2S_GuildWarQuitHold);
	}

	private void OnQingLiBtnClick(object obj)
	{
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
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
		MC2S_GuildWarKickHold mC2S_GuildWarKickHold = new MC2S_GuildWarKickHold();
		mC2S_GuildWarKickHold.WarID = mGWEnterData.WarID;
		mC2S_GuildWarKickHold.TeamID = selfTeamFlag;
		mC2S_GuildWarKickHold.StrongholdID = strongHold.ID;
		mC2S_GuildWarKickHold.SlotIndex = this.mIndex;
		Globals.Instance.CliSession.Send(1015, mC2S_GuildWarKickHold);
	}

	private void OnLiKaiBtnClick(object obj)
	{
		this.DoLeaveHold();
	}

	private void OnZhuLiBtnClick(object go)
	{
		this.DoLeaveHold();
	}

	private void IsDeadZhanLing()
	{
		if (Globals.Instance.Player.GuildSystem.LocalClientMember != null && Globals.Instance.Player.GuildSystem.LocalClientMember.Member != null && Globals.Instance.Player.GuildSystem.LocalClientMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Dead)
		{
			int b = 1;
			foreach (MiscInfo current in Globals.Instance.AttDB.MiscDict.Values)
			{
				if (current.GuildWarReviveCost == 0)
				{
					break;
				}
				b = current.ID;
			}
			int id = Mathf.Min((int)(Globals.Instance.Player.GuildSystem.LocalClientMember.Member.KilledNum + 1u), b);
			MiscInfo info = Globals.Instance.AttDB.MiscDict.GetInfo(id);
			int num = (info == null) ? 100 : info.GuildWarReviveCost;
			string text = (Globals.Instance.Player.Data.Diamond >= num) ? "[00ff00]" : "[ff0000]";
			string @string = Singleton<StringManager>.Instance.GetString("guildCraft66", new object[]
			{
				text,
				(info == null) ? 100 : info.GuildWarReviveCost
			});
			GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(@string, MessageBox.Type.OKCancel, null);
			GameMessageBox expr_17C = gameMessageBox;
			expr_17C.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_17C.OkClick, new MessageBox.MessageDelegate(this.OnFuHuoBtnClick));
		}
		else
		{
			GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft39"), MessageBox.Type.OKCancel, null);
			gameMessageBox2.TextOK = Singleton<StringManager>.Instance.GetString("guildCraft37");
			GameMessageBox expr_1D2 = gameMessageBox2;
			expr_1D2.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_1D2.OkClick, new MessageBox.MessageDelegate(this.OnSureZhanLingClick));
		}
	}

	private void OnZhanLingBtnClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold == null)
		{
			return;
		}
		if (Globals.Instance.Player.GuildSystem.mGWEnterData == null)
		{
			return;
		}
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		if (selfTeamFlag == EGuildWarTeamId.EGWTI_None)
		{
			return;
		}
		if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Neutrality)
		{
			if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare) && Globals.Instance.Player.GuildSystem.IsGuarding())
			{
				GameMessageBox gameMessageBox = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft65"), MessageBox.Type.OKCancel, null);
				GameMessageBox expr_DD = gameMessageBox;
				expr_DD.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_DD.OkClick, new MessageBox.MessageDelegate(this.OnSureZhanLingClick));
			}
			else
			{
				this.IsDeadZhanLing();
			}
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Protected)
		{
			if (Globals.Instance.Player.GuildSystem.StrongHold.OwnerId == selfTeamFlag)
			{
				if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare) && Globals.Instance.Player.GuildSystem.IsGuarding())
				{
					GameMessageBox gameMessageBox2 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft65"), MessageBox.Type.OKCancel, null);
					GameMessageBox expr_184 = gameMessageBox2;
					expr_184.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_184.OkClick, new MessageBox.MessageDelegate(this.OnSureZhanLingClick));
				}
				else
				{
					this.IsDeadZhanLing();
				}
			}
			else
			{
				GameUIManager.mInstance.ShowMessageTipByKey("guildCraft38", 0f, 0f);
			}
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_Own)
		{
			if (Globals.Instance.Player.GuildSystem.StrongHold.OwnerId == selfTeamFlag)
			{
				if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare) && Globals.Instance.Player.GuildSystem.IsGuarding())
				{
					GuildWarClientTeamMember localClientMember = Globals.Instance.Player.GuildSystem.LocalClientMember;
					if (localClientMember != null && localClientMember.Member != null)
					{
						if (localClientMember.Member.StrongholdId == strongHold.ID)
						{
							this.OnSureZhanLingClick(null);
						}
						else
						{
							GameMessageBox gameMessageBox3 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft65"), MessageBox.Type.OKCancel, null);
							GameMessageBox expr_295 = gameMessageBox3;
							expr_295.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_295.OkClick, new MessageBox.MessageDelegate(this.OnSureZhanLingClick));
						}
					}
				}
				else
				{
					this.IsDeadZhanLing();
				}
			}
		}
		else if (strongHold.Status == EGuildWarStrongholdState.EGWPS_OwnerChanging)
		{
			GuildWarStrongholdSlot guildWarStrongholdSlot = strongHold.Slots[this.mIndex - 1];
			if (guildWarStrongholdSlot != null)
			{
				GuildWarClientTeamMember strongHoldMember = Globals.Instance.Player.GuildSystem.GetStrongHoldMember(guildWarStrongholdSlot.PlayerID);
				if (strongHoldMember == null || strongHoldMember.Member.Status == EGuardWarTeamMemState.EGWTMS_Empty)
				{
					if (strongHold.OwnerId == Globals.Instance.Player.GuildSystem.GetSelfTeamFlag())
					{
						if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare) && Globals.Instance.Player.GuildSystem.IsGuarding())
						{
							GameMessageBox gameMessageBox4 = GameMessageBox.ShowMessageBox(Singleton<StringManager>.Instance.GetString("guildCraft65"), MessageBox.Type.OKCancel, null);
							GameMessageBox expr_38E = gameMessageBox4;
							expr_38E.OkClick = (MessageBox.MessageDelegate)Delegate.Combine(expr_38E.OkClick, new MessageBox.MessageDelegate(this.OnSureZhanLingClick));
						}
						else
						{
							this.IsDeadZhanLing();
						}
					}
					else
					{
						GameUIManager.mInstance.ShowMessageTipByKey("guildCraft40", 0f, 0f);
					}
				}
			}
		}
	}
}
