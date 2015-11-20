using Proto;
using ProtoBuf;
using System;
using System.IO;

public sealed class PillageScene : PvpScene
{
	public PillageScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Init()
	{
		base.Init();
		Globals.Instance.CliSession.Register(819, new ClientSession.MsgHandler(this.HandlePvpPillageResult));
	}

	public override void Destroy()
	{
		base.Destroy();
		Globals.Instance.CliSession.Unregister(819, new ClientSession.MsgHandler(this.HandlePvpPillageResult));
	}

	public override void OnStart()
	{
		this.status = 2;
		for (int i = 0; i < 4; i++)
		{
			if (this.actorMgr.Actors[i] != null)
			{
				this.actorMgr.Actors[i].AiCtrler.Locked = false;
				this.actorMgr.Actors[i].AiCtrler.EnableAI = GameCache.Data.EnableAI;
			}
			if (this.remoteActor[i] != null)
			{
				this.remoteActor[i].AiCtrler.Locked = false;
				this.remoteActor[i].AiCtrler.EnableAI = true;
			}
		}
		this.actorMgr.Actors[0].AiCtrler.AssistSkill = 0;
		MC2S_PvpCombatStart ojb = new MC2S_PvpCombatStart();
		Globals.Instance.CliSession.Send(805, ojb);
	}

	public override void OnLoadRespawnOK()
	{
		base.OnLoadRespawnOK();
		base.HpMpMod();
		int num = 0;
		int num2 = 0;
		for (int i = 1; i < 5; i++)
		{
			if (!(this.actorMgr.Actors[i] == null))
			{
				if (this.actorMgr.Actors[i].IsMelee)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		int num3 = 0;
		int num4 = 0;
		for (int j = 1; j < 5; j++)
		{
			if (!(this.actorMgr.Actors[j] == null))
			{
				int fellowSlot;
				if (j >= 4)
				{
					fellowSlot = 7;
				}
				else if (this.actorMgr.Actors[j].IsMelee)
				{
					num3++;
					fellowSlot = 100 + num * 10 + num3;
				}
				else
				{
					num4++;
					fellowSlot = 200 + num2 * 10 + num4;
				}
				this.actorMgr.Actors[j].AiCtrler.SetFellowSlot(fellowSlot);
			}
		}
	}

	public override void SendPvpResultMsg()
	{
		MC2S_PvpPillageResult mC2S_PvpPillageResult = new MC2S_PvpPillageResult();
		if (this.win)
		{
			mC2S_PvpPillageResult.ResultKey = (this.actorMgr.Key ^ 2014);
		}
		else
		{
			mC2S_PvpPillageResult.ResultKey = (this.actorMgr.Key ^ 2010);
		}
		mC2S_PvpPillageResult.Log = this.actorMgr.GetCombatLog();
		Globals.Instance.CliSession.Send(818, mC2S_PvpPillageResult);
	}

	public void HandlePvpPillageResult(MemoryStream stream)
	{
		MS2C_PvpPillageResult mS2C_PvpPillageResult = Serializer.NonGeneric.Deserialize(typeof(MS2C_PvpPillageResult), stream) as MS2C_PvpPillageResult;
		if (mS2C_PvpPillageResult.Result != 0)
		{
			GameUIManager.mInstance.ShowMessageTip("EPVPR", mS2C_PvpPillageResult.Result);
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
		GUIPillageResultScene.ShowPillageResult(this.win, mS2C_PvpPillageResult);
	}
}
