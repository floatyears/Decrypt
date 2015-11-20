using Att;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GuildCraftMapItem : MonoBehaviour
{
	private GUIGuildCraftScene mBaseScene;

	private GuildWarClientCity mGuildWarCity;

	private UIButton mTowerIcon;

	private UILabel mTowerName;

	private UILabel mGuildName;

	private UILabel mTakeRewardBtnTxt;

	private GameObject mBattleMark;

	private UIButton mTakeRewardBtn;

	private GameObject mTakeRewardBtnGo;

	private GameObject mTakeRewardBtnEffect;

	private GameObject mTipGo;

	private UILabel mTipTxt;

	private GameObject mGuanMark;

	private GameObject mGuanMarkEffect;

	private int mPosIndex;

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWitBaseScene(GUIGuildCraftScene baseScene, int posIndex)
	{
		this.mBaseScene = baseScene;
		this.mPosIndex = posIndex;
		this.CreateObjects();
	}

	private void CreateObjects()
	{
		this.mTowerIcon = base.transform.Find("icon").GetComponent<UIButton>();
		UIEventListener expr_2B = UIEventListener.Get(this.mTowerIcon.gameObject);
		expr_2B.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_2B.onPress, new UIEventListener.BoolDelegate(this.OnTowerIconClick));
		this.mTowerName = base.transform.Find("bg/name").GetComponent<UILabel>();
		this.mGuildName = base.transform.Find("guildName").GetComponent<UILabel>();
		this.mBattleMark = base.transform.Find("battleMark").gameObject;
		this.mTakeRewardBtnGo = base.transform.Find("takeRewardBtn").gameObject;
		this.mTakeRewardBtn = this.mTakeRewardBtnGo.GetComponent<UIButton>();
		this.mTakeRewardBtnEffect = this.mTakeRewardBtnGo.transform.Find("Effect").gameObject;
		this.mTakeRewardBtnEffect.SetActive(false);
		this.mTipGo = base.transform.Find("tipBg").gameObject;
		this.mTipTxt = this.mTipGo.transform.Find("tip").GetComponent<UILabel>();
		if (this.mPosIndex == 0)
		{
			Transform transform = base.transform.Find("guanMark");
			if (transform != null)
			{
				this.mGuanMark = transform.gameObject;
				Transform transform2 = transform.Find("ui96");
				if (transform2 != null)
				{
					this.mGuanMarkEffect = transform2.gameObject;
					Tools.SetParticleRenderQueue2(this.mGuanMarkEffect, 3200);
				}
			}
		}
		UIEventListener expr_1A3 = UIEventListener.Get(this.mTakeRewardBtnGo);
		expr_1A3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1A3.onClick, new UIEventListener.VoidDelegate(this.OnTakeRewardBtnClick));
		this.mTakeRewardBtnTxt = this.mTakeRewardBtn.transform.Find("Label").GetComponent<UILabel>();
	}

	private void Refresh()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (this.mGuildWarCity == null)
		{
			return;
		}
		if (this.mGuildWarCity.City.Status == EGuildWarCityState.EGWCS_Owned)
		{
			this.mGuildName.gameObject.SetActive(true);
			this.mGuildName.text = this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildWarCity.GuildName).ToString();
			if (Globals.Instance.Player.GuildSystem.Guild != null && this.mGuildWarCity.City.OwnerId == Globals.Instance.Player.GuildSystem.Guild.ID)
			{
				if (Globals.Instance.Player.GuildSystem.mGWPlayerData != null)
				{
					if (Globals.Instance.Player.GuildSystem.mGWPlayerData.Reward == EGuildWarReward.EGWR_No)
					{
						this.mTakeRewardBtnGo.SetActive(false);
					}
					else if (Globals.Instance.Player.GuildSystem.mGWPlayerData.Reward == EGuildWarReward.EGWR_NotTake)
					{
						this.mTakeRewardBtnGo.SetActive(true);
						this.mTakeRewardBtn.isEnabled = true;
						Tools.SetButtonState(this.mTakeRewardBtnGo, true);
						this.mTakeRewardBtnTxt.text = Singleton<StringManager>.Instance.GetString("guildCraft3");
						this.mTakeRewardBtnEffect.SetActive(true);
					}
					else if (Globals.Instance.Player.GuildSystem.mGWPlayerData.Reward == EGuildWarReward.EGWR_Taken)
					{
						this.mTakeRewardBtnGo.SetActive(true);
						this.mTakeRewardBtn.isEnabled = false;
						Tools.SetButtonState(this.mTakeRewardBtnGo, false);
						this.mTakeRewardBtnTxt.text = Singleton<StringManager>.Instance.GetString("guildSchool13");
						this.mTakeRewardBtnEffect.SetActive(false);
					}
				}
				else
				{
					this.mTakeRewardBtnGo.SetActive(false);
				}
				this.mGuildName.color = Color.green;
			}
			else
			{
				this.mTakeRewardBtnGo.SetActive(false);
				this.mGuildName.color = Color.white;
			}
			this.mBattleMark.SetActive(false);
			this.mTipGo.SetActive(false);
			if (this.mGuanMark != null)
			{
				this.mGuanMark.SetActive(true);
			}
		}
		else if (this.mGuildWarCity.City.Status == EGuildWarCityState.EGWCS_NoOwner)
		{
			this.mGuildName.gameObject.SetActive(false);
			this.mTakeRewardBtn.gameObject.SetActive(false);
			this.mBattleMark.SetActive(false);
			this.mTipGo.SetActive(false);
			if (this.mGuanMark != null)
			{
				this.mGuanMark.SetActive(false);
			}
		}
		else if (this.mGuildWarCity.City.Status == EGuildWarCityState.EGWCS_OnWar)
		{
			this.mGuildName.gameObject.SetActive(false);
			this.mTakeRewardBtnGo.SetActive(false);
			this.mTipGo.SetActive(true);
			this.mTipTxt.text = Singleton<StringManager>.Instance.GetString("guildCraft2");
			if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
			{
				this.mBattleMark.SetActive(true);
			}
			else
			{
				this.mBattleMark.SetActive(false);
			}
			if (this.mGuanMark != null)
			{
				this.mGuanMark.SetActive(false);
			}
		}
	}

	private void OnEnterStateNormal()
	{
		this.Refresh();
	}

	private void OnEnterStateFFHalfHourBefore()
	{
		this.Refresh();
	}

	private void OnEnterStateFinalFourPrepare()
	{
		this.Refresh();
	}

	private void OnEnterStateFinalFourGoing()
	{
		this.Refresh();
	}

	private void OnEnterStateFinalFourEnd()
	{
		this.Refresh();
	}

	private void OnEnterStateFinalPrepare()
	{
		this.Refresh();
	}

	private void OnEnterStateFinalGoing()
	{
		this.Refresh();
	}

	private void OnEnterStateFinalEnd()
	{
		this.Refresh();
	}

	public void Refresh(GuildWarClientCity wc)
	{
		this.mGuildWarCity = wc;
		if (this.mGuildWarCity == null)
		{
			return;
		}
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(this.mGuildWarCity.City.CityId);
		if (info != null)
		{
			this.mTowerName.text = info.CastleName;
			this.mTowerName.color = new Color32(255, 235, 215, 255);
			GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
			if (mWarStateInfo != null)
			{
				if (mWarStateInfo.mWarState == EGuildWarState.EGWS_Normal)
				{
					this.OnEnterStateNormal();
				}
				else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_SelectFourTeam)
				{
					this.OnEnterStateFFHalfHourBefore();
				}
				else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare)
				{
					this.OnEnterStateFinalFourPrepare();
				}
				else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing)
				{
					this.OnEnterStateFinalFourGoing();
				}
				else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourEnd)
				{
					this.OnEnterStateFinalFourEnd();
				}
				else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
				{
					this.OnEnterStateFinalPrepare();
				}
				else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
				{
					this.OnEnterStateFinalGoing();
				}
				else if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalEnd)
				{
					this.OnEnterStateFinalEnd();
				}
			}
		}
	}

	private void OnTakeRewardBtnClick(GameObject go)
	{
		if (Globals.Instance.Player.GuildSystem.mWarStateInfo == null)
		{
			return;
		}
		if (this.mGuildWarCity == null)
		{
			return;
		}
		if (this.mGuildWarCity.City.Status == EGuildWarCityState.EGWCS_Owned && Globals.Instance.Player.GuildSystem.Guild != null && this.mGuildWarCity.City.OwnerId == Globals.Instance.Player.GuildSystem.Guild.ID && Globals.Instance.Player.GuildSystem.HasGuildWarReward())
		{
			this.mBaseScene.mCityID = this.mGuildWarCity.City.CityId;
			MC2S_GuildWarTakeReward ojb = new MC2S_GuildWarTakeReward();
			Globals.Instance.CliSession.Send(991, ojb);
		}
	}

	private void OnTowerIconClick(GameObject go, bool isPressed)
	{
		if (this.mGuildWarCity == null)
		{
			return;
		}
		if (isPressed)
		{
			this.mBaseScene.ShowTowerOutPutTip(this.mGuildWarCity);
		}
		else
		{
			this.mBaseScene.HideTowerOutPutTip();
		}
	}
}
