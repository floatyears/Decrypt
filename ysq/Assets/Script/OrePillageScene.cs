using Proto;
using ProtoBuf;
using System;
using System.IO;

public sealed class OrePillageScene : PvpScene
{
	public OrePillageScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		base.Init();
		Globals.Instance.CliSession.Register(1029, new ClientSession.MsgHandler(this.HandleOrePillageResult));
	}

	public override void Destroy()
	{
		base.Destroy();
		Globals.Instance.CliSession.Unregister(1029, new ClientSession.MsgHandler(this.HandleOrePillageResult));
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
		MC2S_OrePillageResult mC2S_OrePillageResult = new MC2S_OrePillageResult();
		if (this.win)
		{
			mC2S_OrePillageResult.ResultKey = (this.actorMgr.Key ^ 2014);
		}
		else
		{
			mC2S_OrePillageResult.ResultKey = (this.actorMgr.Key ^ 2010);
		}
		mC2S_OrePillageResult.Log = this.actorMgr.GetCombatLog();
		Globals.Instance.CliSession.Send(1028, mC2S_OrePillageResult);
	}

	public void HandleOrePillageResult(MemoryStream stream)
	{
		MS2C_OrePillageResult mS2C_OrePillageResult = Serializer.NonGeneric.Deserialize(typeof(MS2C_OrePillageResult), stream) as MS2C_OrePillageResult;
		if (mS2C_OrePillageResult.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EGR", mS2C_OrePillageResult.Result);
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
		Globals.Instance.SenceMgr.CloseScene();
		GUIGuildMinesResultScene.Show(this.win, mS2C_OrePillageResult);
	}
}
