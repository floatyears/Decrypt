using Att;
using Holoville.HOTween;
using Proto;
using System;
using System.Text;
using UnityEngine;

public class GuildCraftSetItem : MonoBehaviour
{
	private GameObject mStateTip;

	private UILabel mStateTipTxt;

	private GameObject mTeamFlag;

	private UISprite mTeamFlagSp;

	private UILabel mTeamFlagLb;

	private GameObject mBattleMark;

	private UISprite mItemIcon;

	private UILabel mName;

	private UILabel mName2;

	private GameObject mZhanLingMark;

	private UISlider mZhanLingProgres;

	private GameObject mBaoHuSp;

	private UILabel mAddScoreTip;

	private UILabel mAddScoreTipCD;

	private bool mIsRed;

	private int mIndex;

	private float mRefreshTimer;

	private float mRefreshTimerForScore;

	private string mGuildCraft62Str;

	private string mGuildCraft46Str;

	private string mGuildCraft76Str;

	private string mPointStr;

	private GameObject mEffect92;

	private GameObject mEffect92_1;

	private GameObject mEffect93;

	private GameObject mEffect94;

	private EGuildWarStrongholdState mHoldPreState = EGuildWarStrongholdState.EGWPS_Own;

	private StringBuilder mSb = new StringBuilder(42);

	public void InitWithBaseScene(bool isRed, int index)
	{
		this.mIsRed = isRed;
		this.mIndex = index;
		this.CreateObjects();
		this.Refresh();
		this.mRefreshTimer = Time.time;
		this.mRefreshTimerForScore = Time.time;
		this.mGuildCraft62Str = Singleton<StringManager>.Instance.GetString("guildCraft62");
		this.mPointStr = Singleton<StringManager>.Instance.GetString("point");
		this.mGuildCraft46Str = Singleton<StringManager>.Instance.GetString("guildCraft46");
		this.mGuildCraft76Str = Singleton<StringManager>.Instance.GetString("guildCraft76");
	}

