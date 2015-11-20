using Att;
using Proto;
using System;
using UnityEngine;

public abstract class BaseScene
{
	public enum ESceneStatus
	{
		ESS_None,
		ESS_Init,
		ESS_Gaming,
		ESS_Win,
		ESS_Dead,
		ESS_TimeUp
	}

	protected ActorManager actorMgr;

	protected SceneInfo senceInfo;

	protected int status;

	protected float timer;

	protected float combatTimer;

	protected float maxCombatTimer = 180f;

	protected bool stopTimer;

	public int CurStatus
	{
		get
		{
			return this.status;
		}
	}

	public BaseScene(ActorManager actorManager)
	{
		this.actorMgr = actorManager;
	}

	public void SetSceneInfo(SceneInfo info)
	{
		this.senceInfo = info;
	}

	public virtual void Init()
	{
	}

	public virtual void Update(float elapse)
	{
	}

	public virtual void Destroy()
	{
	}

	public virtual void OnLoadRespawnPoint(int infoID, int groupID, Vector3 position, float rotationY, Vector3 scale)
	{
	}

	public virtual void OnLoadRespawnOK()
	{
	}

	public virtual void OnUILoaded()
	{
	}

	public virtual void OnPreStart()
	{
	}

	public virtual void OnStart()
	{
	}

	public virtual void OnChangeAIMode()
	{
	}

	public virtual void OnPlayerFindEnemy(ActorController actor)
	{
	}

	public virtual void OnPlayerDead()
	{
	}

	public virtual void OnPetDead()
	{
	}

	public virtual void OnRemoteActorDead()
	{
	}

	public virtual void OnAllMonsterDead()
	{
	}

	public virtual void OnPlayWinOver()
	{
	}

	public virtual void OnDoDamage(MonsterInfo info, long damage)
	{
	}

	public virtual void OnPlayerResurrect()
	{
		this.stopTimer = false;
		this.status = 2;
	}

	public virtual void OnSkip(bool result)
	{
	}

	public virtual float GetPreStartDelay()
	{
		return 1f;
	}

	public virtual float GetStartDelay()
	{
		return 1.5f;
	}

	public virtual ActorController GetRemotePlayerActor()
	{
		return null;
	}

	public virtual float GetCombatTimer()
	{
		if (this.combatTimer < 0f)
		{
			return 0f;
		}
		return this.combatTimer;
	}

	public virtual float GetMaxCombatTimer()
	{
		return this.maxCombatTimer;
	}

	protected virtual void ShowDeadUI()
	{
		GameUIManager.mInstance.uiState.CurSceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		this.SendFailLog(GameUIManager.mInstance.uiState.CurSceneInfo);
		GameAnalytics.OnFailScene(GameUIManager.mInstance.uiState.CurSceneInfo, GameAnalytics.ESceneFailed.CombatEffectiveness);
		Globals.Instance.SenceMgr.CloseScene();
		GameUIManager.mInstance.ChangeSession<GUIGameResultFailureScene>(null, false, false);
	}

	public virtual void OnFailed()
	{
		GameUIManager.mInstance.ShowMessageTipByKey("PveR_6", 0f, 0f);
		SceneInfo sceneInfo = Globals.Instance.SenceMgr.sceneInfo;
		GameAnalytics.OnFailScene(sceneInfo, GameAnalytics.ESceneFailed.UIClose);
		Globals.Instance.SenceMgr.CloseScene();
		switch (sceneInfo.Type)
		{
		case 0:
			GameUIManager.mInstance.ChangeSession<GUIWorldMap>(null, true, true);
			break;
		case 1:
			GameUIManager.mInstance.ChangeSession<GUITrailTowerSceneV2>(null, true, true);
			break;
		case 2:
			GameUIManager.mInstance.ChangeSession<GUIPVP4ReadyScene>(null, true, true);
			break;
		case 3:
			GUIWorldBossVictoryScene.BackBossScene();
			break;
		case 4:
			GameUIManager.mInstance.ChangeSession<GUIPillageScene>(null, true, true);
			break;
		case 5:
			GameUIManager.mInstance.ChangeSession<GUIGuildSchoolScene>(null, true, true);
			break;
		case 6:
			GameUIManager.mInstance.ChangeSession<GUIKingRewardScene>(null, true, true);
			break;
		case 7:
			GUIGuardScene.Show(true);
			break;
		case 8:
			GUIGuildMinesScene.Show(true);
			break;
		}
	}

	public void SendFailLog(SceneInfo sInfo)
	{
		if (sInfo == null)
		{
			return;
		}
		MC2S_CombatLog mC2S_CombatLog = new MC2S_CombatLog();
		int type = sInfo.Type;
		if (type != 6)
		{
			if (type != 7)
			{
				if (type != 0)
				{
					return;
				}
				mC2S_CombatLog.Type = 3;
			}
			else
			{
				mC2S_CombatLog.Type = 5;
			}
		}
		else
		{
			mC2S_CombatLog.Type = 4;
		}
		mC2S_CombatLog.Log = this.actorMgr.GetCombatLog();
		Globals.Instance.CliSession.SendPacket(649, mC2S_CombatLog);
	}
}
