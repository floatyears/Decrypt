using Att;
using System;
using UnityEngine;

public sealed class StartScene : WorldScene
{
	private Tutorial_StartScene tutorial;

	private SocketDataEx playerSocket;

	private int fashionID;

	public Tutorial_StartScene Tutorial
	{
		get
		{
			return this.tutorial;
		}
	}

	public StartScene(ActorManager actorManager) : base(actorManager)
	{
	}

	public override void Update(float elapse)
	{
		base.Update(elapse);
	}

	public override void Destroy()
	{
		base.Destroy();
		this.tutorial = null;
		if (this.playerSocket != null)
		{
			this.playerSocket.EquipFashion(this.fashionID);
		}
	}

	public override void OnLoadRespawnOK()
	{
		this.CreateActors();
		base.RespawnMonster();
	}

	public void CreateActors()
	{
		Quaternion rotation = Quaternion.Euler(0f, this.actorMgr.BornRotationY, 0f);
		PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(10180);
		if (info != null)
		{
			this.actorMgr.Actors[3] = this.actorMgr.CreateAssist(10180, 29017, this.actorMgr.BornPosition, rotation, false);
			if (this.actorMgr.Actors[3] != null)
			{
				this.actorMgr.Actors[3].AddSkill(0, info.SkillID[0], true);
				this.actorMgr.Actors[3].AddSkill(1, 403101, true);
			}
		}
		info = Globals.Instance.AttDB.PetDict.GetInfo(10187);
		if (info != null)
		{
			this.actorMgr.Actors[2] = this.actorMgr.CreateAssist(10187, 29018, this.actorMgr.BornPosition, rotation, false);
			if (this.actorMgr.Actors[2] != null)
			{
				this.actorMgr.Actors[2].AddSkill(0, info.SkillID[0], true);
				this.actorMgr.Actors[2].AddSkill(1, 402701, true);
			}
		}
		info = Globals.Instance.AttDB.PetDict.GetInfo(10181);
		if (info != null)
		{
			this.actorMgr.Actors[1] = this.actorMgr.CreateAssist(10181, 29016, this.actorMgr.BornPosition, rotation, false);
			if (this.actorMgr.Actors[1] != null)
			{
				this.actorMgr.Actors[1].AddSkill(0, info.SkillID[0], true);
				this.actorMgr.Actors[1].AddSkill(1, 401901, true);
			}
		}
		int num;
		if (Globals.Instance.Player.Data.Gender == 0)
		{
			num = 105;
		}
		else
		{
			num = 205;
		}
		this.playerSocket = Globals.Instance.Player.TeamSystem.GetSocket(0);
		if (this.playerSocket == null)
		{
			global::Debug.LogError(new object[]
			{
				"GetSocket 0 error"
			});
			return;
		}
		this.fashionID = this.playerSocket.GetFashionID();
		this.playerSocket.EquipFashion(num);
		ActorController actorController = this.actorMgr.CreateLocalActor(0, this.actorMgr.BornPosition, rotation);
		if (actorController == null)
		{
			global::Debug.LogError(new object[]
			{
				"CreatePlayer error"
			});
			return;
		}
		actorController.ResetAtt(29018);
		actorController.MaxMP = actorController.MaxHP / 5L;
		actorController.CurMP = actorController.MaxMP;
		this.actorMgr.Actors[0] = actorController;
		actorController.Undead = true;
		actorController.AddSkill(0, 16, true);
		actorController.AddSkill(1, 101605, true);
		actorController.AddSkill(2, 104605, true);
		actorController.AddSkill(3, 104405, true);
		this.actorMgr.ResetAI();
	}

	public override void OnUILoaded()
	{
		GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
		if (session != null)
		{
			session.mOptionalLayer.gameObject.SetActive(false);
			session.mGameController.gameObject.SetActive(false);
		}
	}

	public override void OnPreStart()
	{
		if (GameUIManager.mInstance.ShowPlotDialog(1001, new GUIPlotDialog.FinishCallback(base.DialogFinish), null))
		{
			this.actorMgr.Pause(true);
		}
	}

	public override void OnStart()
	{
		this.status = 2;
		this.stopTimer = true;
		GameObject gameObject = new GameObject("Tutorial");
		this.tutorial = gameObject.AddComponent<Tutorial_StartScene>();
		this.tutorial.GuideMove();
		if (GameCache.Data.EnableAI)
		{
			GameCache.Data.EnableAI = false;
			GameCache.UpdateNow = true;
		}
	}

	public override void OnPlayWinOver()
	{
		Globals.Instance.SenceMgr.CloseScene();
		GameUIManager.mInstance.ChangeSession<GUIMainMenuScene>(null, false, true);
		GameUIManager.mInstance.ClearGobackSession();
	}

	public override float GetPreStartDelay()
	{
		return 0.5f;
	}

	public override float GetStartDelay()
	{
		return 0.5f;
	}
}