	private void CreateObjects()
	{
		this.mItemIcon = base.transform.GetComponent<UISprite>();
		UIEventListener expr_1C = UIEventListener.Get(base.gameObject);
		expr_1C.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_1C.onClick, new UIEventListener.VoidDelegate(this.OnIconClick));
		this.mStateTip = base.transform.Find("stateTip").gameObject;
		UIEventListener expr_63 = UIEventListener.Get(this.mStateTip);
		expr_63.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_63.onClick, new UIEventListener.VoidDelegate(this.OnZhuFangClick));
		this.mStateTipTxt = this.mStateTip.transform.Find("txt").GetComponent<UILabel>();
		this.mStateTip.SetActive(false);
		this.mBattleMark = base.transform.Find("battleMark").gameObject;
		this.mBattleMark.SetActive(false);
		this.mTeamFlag = base.transform.Find("teamFlag").gameObject;
		this.mTeamFlagSp = this.mTeamFlag.GetComponent<UISprite>();
		this.mTeamFlagLb = this.mTeamFlag.transform.Find("num").GetComponent<UILabel>();
		this.mName = base.transform.Find("name").GetComponent<UILabel>();
		this.mName2 = base.transform.Find("name2").GetComponent<UILabel>();
		this.mAddScoreTip = base.transform.Find("addScore").GetComponent<UILabel>();
		this.mAddScoreTip.text = string.Empty;
		this.mAddScoreTip.gameObject.SetActive(false);
		this.mAddScoreTipCD = base.transform.Find("scoreCD").GetComponent<UILabel>();
		this.mAddScoreTipCD.text = string.Empty;
		this.mAddScoreTipCD.gameObject.SetActive(false);
		this.mZhanLingMark = base.transform.Find("zhangLingMark").gameObject;
		this.mEffect92_1 = this.mZhanLingMark.transform.Find("ui92_1").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect92_1, 3200);
		NGUITools.SetActive(this.mEffect92_1, false);
		this.mZhanLingMark.SetActive(false);
		this.mZhanLingProgres = base.transform.Find("zhanLing").GetComponent<UISlider>();
		this.mZhanLingProgres.gameObject.SetActive(false);
		this.mBaoHuSp = base.transform.Find("baohu").gameObject;
		this.mBaoHuSp.SetActive(false);
		this.mEffect92 = this.mBaoHuSp.transform.Find("ui92").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect92, 3200);
		NGUITools.SetActive(this.mEffect92, false);
		this.mEffect93 = base.transform.Find("ui93").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect93, 3200);
		NGUITools.SetActive(this.mEffect93, false);
		this.mEffect94 = base.transform.Find("ui94").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect94, 3200);
		NGUITools.SetActive(this.mEffect94, false);
	}

	public void ShowAddScoreEffect(int scoreNum)
	{
		if (scoreNum <= 0)
		{
			return;
		}
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		int num = (!this.mIsRed) ? (this.mIndex + 5) : this.mIndex;
		GuildWarStronghold guildWarStronghold = null;
		if (num < mGWEnterData.Strongholds.Count)
		{
			guildWarStronghold = mGWEnterData.Strongholds[num];
		}
		if (guildWarStronghold == null)
		{
			return;
		}
		if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Red)
		{
			this.mAddScoreTip.color = new Color32(255, 26, 26, 255);
			this.mAddScoreTip.effectColor = new Color32(105, 32, 32, 255);
		}
		else if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Blue)
		{
			this.mAddScoreTip.color = new Color32(82, 150, 255, 255);
			this.mAddScoreTip.effectColor = new Color32(31, 40, 126, 255);
		}
		this.mAddScoreTip.gameObject.SetActive(true);
		this.mAddScoreTip.transform.localPosition = new Vector3(0f, 20f, 0f);
		this.mAddScoreTip.color = new Color(this.mAddScoreTip.color.r, this.mAddScoreTip.color.g, this.mAddScoreTip.color.b, 1f);
		this.mAddScoreTip.text = this.mSb.Remove(0, this.mSb.Length).Append(this.mGuildCraft62Str).Append(scoreNum).ToString();
		HOTween.To(this.mAddScoreTip.transform, 2f, new TweenParms().Prop("localPosition", new Vector3(0f, 70f, 0f)).OnComplete(()=>
		{
			this.mAddScoreTip.gameObject.SetActive(false);
		}));
	}

	public void Refresh()
	{
		NGUITools.SetActive(this.mEffect93, false);
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		int num = (!this.mIsRed) ? (this.mIndex + 5) : this.mIndex;
		int selfStrongHoldIndex = Globals.Instance.Player.GuildSystem.GetSelfStrongHoldIndex();
		if (0 < selfStrongHoldIndex && selfStrongHoldIndex <= 10 && selfStrongHoldIndex - 1 == num)
		{
			NGUITools.SetActive(this.mEffect94, true);
			this.mName.color = Color.green;
		}
		else
		{
			NGUITools.SetActive(this.mEffect94, false);
			this.mName.color = Color.white;
		}
		GuildWarStronghold guildWarStronghold = null;
		if (num < mGWEnterData.Strongholds.Count)
		{
			guildWarStronghold = mGWEnterData.Strongholds[num];
		}
		if (guildWarStronghold == null)
		{
			return;
		}
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(num + 1);
		if (info == null)
		{
			return;
		}
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		this.mName.text = info.StrongholdName;
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
		}
		string value = this.mSb.ToString();
		this.mName2.text = this.mSb.Remove(0, this.mSb.Length).Append("(").Append(value).Append(")").ToString();
		if (this.mIndex == 0)
		{
			this.mItemIcon.spriteName = "paoTai";
		}
		else if (this.mIndex == 1)
		{
			this.mItemIcon.spriteName = "faMuChang";
		}
		else if (this.mIndex == 2)
		{
			this.mItemIcon.spriteName = "tieJiangPu";
		}
		else if (this.mIndex == 3)
		{
			this.mItemIcon.spriteName = "kuangDong";
		}
		else if (this.mIndex == 4)
		{
			this.mItemIcon.spriteName = "yingDi";
		}
		this.mItemIcon.MakePixelPerfect();
		bool flag = Globals.Instance.Player.GuildSystem.IsCanZhanMember(mGWEnterData.WarID);
		this.mStateTip.SetActive(false);
		if (guildWarStronghold.Status == EGuildWarStrongholdState.EGWPS_Neutrality)
		{
			if (flag)
			{
				this.mStateTip.SetActive(true);
				this.mStateTipTxt.text = this.mGuildCraft46Str;
				this.mStateTipTxt.color = Color.white;
			}
			this.mTeamFlag.SetActive(false);
			this.mBattleMark.SetActive(false);
			this.mZhanLingMark.SetActive(false);
			NGUITools.SetActive(this.mEffect92, false);
			NGUITools.SetActive(this.mEffect92_1, false);
			NGUITools.SetActive(this.mEffect93, false);
			this.mZhanLingProgres.gameObject.SetActive(false);
			this.mBaoHuSp.SetActive(false);
			this.mAddScoreTipCD.gameObject.SetActive(false);
		}
		else if (guildWarStronghold.Status == EGuildWarStrongholdState.EGWPS_Own)
		{
			GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
			if (mWarStateInfo != null)
			{
				if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare) && guildWarStronghold.OwnerId != selfTeamFlag)
				{
					this.mTeamFlag.SetActive(false);
				}
				else
				{
					this.mTeamFlag.SetActive(true);
					if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Red)
					{
						this.mTeamFlagSp.spriteName = "0Flag";
						if (this.mHoldPreState == EGuildWarStrongholdState.EGWPS_OwnerChanging)
						{
							NGUITools.SetActive(this.mEffect93, true);
						}
						this.mHoldPreState = EGuildWarStrongholdState.EGWPS_Own;
					}
					else if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Blue)
					{
						this.mTeamFlagSp.spriteName = "Flag1";
						if (this.mHoldPreState == EGuildWarStrongholdState.EGWPS_OwnerChanging)
						{
							NGUITools.SetActive(this.mEffect93, true);
						}
						this.mHoldPreState = EGuildWarStrongholdState.EGWPS_Own;
					}
					else
					{
						this.mTeamFlag.SetActive(false);
					}
					if (this.mTeamFlag.activeInHierarchy)
					{
						this.mSb.Remove(0, this.mSb.Length).Append(guildWarStronghold.DefenceNum).Append("/").Append(guildWarStronghold.Slots.Count);
						this.mTeamFlagLb.text = this.mSb.ToString();
					}
				}
				if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare)
				{
					if (guildWarStronghold.OwnerId == selfTeamFlag && guildWarStronghold.DefenceNum < guildWarStronghold.Slots.Count && flag)
					{
						this.mStateTip.SetActive(true);
						this.mStateTipTxt.text = this.mGuildCraft46Str;
						this.mStateTipTxt.color = Color.white;
					}
				}
				else if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing) && guildWarStronghold.OwnerId != selfTeamFlag && guildWarStronghold.DefenceNum > 0 && flag)
				{
					this.mStateTip.SetActive(true);
					this.mStateTipTxt.text = this.mGuildCraft76Str;
					this.mStateTipTxt.color = Color.red;
				}
			}
			else
			{
				this.mTeamFlag.SetActive(false);
			}
			this.mBattleMark.SetActive(false);
			for (int i = 0; i < guildWarStronghold.Slots.Count; i++)
			{
				if (guildWarStronghold.Slots[i].Status == EGuardWarStrongholdSlotState.EGWPSS_War)
				{
					this.mBattleMark.SetActive(true);
					break;
				}
			}
			this.mZhanLingMark.SetActive(false);
			NGUITools.SetActive(this.mEffect92, false);
			NGUITools.SetActive(this.mEffect92_1, false);
			this.mZhanLingProgres.gameObject.SetActive(false);
			this.mBaoHuSp.SetActive(false);
			if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Red)
			{
				this.mAddScoreTipCD.color = new Color32(255, 26, 26, 255);
				this.mAddScoreTipCD.effectColor = new Color32(105, 32, 32, 255);
			}
			else if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Blue)
			{
				this.mAddScoreTipCD.color = new Color32(82, 150, 255, 255);
				this.mAddScoreTipCD.effectColor = new Color32(31, 40, 126, 255);
			}
		}
		else if (guildWarStronghold.Status == EGuildWarStrongholdState.EGWPS_Protected)
		{
			this.mTeamFlag.SetActive(true);
			if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Red)
			{
				this.mTeamFlagSp.spriteName = "0Flag";
				if (guildWarStronghold.OwnerId == selfTeamFlag && guildWarStronghold.DefenceNum < guildWarStronghold.Slots.Count && flag)
				{
					this.mStateTip.SetActive(true);
					this.mStateTipTxt.text = this.mGuildCraft46Str;
					this.mStateTipTxt.color = Color.white;
				}
			}
			else if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Blue)
			{
				this.mTeamFlagSp.spriteName = "Flag1";
				if (guildWarStronghold.OwnerId == selfTeamFlag && guildWarStronghold.DefenceNum < guildWarStronghold.Slots.Count && flag)
				{
					this.mStateTip.SetActive(true);
					this.mStateTipTxt.text = this.mGuildCraft46Str;
					this.mStateTipTxt.color = Color.white;
				}
			}
			else
			{
				this.mTeamFlag.SetActive(false);
			}
			this.mTeamFlagLb.text = string.Empty;
			this.mBattleMark.SetActive(false);
			this.mZhanLingMark.SetActive(false);
			NGUITools.SetActive(this.mEffect92, true);
			NGUITools.SetActive(this.mEffect92_1, false);
			NGUITools.SetActive(this.mEffect93, false);
			this.mZhanLingProgres.gameObject.SetActive(false);
			this.mBaoHuSp.SetActive(true);
			this.mAddScoreTipCD.gameObject.SetActive(false);
		}
		else if (guildWarStronghold.Status == EGuildWarStrongholdState.EGWPS_OwnerChanging)
		{
			this.mHoldPreState = EGuildWarStrongholdState.EGWPS_OwnerChanging;
			this.mTeamFlag.SetActive(true);
			if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Red)
			{
				this.mTeamFlagSp.spriteName = "0Flag";
				if (guildWarStronghold.OwnerId == selfTeamFlag && guildWarStronghold.DefenceNum < guildWarStronghold.Slots.Count && flag)
				{
					this.mStateTip.SetActive(true);
					this.mStateTipTxt.text = this.mGuildCraft46Str;
					this.mStateTipTxt.color = Color.white;
				}
			}
			else if (guildWarStronghold.OwnerId == EGuildWarTeamId.EGWTI_Blue)
			{
				this.mTeamFlagSp.spriteName = "Flag1";
				if (guildWarStronghold.OwnerId == selfTeamFlag && guildWarStronghold.DefenceNum < guildWarStronghold.Slots.Count && flag)
				{
					this.mStateTip.SetActive(true);
					this.mStateTipTxt.text = this.mGuildCraft46Str;
					this.mStateTipTxt.color = Color.white;
				}
			}
			else
			{
				this.mTeamFlag.SetActive(false);
			}
			this.mTeamFlagLb.text = string.Empty;
			this.mBattleMark.SetActive(false);
			this.mBaoHuSp.SetActive(false);
			this.mZhanLingMark.SetActive(true);
			NGUITools.SetActive(this.mEffect92, false);
			NGUITools.SetActive(this.mEffect92_1, true);
			NGUITools.SetActive(this.mEffect93, false);
			this.mZhanLingProgres.gameObject.SetActive(true);
			this.mZhanLingProgres.value = Mathf.Clamp01((float)guildWarStronghold.Para1 / 100f);
			this.mAddScoreTipCD.gameObject.SetActive(false);
		}
	}

	private void DoClickEvent()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_001");
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
		EGuildWarTeamId selfTeamFlag = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		if (Globals.Instance.Player.GuildSystem.IsGuanZhanMember(mGWEnterData.WarID) && (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare))
		{
			return;
		}
		int index = (!this.mIsRed) ? (this.mIndex + 5) : this.mIndex;
		GuildWarStronghold guildWarStronghold = mGWEnterData.Strongholds[index];
		if (guildWarStronghold == null)
		{
			return;
		}
		MC2S_GuildWarQueryStrongholdInfo mC2S_GuildWarQueryStrongholdInfo = new MC2S_GuildWarQueryStrongholdInfo();
		mC2S_GuildWarQueryStrongholdInfo.WarID = mGWEnterData.WarID;
		mC2S_GuildWarQueryStrongholdInfo.TeamID = selfTeamFlag;
		mC2S_GuildWarQueryStrongholdInfo.StrongholdID = guildWarStronghold.ID;
		Globals.Instance.CliSession.Send(981, mC2S_GuildWarQueryStrongholdInfo);
	}

	private void OnIconClick(GameObject go)
	{
		this.DoClickEvent();
	}

	private void OnZhuFangClick(GameObject go)
	{
		this.DoClickEvent();
	}

	private void RefreshPrograss()
	{
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		int index = (!this.mIsRed) ? (this.mIndex + 5) : this.mIndex;
		GuildWarStronghold guildWarStronghold = mGWEnterData.Strongholds[index];
		if (guildWarStronghold == null)
		{
			return;
		}
		if (guildWarStronghold.Status == EGuildWarStrongholdState.EGWPS_OwnerChanging)
		{
			this.mZhanLingProgres.value = Mathf.Clamp01((float)guildWarStronghold.Para1 / 100f);
		}
	}

	private void RefreshScoreCDTxt()
	{
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData == null)
		{
			return;
		}
		int index = (!this.mIsRed) ? (this.mIndex + 5) : this.mIndex;
		GuildWarStronghold guildWarStronghold = mGWEnterData.Strongholds[index];
		if (guildWarStronghold == null)
		{
			return;
		}
		if (!this.mAddScoreTipCD.gameObject.activeInHierarchy)
		{
			this.mAddScoreTipCD.gameObject.SetActive(true);
		}
		int timeStamp = Globals.Instance.Player.GetTimeStamp();
		int num = guildWarStronghold.Para1 - timeStamp;
		if (num > 60)
		{
			num = 60;
			guildWarStronghold.Para1 = timeStamp + num;
		}
		else if (num < 0)
		{
			num = 0;
		}
		this.mAddScoreTipCD.text = this.mSb.Remove(0, this.mSb.Length).Append(num).Append("s").ToString();
	}

	private void Update()
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
		int index = (!this.mIsRed) ? (this.mIndex + 5) : this.mIndex;
		GuildWarStronghold guildWarStronghold = mGWEnterData.Strongholds[index];
		if (guildWarStronghold == null)
		{
			return;
		}
		if (this.mZhanLingProgres.gameObject.activeInHierarchy && Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			if (this.mZhanLingProgres.gameObject.activeInHierarchy)
			{
				this.RefreshPrograss();
			}
		}
		if (this.mAddScoreTipCD == null)
		{
			return;
		}
		if (Time.time - this.mRefreshTimerForScore >= 0.3f)
		{
			if ((mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing) && guildWarStronghold.Status == EGuildWarStrongholdState.EGWPS_Own)
			{
				this.mRefreshTimerForScore = Time.time;
				if (guildWarStronghold.Status == EGuildWarStrongholdState.EGWPS_Own)
				{
					this.RefreshScoreCDTxt();
				}
				else
				{
					this.mAddScoreTipCD.gameObject.SetActive(false);
				}
			}
			else if (this.mAddScoreTipCD.gameObject.activeInHierarchy)
			{
				this.mAddScoreTipCD.gameObject.SetActive(false);
			}
		}
	}
}
