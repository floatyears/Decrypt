using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIGuildSchoolVictoryScene : GameUISession
{
	private GameObject mVictorySprite;

	private Transform mTextBg;

	private Transform mBtnGroup;

	private UISlider mProgressBar;

	private UILabel mProgressTxt;

	private UILabel mDamageLb;

	private UILabel mGoldLb;

	private UILabel mExNum;

	private GameObject mKillerTip;

	public float mOldHpProgress;

	public float mCurHpProgress;

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
		Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("ui/ui_004b", false);
		GameUIManager.mInstance.DestroyGameUIOptionPopUp();
		this.CreateObjects();
		Globals.Instance.CliSession.Register(968, new ClientSession.MsgHandler(this.OnMsgTakeGuildBossDamageReward));
		this.DoSendRequest();
	}

	private void DoSendRequest()
	{
		MC2S_TakeGuildBossDamageReward mC2S_TakeGuildBossDamageReward = new MC2S_TakeGuildBossDamageReward();
		mC2S_TakeGuildBossDamageReward.ID = Globals.Instance.Player.GuildSystem.Guild.AttackAcademyID1;
		Globals.Instance.CliSession.Send(967, mC2S_TakeGuildBossDamageReward);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("victory");
		this.mVictorySprite = transform.Find("victorySprite").gameObject;
		this.mVictorySprite.transform.localScale = Vector3.zero;
		this.mOldHpProgress = Mathf.Clamp01(1f - GameUIManager.mInstance.uiState.GuildBossHp);
		this.mCurHpProgress = Mathf.Clamp01(1f - Globals.Instance.Player.GuildSystem.GetGuildBossData(Globals.Instance.Player.GuildSystem.Guild.AttackAcademyID1).HealthPct);
		this.mTextBg = transform.Find("textBg");
		this.mTextBg.transform.localScale = Vector3.zero;
		this.mProgressTxt = this.mTextBg.Find("progressNum").GetComponent<UILabel>();
		this.mProgressTxt.text = string.Format("{0:F}%", this.mOldHpProgress * 100f);
		this.mProgressBar = this.mProgressTxt.transform.Find("progressBar").GetComponent<UISlider>();
		this.mProgressBar.value = this.mOldHpProgress;
		this.mDamageLb = this.mTextBg.Find("scoreNum").GetComponent<UILabel>();
		this.mDamageLb.text = "0";
		this.mGoldLb = this.mTextBg.Find("goldTxt/goldNum").GetComponent<UILabel>();
		this.mGoldLb.text = "0";
		this.mKillerTip = this.mTextBg.Find("killerTxt").gameObject;
		this.mExNum = this.mKillerTip.transform.Find("num").GetComponent<UILabel>();
		this.mKillerTip.transform.localScale = Vector3.zero;
		this.mBtnGroup = base.transform.Find("ButtonGroup");
		GameObject gameObject = this.mBtnGroup.Find("sureBtn").gameObject;
		UIEventListener expr_20F = UIEventListener.Get(gameObject);
		expr_20F.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_20F.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		this.mBtnGroup.localScale = Vector3.zero;
	}

	protected override void OnPreDestroyGUI()
	{
		if (Globals.Instance == null)
		{
			return;
		}
		Globals.Instance.CliSession.Unregister(968, new ClientSession.MsgHandler(this.OnMsgTakeGuildBossDamageReward));
	}

	private void OnSureBtnClick(GameObject go)
	{
		GameUIManager.mInstance.ChangeSession<GUIGuildSchoolScene>(null, true, true);
	}

	public void OnMsgTakeGuildBossDamageReward(MemoryStream stream)
	{
		MS2C_TakeGuildBossDamageReward mS2C_TakeGuildBossDamageReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeGuildBossDamageReward), stream) as MS2C_TakeGuildBossDamageReward;
		if (mS2C_TakeGuildBossDamageReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_TakeGuildBossDamageReward.Result);
			return;
		}
		base.StartCoroutine(this.Refresh(mS2C_TakeGuildBossDamageReward.Damage, mS2C_TakeGuildBossDamageReward.Reputation, mS2C_TakeGuildBossDamageReward.ExtraReputation));
	}

	[DebuggerHidden]
	private IEnumerator Refresh(long totalDamage, int rewardReputation, int extraReputation)
	{
        return null;
        //GUIGuildSchoolVictoryScene.<Refresh>c__Iterator61 <Refresh>c__Iterator = new GUIGuildSchoolVictoryScene.<Refresh>c__Iterator61();
        //<Refresh>c__Iterator.totalDamage = totalDamage;
        //<Refresh>c__Iterator.rewardReputation = rewardReputation;
        //<Refresh>c__Iterator.extraReputation = extraReputation;
        //<Refresh>c__Iterator.<$>totalDamage = totalDamage;
        //<Refresh>c__Iterator.<$>rewardReputation = rewardReputation;
        //<Refresh>c__Iterator.<$>extraReputation = extraReputation;
        //<Refresh>c__Iterator.<>f__this = this;
        //return <Refresh>c__Iterator;
	}

	private void OnProgressUpdate()
	{
		this.mProgressTxt.text = string.Format("{0:F}%", this.mOldHpProgress * 100f);
		this.mProgressBar.value = this.mOldHpProgress;
	}
}
