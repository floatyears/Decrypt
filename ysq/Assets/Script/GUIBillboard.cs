using Proto;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GUIBillboard : GameUISession
{
	public AnimationCurve ParentBtnCurve;

	[NonSerialized]
	public float ParentBtnDuration = 0.2f;

	[NonSerialized]
	public UITable mBtnContents;

	[NonSerialized]
	public BillboardParentBtn mCurParentBtn;

	[NonSerialized]
	public BillboardChildBtn mCurChildBtn;

	[NonSerialized]
	public bool mHasStarted;

	[NonSerialized]
	public bool isWaitingMessageReply;

	private UIScrollView scrollView;

	private BillboardCommonLayer layer;

	private BillboardParentBtn firstBtn;

	private bool hasInitPVPData;

	private void CreateObjects()
	{
		TopGoods topGoods = GameUIManager.mInstance.GetTopGoods();
		topGoods.Show("billboardLb");
		GameObject parent = GameUITools.FindGameObject("WinBg", base.gameObject);
		this.layer = GameUITools.FindGameObject("Layer", parent).AddComponent<BillboardCommonLayer>();
		this.layer.InitBillboard(this);
		this.scrollView = GameUITools.FindGameObject("ButtonsPanel", parent).GetComponent<UIScrollView>();
		this.mBtnContents = GameUITools.FindGameObject("Contents", this.scrollView.gameObject).GetComponent<UITable>();
		BillboardParentBtn billboardParentBtn = this.InitParentBtn(Singleton<StringManager>.Instance.GetString("BillboardFighting"));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardFighting"), new BillboardChildBtn.VoidCallback(this.OnCombatValueBillboardBtnClick)));
		this.firstBtn = billboardParentBtn;
		billboardParentBtn = this.InitParentBtn(Singleton<StringManager>.Instance.GetString("BillboardLevel"));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardLevel"), new BillboardChildBtn.VoidCallback(this.OnLevelBillboardBtnClick)));
		billboardParentBtn = this.InitParentBtn(Singleton<StringManager>.Instance.GetString("BillboardPVE"));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardPVEStars"), new BillboardChildBtn.VoidCallback(this.OnPVEStarsBillboardBtnClick)));
		billboardParentBtn = this.InitParentBtn(Singleton<StringManager>.Instance.GetString("BillboardPVP4"));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardPVP4"), new BillboardChildBtn.VoidCallback(this.OnPvp4BillboardBtnClick)));
		billboardParentBtn = this.InitParentBtn(Singleton<StringManager>.Instance.GetString("BillboardGuild"));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardGuild"), new BillboardChildBtn.VoidCallback(this.OnGuildBillboardBtnClick)));
		billboardParentBtn = this.InitParentBtn(Singleton<StringManager>.Instance.GetString("BillboardActivity"));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardActivityTrial"), new BillboardChildBtn.VoidCallback(this.OnTrialBillboardBtnClick)));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardActivityWorldBoss"), new BillboardChildBtn.VoidCallback(this.OnWorldBossBillboardBtnClick)));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardRose"), new BillboardChildBtn.VoidCallback(this.OnRoseBillboardBtnClick)));
		billboardParentBtn.AddChildBtn(this.InitChildBtn(Singleton<StringManager>.Instance.GetString("BillboardTurtle"), new BillboardChildBtn.VoidCallback(this.OnTurtleBillboardBtnClick)));
		this.mBtnContents.repositionNow = true;
		base.StartCoroutine(this.OnOpenAnimEnd());
	}

	[DebuggerHidden]
	private IEnumerator OnOpenAnimEnd()
	{
        return null;
        //GUIBillboard.<OnOpenAnimEnd>c__Iterator67 <OnOpenAnimEnd>c__Iterator = new GUIBillboard.<OnOpenAnimEnd>c__Iterator67();
        //<OnOpenAnimEnd>c__Iterator.<>f__this = this;
        //return <OnOpenAnimEnd>c__Iterator;
	}

	protected override void OnPostLoadGUI()
	{
		this.CreateObjects();
		BillboardSubSystem expr_15 = Globals.Instance.Player.BillboardSystem;
		expr_15.QueryCombatValueRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_15.QueryCombatValueRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryCombatValueRankEvent));
		BillboardSubSystem expr_45 = Globals.Instance.Player.BillboardSystem;
		expr_45.QueryPVEStarsRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_45.QueryPVEStarsRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryPVEStarsRankEvent));
		BillboardSubSystem expr_75 = Globals.Instance.Player.BillboardSystem;
		expr_75.QueryLevelRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_75.QueryLevelRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryLevelRankEvent));
		BillboardSubSystem expr_A5 = Globals.Instance.Player.BillboardSystem;
		expr_A5.QueryTrialRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_A5.QueryTrialRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryTrialRankEvent));
		GuildSubSystem expr_D5 = Globals.Instance.Player.GuildSystem;
		expr_D5.QueryGuildRankEvent = (GuildSubSystem.QueryGuildRankCallback)Delegate.Combine(expr_D5.QueryGuildRankEvent, new GuildSubSystem.QueryGuildRankCallback(this.OnQueryGuildRankEvent));
		BillboardSubSystem expr_105 = Globals.Instance.Player.BillboardSystem;
		expr_105.QueryRoseRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_105.QueryRoseRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryRoseRankEvent));
		BillboardSubSystem expr_135 = Globals.Instance.Player.BillboardSystem;
		expr_135.QueryTurtleRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Combine(expr_135.QueryTurtleRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryTurtleRankEvent));
		PvpSubSystem expr_165 = Globals.Instance.Player.PvpSystem;
		expr_165.QueryArenaDataEvent = (PvpSubSystem.VoidCallback)Delegate.Combine(expr_165.QueryArenaDataEvent, new PvpSubSystem.VoidCallback(this.OnQueryPvpDataEvent));
		PvpSubSystem expr_195 = Globals.Instance.Player.PvpSystem;
		expr_195.QueryArenaRankEvent = (PvpSubSystem.VoidCallback)Delegate.Combine(expr_195.QueryArenaRankEvent, new PvpSubSystem.VoidCallback(this.OnQueryPvpRankEvent));
		WorldBossSubSystem expr_1C5 = Globals.Instance.Player.WorldBossSystem;
		expr_1C5.GetBossDataEvent = (WorldBossSubSystem.VoidCallback)Delegate.Combine(expr_1C5.GetBossDataEvent, new WorldBossSubSystem.VoidCallback(this.OnGetBossDataEvent));
		LocalPlayer expr_1F0 = Globals.Instance.Player;
		expr_1F0.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Combine(expr_1F0.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnGetPlayerData));
	}

	private void OnGetPlayerData(bool versionInit, bool newPlayer)
	{
		this.isWaitingMessageReply = false;
	}

	protected override void OnPreDestroyGUI()
	{
		BillboardSubSystem expr_0F = Globals.Instance.Player.BillboardSystem;
		expr_0F.QueryCombatValueRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_0F.QueryCombatValueRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryCombatValueRankEvent));
		BillboardSubSystem expr_3F = Globals.Instance.Player.BillboardSystem;
		expr_3F.QueryPVEStarsRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_3F.QueryPVEStarsRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryPVEStarsRankEvent));
		BillboardSubSystem expr_6F = Globals.Instance.Player.BillboardSystem;
		expr_6F.QueryLevelRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_6F.QueryLevelRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryLevelRankEvent));
		BillboardSubSystem expr_9F = Globals.Instance.Player.BillboardSystem;
		expr_9F.QueryTrialRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_9F.QueryTrialRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryTrialRankEvent));
		GuildSubSystem expr_CF = Globals.Instance.Player.GuildSystem;
		expr_CF.QueryGuildRankEvent = (GuildSubSystem.QueryGuildRankCallback)Delegate.Remove(expr_CF.QueryGuildRankEvent, new GuildSubSystem.QueryGuildRankCallback(this.OnQueryGuildRankEvent));
		BillboardSubSystem expr_FF = Globals.Instance.Player.BillboardSystem;
		expr_FF.QueryRoseRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_FF.QueryRoseRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryRoseRankEvent));
		BillboardSubSystem expr_12F = Globals.Instance.Player.BillboardSystem;
		expr_12F.QueryTurtleRankEvent = (BillboardSubSystem.VoidCallback)Delegate.Remove(expr_12F.QueryTurtleRankEvent, new BillboardSubSystem.VoidCallback(this.OnQueryTurtleRankEvent));
		PvpSubSystem expr_15F = Globals.Instance.Player.PvpSystem;
		expr_15F.QueryArenaDataEvent = (PvpSubSystem.VoidCallback)Delegate.Remove(expr_15F.QueryArenaDataEvent, new PvpSubSystem.VoidCallback(this.OnQueryPvpDataEvent));
		PvpSubSystem expr_18F = Globals.Instance.Player.PvpSystem;
		expr_18F.QueryArenaRankEvent = (PvpSubSystem.VoidCallback)Delegate.Remove(expr_18F.QueryArenaRankEvent, new PvpSubSystem.VoidCallback(this.OnQueryPvpRankEvent));
		WorldBossSubSystem expr_1BF = Globals.Instance.Player.WorldBossSystem;
		expr_1BF.GetBossDataEvent = (WorldBossSubSystem.VoidCallback)Delegate.Remove(expr_1BF.GetBossDataEvent, new WorldBossSubSystem.VoidCallback(this.OnGetBossDataEvent));
		LocalPlayer expr_1EA = Globals.Instance.Player;
		expr_1EA.DataInitEvent = (LocalPlayer.DataInitCallback)Delegate.Remove(expr_1EA.DataInitEvent, new LocalPlayer.DataInitCallback(this.OnGetPlayerData));
		GameUIManager.mInstance.GetTopGoods().Hide();
	}

	private void SendQueryPVPDataRequest()
	{
		MC2S_QueryArenaData ojb = new MC2S_QueryArenaData();
		Globals.Instance.CliSession.Send(801, ojb);
	}

	private void OnPvp4BillboardBtnClick()
	{
		if (this.hasInitPVPData)
		{
			this.OnQueryPvpDataEvent();
		}
		else
		{
			this.SendQueryPVPDataRequest();
			this.hasInitPVPData = true;
		}
	}

	private void OnQueryPvpDataEvent()
	{
		base.StartCoroutine(this.SendQueryPVP4RankRequest());
	}

	[DebuggerHidden]
	private IEnumerator SendQueryPVP4RankRequest()
	{
        return null;
        //return new GUIBillboard.<SendQueryPVP4RankRequest>c__Iterator68();
	}

	private void OnTrialBillboardBtnClick()
	{
		Globals.Instance.Player.BillboardSystem.SendTrialRankRequest();
	}

	private void OnGuildBillboardBtnClick()
	{
		MC2S_GuildRankData ojb = new MC2S_GuildRankData();
		Globals.Instance.CliSession.Send(958, ojb);
	}

	private void OnCombatValueBillboardBtnClick()
	{
		MC2S_CombatValueRank mC2S_CombatValueRank = new MC2S_CombatValueRank();
		mC2S_CombatValueRank.Version = Globals.Instance.Player.BillboardSystem.CombatValueRankVersion;
		Globals.Instance.CliSession.Send(284, mC2S_CombatValueRank);
	}

	private void OnPVEStarsBillboardBtnClick()
	{
		MC2S_PVEStarsRank mC2S_PVEStarsRank = new MC2S_PVEStarsRank();
		mC2S_PVEStarsRank.Version = Globals.Instance.Player.BillboardSystem.PVEStarsRankVersion;
		Globals.Instance.CliSession.Send(290, mC2S_PVEStarsRank);
	}

	private void OnLevelBillboardBtnClick()
	{
		MC2S_LevelRank mC2S_LevelRank = new MC2S_LevelRank();
		mC2S_LevelRank.Version = Globals.Instance.Player.BillboardSystem.LevelRankVersion;
		Globals.Instance.CliSession.Send(294, mC2S_LevelRank);
	}

	private void OnWorldBossBillboardBtnClick()
	{
		GUIBossReadyScene.SendGetBossDataMsg();
	}

	private void OnRoseBillboardBtnClick()
	{
		MC2S_QueryRoseRank mC2S_QueryRoseRank = new MC2S_QueryRoseRank();
		mC2S_QueryRoseRank.Version = Globals.Instance.Player.BillboardSystem.RoseRankVersion;
		Globals.Instance.CliSession.Send(1494, mC2S_QueryRoseRank);
	}

	private void OnTurtleBillboardBtnClick()
	{
		MC2S_QueryTortoiseRank mC2S_QueryTortoiseRank = new MC2S_QueryTortoiseRank();
		mC2S_QueryTortoiseRank.Version = Globals.Instance.Player.BillboardSystem.TurtleRankVersion;
		Globals.Instance.CliSession.Send(1496, mC2S_QueryTortoiseRank);
	}

	private BillboardParentBtn InitParentBtn(string name)
	{
		GameObject prefab = Res.LoadGUI("GUI/BillboardParentBtn");
		GameObject gameObject = Tools.AddChild(this.mBtnContents.gameObject, prefab);
		BillboardParentBtn billboardParentBtn = gameObject.AddComponent<BillboardParentBtn>();
		billboardParentBtn.Init(this, name);
		return billboardParentBtn;
	}

	private BillboardChildBtn InitChildBtn(string name, BillboardChildBtn.VoidCallback cb)
	{
		GameObject original = Res.LoadGUI("GUI/BillboardChildBtn");
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		BillboardChildBtn billboardChildBtn = gameObject.AddComponent<BillboardChildBtn>();
		billboardChildBtn.Init(this, name, cb);
		return billboardChildBtn;
	}

	private void OnQueryPvpRankEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_PVP4, -1);
	}

	private void OnQueryTrialRankEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_Trial, -1);
	}

	private void OnQueryGuildRankEvent(int selfRank)
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_Guild, selfRank);
	}

	private void OnQueryCombatValueRankEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_CombatValue, -1);
	}

	private void OnQueryRoseRankEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_Rose, -1);
	}

	private void OnQueryTurtleRankEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_Turtle, -1);
	}

	private void OnQueryPVEStarsRankEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_PVEStars, -1);
	}

	private void OnQueryLevelRankEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_Level, -1);
	}

	private void OnGetBossDataEvent()
	{
		this.layer.Refresh(BillboardCommonLayer.BillboardType.BT_WorldBoss, -1);
	}
}
