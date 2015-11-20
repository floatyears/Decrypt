using Proto;
using ProtoBuf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class GUIWorldBossVictoryScene : GameUISession
{
	private GameObject mVictorySprite;

	private Transform mTextBg;

	private Transform mBtnGroup;

	private UILabel mTimeLb;

	private UILabel mMaxScoreLb;

	private UILabel mFireLb;

	private GameObject mKillerObj;

	private UILabel mKillerGoldLb;

	private bool sendDamageReward;

	private float sendTimer;

	protected override void OnPostLoadGUI()
	{
		Globals.Instance.BackgroundMusicMgr.StopWarmingSound();
		Globals.Instance.BackgroundMusicMgr.ClearGameBGM();
		Globals.Instance.BackgroundMusicMgr.StopGameClearSound();
		Globals.Instance.BackgroundMusicMgr.PlayLobbyMusic("ui/ui_004b", false);
		this.CreateObjects();
		Globals.Instance.CliSession.Register(639, new ClientSession.MsgHandler(this.OnMsgTakeWorldBossDamageReward));
	}

	protected override void OnLoadedFinished()
	{
		MC2S_TakeWorldBossDamageReward ojb = new MC2S_TakeWorldBossDamageReward();
		this.sendDamageReward = Globals.Instance.CliSession.Send(638, ojb);
	}

	private void CreateObjects()
	{
		Transform transform = base.transform.Find("victory");
		this.mVictorySprite = transform.Find("victorySprite").gameObject;
		this.mVictorySprite.transform.localScale = Vector3.zero;
		this.mTextBg = transform.Find("textBg");
		this.mTimeLb = this.mTextBg.Find("timeNum").GetComponent<UILabel>();
		this.mTimeLb.text = string.Empty;
		this.mMaxScoreLb = this.mTextBg.Find("scoreNum").GetComponent<UILabel>();
		this.mMaxScoreLb.text = string.Empty;
		this.mFireLb = this.mTextBg.Find("fireTxt/fireNum").GetComponent<UILabel>();
		this.mFireLb.text = string.Empty;
		this.mTextBg.localScale = Vector3.zero;
		this.mKillerObj = this.mTextBg.Find("killerTxt").gameObject;
		this.mKillerGoldLb = this.mKillerObj.transform.Find("killerGold").GetComponent<UILabel>();
		this.mBtnGroup = base.transform.Find("ButtonGroup");
		GameObject gameObject = this.mBtnGroup.Find("sureBtn").gameObject;
		UIEventListener expr_14B = UIEventListener.Get(gameObject);
		expr_14B.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_14B.onClick, new UIEventListener.VoidDelegate(this.OnSureBtnClick));
		this.mBtnGroup.transform.localScale = Vector3.zero;
	}

	protected override void OnPreDestroyGUI()
	{
		GameUIManager.mInstance.uiState.WorldBossKillerName = null;
		Globals.Instance.CliSession.Unregister(639, new ClientSession.MsgHandler(this.OnMsgTakeWorldBossDamageReward));
	}

	private void Update()
	{
		if (this.sendDamageReward)
		{
			return;
		}
		this.sendTimer += Time.time;
		if (this.sendTimer < 0.2f)
		{
			return;
		}
		this.sendTimer = 0f;
		MC2S_TakeWorldBossDamageReward ojb = new MC2S_TakeWorldBossDamageReward();
		this.sendDamageReward = Globals.Instance.CliSession.Send(638, ojb);
	}

	[DebuggerHidden]
	private IEnumerator Refresh(long totalDamage, int rewardMoney)
	{
        return null;
        //GUIWorldBossVictoryScene.<Refresh>c__IteratorA0 <Refresh>c__IteratorA = new GUIWorldBossVictoryScene.<Refresh>c__IteratorA0();
        //<Refresh>c__IteratorA.totalDamage = totalDamage;
        //<Refresh>c__IteratorA.rewardMoney = rewardMoney;
        //<Refresh>c__IteratorA.<$>totalDamage = totalDamage;
        //<Refresh>c__IteratorA.<$>rewardMoney = rewardMoney;
        //<Refresh>c__IteratorA.<>f__this = this;
        //return <Refresh>c__IteratorA;
	}

	public static void BackBossScene()
	{
		if (Globals.Instance.Player.WorldBossSystem.WorldBossIsOver())
		{
			GameUIManager.mInstance.uiState.WorldBossIsOver = true;
			GameUIManager.mInstance.ChangeSession<GUIBossReadyScene>(null, true, true);
		}
		else
		{
			GameUIManager.mInstance.ChangeSession<GUIBossMapScene>(null, true, false);
		}
	}

	private void OnSureBtnClick(GameObject go)
	{
		GUIWorldBossVictoryScene.BackBossScene();
	}

	private void UpdateTimeText(int time)
	{
		this.mTimeLb.text = Tools.UpdateTimeText(time);
	}

	public void OnMsgTakeWorldBossDamageReward(MemoryStream stream)
	{
		MS2C_TakeWorldBossDamageReward mS2C_TakeWorldBossDamageReward = Serializer.NonGeneric.Deserialize(typeof(MS2C_TakeWorldBossDamageReward), stream) as MS2C_TakeWorldBossDamageReward;
		if (mS2C_TakeWorldBossDamageReward.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("PveR", mS2C_TakeWorldBossDamageReward.Result);
			return;
		}
		base.StartCoroutine(this.Refresh(mS2C_TakeWorldBossDamageReward.Damage, mS2C_TakeWorldBossDamageReward.FireDragonScale));
	}
}
