using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIGuildCraftKillLog : GameUIBasePopup
{
	private UILabel mTimeLb;

	private UILabel mRedName;

	private UILabel mRedScoreNum;

	private UILabel mRedKillNum;

	private UILabel mBlueName;

	private UILabel mBlueScoreNum;

	private UILabel mBlueKillNum;

	private GameObject mRedShengLiBg;

	private GameObject mRedCommonBg;

	private GameObject mRedShengLiPanel;

	private GameObject mBlueShengLiPanel;

	private GameObject mBlueShengLiBg;

	private GameObject mBlueCommonBg;

	private UILabel mSelfKillNum;

	private GuildCraftKillLogTable mRedTable;

	private GuildCraftKillLogTable mBlueTable;

	private GameObject mQuitBtn;

	private UILabel mQuitBtnLb;

	private GameObject mEffect91;

	private GameObject mEffect91_1;

	private float mRefreshTimer;

	public static bool IsActive()
	{
		return GameUIPopupManager.GetInstance().GetState() == GameUIPopupManager.eSTATE.GUIGuildCraftKillLog;
	}

	public static void ShowMe()
	{
		GameUIPopupManager.GetInstance().PushState(GameUIPopupManager.eSTATE.GUIGuildCraftKillLog, false, null, null);
	}

	public override void OnButtonBlockerClick()
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		if (Globals.Instance.Player.GuildSystem.mGWKillRankData.Winner == EGuildWarTeamId.EGWTI_None)
		{
			GameUIPopupManager.GetInstance().PopState(true, null);
		}
	}

	private void Awake()
	{
		this.CreateObjects();
		NGUITools.SetActive(this.mEffect91, false);
		NGUITools.SetActive(this.mEffect91_1, false);
		base.StartCoroutine(this.InitRecordItems());
		this.mRefreshTimer = Time.time;
	}

	private void CreateObjects()
	{
		this.mEffect91 = base.transform.Find("ui91").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect91, 5450);
		NGUITools.SetActive(this.mEffect91, false);
		this.mEffect91_1 = base.transform.Find("ui91_1").gameObject;
		Tools.SetParticleRQWithUIScale(this.mEffect91_1, 5450);
		NGUITools.SetActive(this.mEffect91_1, false);
		Transform transform = base.transform.Find("winBg");
		this.mQuitBtn = transform.Find("backMenu").gameObject;
		UIEventListener expr_A0 = UIEventListener.Get(this.mQuitBtn);
		expr_A0.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(expr_A0.onClick, new UIEventListener.VoidDelegate(this.OnQuitClick));
		this.mQuitBtnLb = this.mQuitBtn.transform.Find("Label").GetComponent<UILabel>();
		this.mQuitBtn.SetActive(true);
		Transform transform2 = transform.Find("headInfo");
		this.mTimeLb = transform2.Find("cdBg/cdTime").GetComponent<UILabel>();
		this.mTimeLb.text = string.Empty;
		this.mRedName = transform2.Find("redBg/name").GetComponent<UILabel>();
		this.mRedScoreNum = transform2.Find("numBg/num0").GetComponent<UILabel>();
		this.mBlueName = transform2.Find("blueBg/name").GetComponent<UILabel>();
		this.mBlueScoreNum = transform2.Find("numBg/num1").GetComponent<UILabel>();
		Transform transform3 = transform.Find("redBg");
		this.mRedKillNum = transform3.Find("txt0/num").GetComponent<UILabel>();
		this.mRedTable = transform3.Find("contentsPanel/contents").gameObject.AddComponent<GuildCraftKillLogTable>();
		this.mRedTable.maxPerLine = 1;
		this.mRedTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mRedTable.cellWidth = 385f;
		this.mRedTable.cellHeight = 74f;
		this.mRedShengLiBg = transform3.Find("shengLi").gameObject;
		this.mRedShengLiPanel = transform3.Find("shengLiPanel").gameObject;
		this.mRedCommonBg = transform3.Find("Bg").gameObject;
		Transform transform4 = transform.Find("blueBg");
		this.mBlueKillNum = transform4.Find("txt0/num").GetComponent<UILabel>();
		this.mBlueTable = transform4.Find("contentsPanel/contents").gameObject.AddComponent<GuildCraftKillLogTable>();
		this.mBlueTable.maxPerLine = 1;
		this.mBlueTable.arrangement = UICustomGrid.Arrangement.Vertical;
		this.mBlueTable.cellWidth = 385f;
		this.mBlueTable.cellHeight = 74f;
		this.mBlueShengLiBg = transform4.Find("shengLi").gameObject;
		this.mBlueShengLiPanel = transform4.Find("shengLiPanel").gameObject;
		this.mBlueCommonBg = transform4.Find("Bg").gameObject;
		this.mSelfKillNum = transform.Find("txt0/num").GetComponent<UILabel>();
	}

	private int SortMembers(GuildWarClientTeamMember aM, GuildWarClientTeamMember bM)
	{
		if (aM == null || bM == null || aM.Member == null || bM.Member == null)
		{
			return 0;
		}
		if (aM.Member.Score > bM.Member.Score)
		{
			return -1;
		}
		if (aM.Member.Score < bM.Member.Score)
		{
			return 1;
		}
		if (aM.Member.KillNum > bM.Member.KillNum)
		{
			return -1;
		}
		if (aM.Member.KillNum < bM.Member.KillNum)
		{
			return 1;
		}
		if (aM.Member.KillerTimestamp < bM.Member.KillerTimestamp)
		{
			return -1;
		}
		if (aM.Member.KillerTimestamp > bM.Member.KillerTimestamp)
		{
			return 1;
		}
		return 0;
	}

	[DebuggerHidden]
	public IEnumerator InitRecordItems()
	{
        return null;
        //GUIGuildCraftKillLog.<InitRecordItems>c__Iterator56 <InitRecordItems>c__Iterator = new GUIGuildCraftKillLog.<InitRecordItems>c__Iterator56();
        //<InitRecordItems>c__Iterator.<>f__this = this;
        //return <InitRecordItems>c__Iterator;
	}

	private void OnQuitClick(GameObject go)
	{
		Globals.Instance.EffectSoundMgr.Play("ui/ui_002");
		GameUIPopupManager.GetInstance().PopState(true, null);
		if (Globals.Instance.Player.GuildSystem.mGWKillRankData != null && Globals.Instance.Player.GuildSystem.mGWKillRankData.Winner != EGuildWarTeamId.EGWTI_None)
		{
			Globals.Instance.Player.GuildSystem.RequestQueryWarState();
			GameUIManager.mInstance.ChangeSession<GUIGuildCraftScene>(null, false, false);
		}
	}

	private void RefreshTopTime()
	{
		GuildWarStateInfo mWarStateInfo = Globals.Instance.Player.GuildSystem.mWarStateInfo;
		if (mWarStateInfo == null)
		{
			return;
		}
		if (mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalFourGoing || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalPrepare || mWarStateInfo.mWarState == EGuildWarState.EGWS_FinalGoing)
		{
			int num = mWarStateInfo.mTimeStamp - Globals.Instance.Player.GetTimeStamp();
			if (num <= 0)
			{
				num = 0;
			}
			this.mTimeLb.text = Tools.FormatTime2(num);
		}
		else
		{
			this.mTimeLb.text = Tools.FormatTime2(0);
		}
	}

	private void Update()
	{
		if (Time.time - this.mRefreshTimer >= 1f)
		{
			this.mRefreshTimer = Time.time;
			this.RefreshTopTime();
		}
	}
}
