using Proto;
using ProtoBuf;
using System;
using System.IO;

public sealed class ArenaScene : PvpScene
{
	public ArenaScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		base.Init();
		Globals.Instance.CliSession.Register(807, new ClientSession.MsgHandler(this.HandlePvpArenaResult));
	}

	public override void Destroy()
	{
		base.Destroy();
		Globals.Instance.CliSession.Unregister(807, new ClientSession.MsgHandler(this.HandlePvpArenaResult));
	}

	public override void OnStart()
	{
		base.OnStart();
		this.actorMgr.PlayerCtrler.SetControlLocked(true);
	}

	public override void OnLoadRespawnOK()
	{
		base.OnLoadRespawnOK();
		base.HpMpMod();
	}

	public override void SendPvpResultMsg()
	{
		MC2S_PvpArenaResult mC2S_PvpArenaResult = new MC2S_PvpArenaResult();
		if (this.win)
		{
			mC2S_PvpArenaResult.ResultKey = (this.actorMgr.Key ^ 2014);
		}
		else
		{
			mC2S_PvpArenaResult.ResultKey = (this.actorMgr.Key ^ 2010);
		}
		mC2S_PvpArenaResult.Log = this.actorMgr.GetCombatLog();
		Globals.Instance.CliSession.Send(806, mC2S_PvpArenaResult);
	}

	public void HandlePvpArenaResult(MemoryStream stream)
	{
		MS2C_PvpArenaResult mS2C_PvpArenaResult = Serializer.NonGeneric.Deserialize(typeof(MS2C_PvpArenaResult), stream) as MS2C_PvpArenaResult;
		if (mS2C_PvpArenaResult.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpArenaResult.Result);
			this.win = false;
		}
		if (mS2C_PvpArenaResult.HighestRank > 0)
		{
			Globals.Instance.Player.Data.ArenaHighestRank = mS2C_PvpArenaResult.HighestRank;
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
		Globals.Instance.SenceMgr.CloseScene();
		GUIPVPResultScene.ShowArenaResult(this.win, mS2C_PvpArenaResult);
	}
}
