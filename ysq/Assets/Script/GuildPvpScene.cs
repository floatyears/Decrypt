using Att;
using Proto;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;

public sealed class GuildPvpScene : PvpScene
{
	private long myTotalHP;

	private long enemyTotalHP;

	public GuildPvpScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		base.Init();
		this.myTotalHP = 0L;
		this.enemyTotalHP = 0L;
		this.maxCombatTimer = 90f;
		this.combatTimer = this.maxCombatTimer;
		Globals.Instance.CliSession.Register(988, new ClientSession.MsgHandler(this.HandleGuildWarFightEnd));
	}

	public override void Destroy()
	{
		base.Destroy();
		Globals.Instance.CliSession.ShowReconnect(false);
		Globals.Instance.CliSession.Unregister(988, new ClientSession.MsgHandler(this.HandleGuildWarFightEnd));
	}

	public override void OnStart()
	{
		base.OnStart();
		this.actorMgr.PlayerCtrler.SetControlLocked(true);
		Globals.Instance.CliSession.ShowReconnect(true);
		MC2S_GuildWarCombatStart mC2S_GuildWarCombatStart = new MC2S_GuildWarCombatStart();
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold != null)
		{
			mC2S_GuildWarCombatStart.StrongholdID = strongHold.ID;
		}
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData != null)
		{
			mC2S_GuildWarCombatStart.WarID = mGWEnterData.WarID;
		}
		mC2S_GuildWarCombatStart.TeamID = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		mC2S_GuildWarCombatStart.SlotIndex = GameUIManager.mInstance.uiState.GuildWarFightSlotIndex;
		Globals.Instance.CliSession.Send(985, mC2S_GuildWarCombatStart);
	}

	public override void OnLoadRespawnOK()
	{
		base.OnLoadRespawnOK();
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		GuildInfo info = Globals.Instance.AttDB.GuildDict.GetInfo(GameUIManager.mInstance.uiState.GuildWarFightHoldIndex);
		if (info != null)
		{
			int strongholdBuffID = info.StrongholdBuffID;
			if (strongholdBuffID != 0)
			{
				for (int i = 4; i < actors.Count; i++)
				{
					if (actors[i] != null)
					{
						actors[i].AddBuff(strongholdBuffID, actors[i]);
					}
				}
			}
		}
		int myHpPct = Globals.Instance.Player.GuildSystem.MyHpPct;
		int enemyHpPct = Globals.Instance.Player.GuildSystem.EnemyHpPct;
		for (int j = 0; j < 4; j++)
		{
			if (actors[j] != null)
			{
				this.myTotalHP += actors[j].MaxHP;
				long curHP = actors[j].MaxHP * (long)myHpPct / 10000L;
				actors[j].CurHP = curHP;
			}
		}
		for (int k = 4; k < actors.Count; k++)
		{
			if (actors[k] != null)
			{
				this.enemyTotalHP += actors[k].MaxHP;
				long curHP = actors[k].MaxHP * (long)enemyHpPct / 10000L;
				actors[k].CurHP = curHP;
			}
		}
	}

	public override void SendPvpResultMsg()
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		long num = 0L;
		MC2S_GuildWarFightEnd mC2S_GuildWarFightEnd = new MC2S_GuildWarFightEnd();
		long num2;
		if (this.win)
		{
			mC2S_GuildWarFightEnd.ResultKey = (this.actorMgr.Key ^ 2014);
			for (int i = 0; i < 4; i++)
			{
				if (actors[i] != null && !actors[i].IsDead)
				{
					num += actors[i].CurHP;
				}
			}
			num2 = this.myTotalHP;
		}
		else
		{
			mC2S_GuildWarFightEnd.ResultKey = (this.actorMgr.Key ^ 2010);
			for (int j = 4; j < actors.Count; j++)
			{
				if (actors[j] != null && !actors[j].IsDead)
				{
					num += actors[j].CurHP;
				}
			}
			num2 = this.enemyTotalHP;
		}
		mC2S_GuildWarFightEnd.Log = this.actorMgr.GetCombatLog();
		if (num > num2)
		{
			num = num2;
		}
		mC2S_GuildWarFightEnd.HealthPct = (int)(num * 10000L / num2);
		GuildWarStronghold strongHold = Globals.Instance.Player.GuildSystem.StrongHold;
		if (strongHold != null)
		{
			mC2S_GuildWarFightEnd.StrongholdID = strongHold.ID;
		}
		GuildWarClient mGWEnterData = Globals.Instance.Player.GuildSystem.mGWEnterData;
		if (mGWEnterData != null)
		{
			mC2S_GuildWarFightEnd.WarID = mGWEnterData.WarID;
		}
		mC2S_GuildWarFightEnd.TeamID = Globals.Instance.Player.GuildSystem.GetSelfTeamFlag();
		mC2S_GuildWarFightEnd.SlotIndex = GameUIManager.mInstance.uiState.GuildWarFightSlotIndex;
		Globals.Instance.CliSession.Send(987, mC2S_GuildWarFightEnd);
	}

	public void HandleGuildWarFightEnd(MemoryStream stream)
	{
		MS2C_GuildWarFightEnd mS2C_GuildWarFightEnd = Serializer.NonGeneric.Deserialize(typeof(MS2C_GuildWarFightEnd), stream) as MS2C_GuildWarFightEnd;
		if (mS2C_GuildWarFightEnd.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_GuildWarFightEnd.Result);
			this.win = false;
		}
		GameUIManager.mInstance.uiState.CurSceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		if (this.win)
		{
			GameAnalytics.OnFinishScene(Globals.Instance.SenceMgr.sceneInfo);
		}
		else
		{
			GameAnalytics.OnFailScene(Globals.Instance.SenceMgr.sceneInfo, GameAnalytics.ESceneFailed.CombatEffectiveness);
		}
		Globals.Instance.CliSession.ShowReconnect(false);
		Globals.Instance.SenceMgr.CloseScene();
		if (Globals.Instance.Player.GuildSystem.LocalClientMember != null)
		{
			Globals.Instance.Player.GuildSystem.LocalClientMember.Member = mS2C_GuildWarFightEnd.Player;
		}
		GUIGuildCraftResultScene.ShowMe(this.win, mS2C_GuildWarFightEnd);
	}
}
