using Att;
using NtUniSdk.Unity3d;
using Proto;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CommandParser
{
	public class Command
	{
		private string[] tokens;

		public Command(string cmdStr)
		{
			this.tokens = cmdStr.Split(new char[0]);
		}

		public string GetName()
		{
			if (this.tokens.Length == 0)
			{
				return string.Empty;
			}
			return this.tokens[0].ToLower();
		}

		public string GetParam(int index)
		{
			if (index >= 0 && index < this.tokens.Length)
			{
				return this.tokens[index];
			}
			return string.Empty;
		}
	}

	public delegate void CallBackFunc(CommandParser.Command param);

	private static CommandParser instance;

	private Dictionary<string, CommandParser.CallBackFunc> mRegistedCmds = new Dictionary<string, CommandParser.CallBackFunc>();

	public static CommandParser Instance
	{
		get
		{
			if (CommandParser.instance == null)
			{
				CommandParser.instance = new CommandParser();
				CommandParser.instance.RegisterCmds();
			}
			return CommandParser.instance;
		}
	}

	private void RegisterCmds()
	{
		this.RegisterCmd("fps", new CommandParser.CallBackFunc(this.HandleShowHideFpsCmd));
		this.RegisterCmd("Skill", new CommandParser.CallBackFunc(this.HandleSkillCmd));
		this.RegisterCmd("Summon", new CommandParser.CallBackFunc(this.HandleSummonCmd));
		this.RegisterCmd("AddBuff", new CommandParser.CallBackFunc(this.HandleAddBuffCmd));
		this.RegisterCmd("Dead", new CommandParser.CallBackFunc(this.HandleDeadCmd));
		this.RegisterCmd("ClearCD", new CommandParser.CallBackFunc(this.HandleClearCDCmd));
		this.RegisterCmd("TakeDayEnergy", new CommandParser.CallBackFunc(this.HandleTakeDayEnergyCmd));
		this.RegisterCmd("TakeQuestReward", new CommandParser.CallBackFunc(this.HandleTakeQuestRewardCmd));
		this.RegisterCmd("GetMailData", new CommandParser.CallBackFunc(this.HandleGetMailDataCmd));
		this.RegisterCmd("TakeMailAffix", new CommandParser.CallBackFunc(this.HandleTakeMailAffixCmd));
		this.RegisterCmd("Chat", new CommandParser.CallBackFunc(this.HandleChatCmd));
		this.RegisterCmd("Diamond2Money", new CommandParser.CallBackFunc(this.HandleDiamond2MoneyCmd));
		this.RegisterCmd("LuckyRoll", new CommandParser.CallBackFunc(this.HandleLuckyRollCmd));
		this.RegisterCmd("SignIn", new CommandParser.CallBackFunc(this.HandleSignIn));
		this.RegisterCmd("TakeLevelReward", new CommandParser.CallBackFunc(this.HandleTakeLevelRewardCmd));
		this.RegisterCmd("TakeCardDiamond", new CommandParser.CallBackFunc(this.HandleTakeCardDiamondCmd));
		this.RegisterCmd("ChangeName", new CommandParser.CallBackFunc(this.HandleChangeNameCmd));
		this.RegisterCmd("Dialog", new CommandParser.CallBackFunc(this.HandleDialogCmd));
		this.RegisterCmd("ChangeForceFollow", new CommandParser.CallBackFunc(this.HandleChangeForceFollowCmd));
		this.RegisterCmd("ShowReconnect", new CommandParser.CallBackFunc(this.HandleShowReconnectCmd));
		this.RegisterCmd("GetBoss", new CommandParser.CallBackFunc(this.HandleGetBossDataCmd));
		this.RegisterCmd("EnterWS", new CommandParser.CallBackFunc(this.HandleEnterWSCmd));
		this.RegisterCmd("TakeWReward", new CommandParser.CallBackFunc(this.HandleTakeWRewardCmd));
		this.RegisterCmd("CloseSession", new CommandParser.CallBackFunc(this.HandleCloseSessionCmd));
		this.RegisterCmd("Reconnect", new CommandParser.CallBackFunc(this.HandleReconnectCmd));
		this.RegisterCmd("TrialStart", new CommandParser.CallBackFunc(this.HandleTrialStartCmd));
		this.RegisterCmd("TrialReset", new CommandParser.CallBackFunc(this.HandleTrialResetCmd));
		this.RegisterCmd("TrialFarmStart", new CommandParser.CallBackFunc(this.HandleTrialFarmStartCmd));
		this.RegisterCmd("TrialFarmStop", new CommandParser.CallBackFunc(this.HandleTrialFarmStopCmd));
		this.RegisterCmd("EnterScene", new CommandParser.CallBackFunc(this.HandleEnterSceneCmd));
		this.RegisterCmd("EnterTutorial", new CommandParser.CallBackFunc(this.HandleEnterTutorialCmd));
		this.RegisterCmd("CloseTutorial", new CommandParser.CallBackFunc(this.HandleCloseTutorialCmd));
		this.RegisterCmd("RemoveTutorial", new CommandParser.CallBackFunc(this.HandleRemoveTutorialCmd));
		this.RegisterCmd("PassTutorial", new CommandParser.CallBackFunc(this.HandlePassTutorialCmd));
		this.RegisterCmd("Door", new CommandParser.CallBackFunc(this.HandleDoorCmd));
		this.RegisterCmd("AttackSpeedMod", new CommandParser.CallBackFunc(this.HandleAttackSpeedModCmd));
		this.RegisterCmd("Pause", new CommandParser.CallBackFunc(this.HandlePauseCmd));
		this.RegisterCmd("QueryAtt", new CommandParser.CallBackFunc(this.HandleQueryAttCmd));
		this.RegisterCmd("AddDiamond", new CommandParser.CallBackFunc(this.HandleAddDiamond));
		this.RegisterCmd("AddEnergy", new CommandParser.CallBackFunc(this.HandleAddEnergy));
		this.RegisterCmd("AddMoney", new CommandParser.CallBackFunc(this.HandleAddMoney));
		this.RegisterCmd("ResetSceneCD", new CommandParser.CallBackFunc(this.HandleResetSceneCD));
		this.RegisterCmd("AddHP", new CommandParser.CallBackFunc(this.HandleAddHP));
		this.RegisterCmd("PetDead", new CommandParser.CallBackFunc(this.HandlePetDeadCmd));
		this.RegisterCmd("q", new CommandParser.CallBackFunc(this.HandleTestCmd));
		this.RegisterCmd("CheckPayResult", new CommandParser.CallBackFunc(this.HandleCheckPayResultCmd));
		this.RegisterCmd("ExchangeGiftCode", new CommandParser.CallBackFunc(this.HandleExchangeGiftCodeCmd));
		this.RegisterCmd("ChangeSocket", new CommandParser.CallBackFunc(this.HandleChangeSocketCmd));
		this.RegisterCmd("SetCombatPet", new CommandParser.CallBackFunc(this.HandleSetCombatPetCmd));
		this.RegisterCmd("EquipItem", new CommandParser.CallBackFunc(this.HandleEquipItemCmd));
		this.RegisterCmd("ShowAtt", new CommandParser.CallBackFunc(this.HandleShowAttCmd));
		this.RegisterCmd("PetLevelup", new CommandParser.CallBackFunc(this.HandlePetLevelupCmd));
		this.RegisterCmd("PetFurther", new CommandParser.CallBackFunc(this.HandlePetFurtherCmd));
		this.RegisterCmd("EquipEnhance", new CommandParser.CallBackFunc(this.HandleItemEnhanceCmd));
		this.RegisterCmd("EquipFefine", new CommandParser.CallBackFunc(this.HandleEquipFefineCmd));
		this.RegisterCmd("EquipEnhance", new CommandParser.CallBackFunc(this.HandleTrinketEnhanceCmd));
		this.RegisterCmd("EquipFefine", new CommandParser.CallBackFunc(this.HandleTrinketFefineCmd));
		this.RegisterCmd("EquipCreate", new CommandParser.CallBackFunc(this.HandleEquipCreateCmd));
		this.RegisterCmd("TrinketCreate", new CommandParser.CallBackFunc(this.HandleTrinketCreateCmd));
		this.RegisterCmd("EquipBreakUp", new CommandParser.CallBackFunc(this.HandleEquipBreakUpCmd));
		this.RegisterCmd("TrinketReborn", new CommandParser.CallBackFunc(this.HandleTrinketRebornCmd));
		this.RegisterCmd("PetSkill", new CommandParser.CallBackFunc(this.HandlePetSkillCmd));
		this.RegisterCmd("PetReborn", new CommandParser.CallBackFunc(this.HandlePetRebornCmd));
		this.RegisterCmd("PetBreakUp", new CommandParser.CallBackFunc(this.HandlePetBreakUpCmd));
		this.RegisterCmd("QueryArena", new CommandParser.CallBackFunc(this.HandleQueryArenaCmd));
		this.RegisterCmd("ArenaStart", new CommandParser.CallBackFunc(this.HandleArenaStartCmd));
		this.RegisterCmd("QueryPillage", new CommandParser.CallBackFunc(this.HandleQueryPillageCmd));
		this.RegisterCmd("PillageStart", new CommandParser.CallBackFunc(this.HandlePillageStartCmd));
		this.RegisterCmd("ConLevelup", new CommandParser.CallBackFunc(this.HandleConLevelupCmd));
		this.RegisterCmd("TakeAchievementReward", new CommandParser.CallBackFunc(this.HandleTakeAchievementRewardCmd));
		this.RegisterCmd("QueryRemotePlayer", new CommandParser.CallBackFunc(this.HandleQueryRemotePlayerCmd));
		this.RegisterCmd("TakeDailyScoreReward", new CommandParser.CallBackFunc(this.HandleTakeDailyScoreRewardCmd));
		this.RegisterCmd("ShowPet", new CommandParser.CallBackFunc(this.HandleShowPetCmd));
		this.RegisterCmd("TakeSevenDayReward", new CommandParser.CallBackFunc(this.HandleTakeSevenDayRewardCmd));
		this.RegisterCmd("ShareAchievement", new CommandParser.CallBackFunc(this.HandleShareAchievementCmd));
		this.RegisterCmd("TakeShareAchievementReward", new CommandParser.CallBackFunc(this.HandleTakeShareAchievementRewardCmd));
		this.RegisterCmd("BuyFund", new CommandParser.CallBackFunc(this.HandleBuyFundCmd));
		this.RegisterCmd("TakeFundLevelReward", new CommandParser.CallBackFunc(this.HandleTakeFundLevelRewardCmd));
		this.RegisterCmd("TakeWelfare", new CommandParser.CallBackFunc(this.HandleTakeWelfareCmd));
		this.RegisterCmd("Undead", new CommandParser.CallBackFunc(this.HandleUndeadCmd));
		this.RegisterCmd("ChangeMemory", new CommandParser.CallBackFunc(this.HandleChangeMemoryCmd));
		this.RegisterCmd("ShowIndicate", new CommandParser.CallBackFunc(this.HandleShowIndicateCmd));
		this.RegisterCmd("TakeDayHotReward", new CommandParser.CallBackFunc(this.HandleTakeDayHotRewardCmd));
		this.RegisterCmd("EquipAwakeItem", new CommandParser.CallBackFunc(this.HandleEquipAwakeItemCmd));
		this.RegisterCmd("AwakeLevelup", new CommandParser.CallBackFunc(this.HandleAwakeLevelupCmd));
		this.RegisterCmd("AwakeItemCreate", new CommandParser.CallBackFunc(this.HandleAwakeItemCreateCmd));
		this.RegisterCmd("AwakeItemBreakUp", new CommandParser.CallBackFunc(this.HandleAwakeItemBreakUpCmd));
		this.RegisterCmd("BuyActivityPayShopItem", new CommandParser.CallBackFunc(this.HandleBuyActivityPayShopItemCmd));
		this.RegisterCmd("ShowStats", new CommandParser.CallBackFunc(this.HandleShowStatsCmd));
		this.RegisterCmd("SkipCombat", new CommandParser.CallBackFunc(this.HandleSkipCombatCmd));
		this.RegisterCmd("GetFamePlayers", new CommandParser.CallBackFunc(this.HandleGetFamePlayersCmd));
		this.RegisterCmd("PraisePlayer", new CommandParser.CallBackFunc(this.HandlePraisePlayerCmd));
		this.RegisterCmd("SetFameMessage", new CommandParser.CallBackFunc(this.HandleSetFameMessageCmd));
		this.RegisterCmd("RecommendFriend", new CommandParser.CallBackFunc(this.HandleRecommendFriendCmd));
		this.RegisterCmd("RequestFriend", new CommandParser.CallBackFunc(this.HandleRequestFriendCmd));
		this.RegisterCmd("ReplyFriend", new CommandParser.CallBackFunc(this.HandleReplyFriendCmd));
		this.RegisterCmd("RemoveFriend", new CommandParser.CallBackFunc(this.HandleRemoveFriendCmd));
		this.RegisterCmd("AddBlackList", new CommandParser.CallBackFunc(this.HandleAddBlackListCmd));
		this.RegisterCmd("RemoveBlackList", new CommandParser.CallBackFunc(this.HandleRemoveBlackListCmd));
		this.RegisterCmd("GiveFriendEnergy", new CommandParser.CallBackFunc(this.HandleGiveFriendEnergyCmd));
		this.RegisterCmd("TakeFriendEnergy", new CommandParser.CallBackFunc(this.HandleTakeFriendEnergyCmd));
		this.RegisterCmd("SummonTower", new CommandParser.CallBackFunc(this.HandleSummonTowerCmd));
		this.RegisterCmd("ShowBossAtt", new CommandParser.CallBackFunc(this.HandleShowBossAttCmd));
		this.RegisterCmd("TakePayReward", new CommandParser.CallBackFunc(this.HandleTakePayRewardCmd));
		this.RegisterCmd("DestroyAllTower", new CommandParser.CallBackFunc(this.HandleDestroyAllTowerCmd));
		this.RegisterCmd("ShowBuffCount", new CommandParser.CallBackFunc(this.HandleShowBuffCountCmd));
		this.RegisterCmd("GetItemCount", new CommandParser.CallBackFunc(this.HandleGetItemCountCmd));
		this.RegisterCmd("PetExchange", new CommandParser.CallBackFunc(this.HandlePetExchangeCmd));
		this.RegisterCmd("AddEP", new CommandParser.CallBackFunc(this.HandleAddEPCmd));
		this.RegisterCmd("PetChangeModel", new CommandParser.CallBackFunc(this.HandlePetChangeModelCmd));
		this.RegisterCmd("UploadVoice", new CommandParser.CallBackFunc(this.HandleUploadVoiceCmd));
		this.RegisterCmd("GetVoice", new CommandParser.CallBackFunc(this.HandleGetVoiceCmd));
		this.RegisterCmd("TranslateVoice", new CommandParser.CallBackFunc(this.HandleTranslateVoiceCmd));
	}

	private void UnregisterCmds()
	{
		this.mRegistedCmds.Clear();
	}

	private void RegisterCmd(string cmd, CommandParser.CallBackFunc callback)
	{
		cmd = cmd.ToLower();
		if (!this.mRegistedCmds.ContainsKey(cmd))
		{
			this.mRegistedCmds.Add(cmd, callback);
		}
	}

	private void UnregisterCmd(string cmd)
	{
		cmd = cmd.ToLower();
		if (this.mRegistedCmds.ContainsKey(cmd))
		{
			this.mRegistedCmds.Remove(cmd);
		}
	}

	public bool Parse(string commandStr)
	{
		if (string.IsNullOrEmpty(commandStr))
		{
			return false;
		}
		if (Globals.Instance.CliSession.Privilege <= 0)
		{
			return false;
		}
		string text = commandStr.Trim();
		if (text.Length < 2)
		{
			return false;
		}
		char c = text[0];
		if (c == '-')
		{
			CommandParser.Command command = new CommandParser.Command(text);
			string key = command.GetName().Trim(new char[]
			{
				'-'
			});
			if (this.mRegistedCmds.ContainsKey(key))
			{
				this.mRegistedCmds[key](command);
			}
			else
			{
				this.SendServerCommand(text);
			}
			return true;
		}
		return false;
	}

	private void HandleShowHideFpsCmd(CommandParser.Command param)
	{
		int num = 0;
		if (int.TryParse(param.GetParam(1), out num))
		{
			if (num == 1)
			{
				GameUIManager.mInstance.ShowFPS();
			}
			else
			{
				GameUIManager.mInstance.HideFPS();
			}
		}
	}

	private void HandleSkillCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		int num2 = 0;
		int.TryParse(param.GetParam(2), out num2);
		ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		if (num < 0 || num >= actor.Skills.Length)
		{
			global::Debug.Log(new object[]
			{
				string.Format("param 1 error, index = {0}", num)
			});
			return;
		}
		if (actor.Skills[num] != null)
		{
			actor.Skills[num] = null;
		}
		if (num2 != 0)
		{
			actor.AddSkill(num, num2, true);
		}
		GUICombatMain session = GameUIManager.mInstance.GetSession<GUICombatMain>();
		if (session != null)
		{
			session.mSkillLayer.SkillBtn1.IsEquipChanged = true;
			session.mSkillLayer.SkillBtn2.IsEquipChanged = true;
			session.mSkillLayer.SkillBtn3.IsEquipChanged = true;
		}
	}

	private void HandleSummonCmd(CommandParser.Command param)
	{
		int monsterID = 0;
		int.TryParse(param.GetParam(1), out monsterID);
		int num = 0;
		int.TryParse(param.GetParam(2), out num);
		int num2 = 0;
		int.TryParse(param.GetParam(3), out num2);
		ActorController actorController = Globals.Instance.ActorMgr.GetActor(0);
		if (actorController == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		actorController = Globals.Instance.ActorMgr.SummonMonster(monsterID, actorController.transform.position, actorController.transform.rotation);
		if (num != 0)
		{
			actorController.GetComponent<AIController>().Locked = true;
		}
		if (num2 != 0)
		{
			actorController.Undead = true;
		}
	}

	private void HandleAddBuffCmd(CommandParser.Command param)
	{
		int buffID = 0;
		int.TryParse(param.GetParam(1), out buffID);
		int num = 0;
		int.TryParse(param.GetParam(2), out num);
		int index = 0;
		int.TryParse(param.GetParam(3), out index);
		ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		PoolMgr.CreateBuffPrefabPool(buffID);
		if (num == 0)
		{
			actor.AddBuff(buffID, actor);
		}
		else if (num == 1)
		{
			if (actor.AiCtrler.Target != null)
			{
				actor.AiCtrler.Target.AddBuff(buffID, actor);
			}
		}
		else if (num == 2)
		{
			for (int i = 0; i < 5; i++)
			{
				actor = Globals.Instance.ActorMgr.GetActor(i);
				if (actor != null)
				{
					actor.AddBuff(buffID, actor);
				}
			}
		}
		else if (num == 3)
		{
			actor = Globals.Instance.ActorMgr.GetActor(index);
			if (actor != null)
			{
				actor.AddBuff(buffID, actor);
			}
		}
	}

	private void HandleDeadCmd(CommandParser.Command param)
	{
		int index = 0;
		int.TryParse(param.GetParam(1), out index);
		ActorController actor = Globals.Instance.ActorMgr.GetActor(index);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		actor.DoDamage(actor.MaxHP * 2L, null, false);
	}

	private void HandleClearCDCmd(CommandParser.Command param)
	{
		ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		actor.ClearSkillCD();
	}

	private void HandleTakeDayEnergyCmd(CommandParser.Command param)
	{
		int flag = 0;
		int.TryParse(param.GetParam(1), out flag);
		MC2S_GetDayEnergy mC2S_GetDayEnergy = new MC2S_GetDayEnergy();
		mC2S_GetDayEnergy.Flag = flag;
		Globals.Instance.CliSession.Send(206, mC2S_GetDayEnergy);
	}

	private void HandleTakeQuestRewardCmd(CommandParser.Command param)
	{
		int questID = 0;
		int.TryParse(param.GetParam(1), out questID);
		MC2S_TakeQuestReward mC2S_TakeQuestReward = new MC2S_TakeQuestReward();
		mC2S_TakeQuestReward.QuestID = questID;
		Globals.Instance.CliSession.Send(209, mC2S_TakeQuestReward);
	}

	private void HandleGetMailDataCmd(CommandParser.Command param)
	{
		MC2S_GetMailData mC2S_GetMailData = new MC2S_GetMailData();
		mC2S_GetMailData.MinMailID = 0u;
		Globals.Instance.CliSession.Send(212, mC2S_GetMailData);
	}

	private void HandleTakeMailAffixCmd(CommandParser.Command param)
	{
		uint mailID = 0u;
		uint.TryParse(param.GetParam(1), out mailID);
		MC2S_TakeMailAffix mC2S_TakeMailAffix = new MC2S_TakeMailAffix();
		mC2S_TakeMailAffix.MailID = mailID;
		Globals.Instance.CliSession.Send(214, mC2S_TakeMailAffix);
	}

	private void HandleChatCmd(CommandParser.Command param)
	{
		string param2 = param.GetParam(1);
		int channel = 0;
		int.TryParse(param.GetParam(2), out channel);
		ulong playerID = 0uL;
		ulong.TryParse(param.GetParam(2), out playerID);
		MC2S_Chat mC2S_Chat = new MC2S_Chat();
		mC2S_Chat.Message = param2;
		mC2S_Chat.Channel = channel;
		mC2S_Chat.PlayerID = playerID;
		mC2S_Chat.Voice = false;
		Globals.Instance.CliSession.Send(216, mC2S_Chat);
	}

	private void HandleDiamond2MoneyCmd(CommandParser.Command param)
	{
		int count = 0;
		int.TryParse(param.GetParam(1), out count);
		MC2S_Diamond2Money mC2S_Diamond2Money = new MC2S_Diamond2Money();
		mC2S_Diamond2Money.Count = count;
		Globals.Instance.CliSession.Send(220, mC2S_Diamond2Money);
	}

	private void HandleLuckyRollCmd(CommandParser.Command param)
	{
		int type = 0;
		int.TryParse(param.GetParam(1), out type);
		int num = 0;
		int.TryParse(param.GetParam(2), out num);
		int num2 = 0;
		int.TryParse(param.GetParam(3), out num2);
		MC2S_LuckyRoll mC2S_LuckyRoll = new MC2S_LuckyRoll();
		mC2S_LuckyRoll.Type = type;
		mC2S_LuckyRoll.Flag = (num != 0);
		mC2S_LuckyRoll.Free = (num2 != 0);
		Globals.Instance.CliSession.Send(206, mC2S_LuckyRoll);
	}

	private void HandleSignIn(CommandParser.Command param)
	{
		int index = 0;
		int.TryParse(param.GetParam(1), out index);
		MC2S_SignIn mC2S_SignIn = new MC2S_SignIn();
		mC2S_SignIn.Index = index;
		Globals.Instance.CliSession.Send(226, mC2S_SignIn);
	}

	private void HandleTakeLevelRewardCmd(CommandParser.Command param)
	{
		int index = 0;
		int.TryParse(param.GetParam(1), out index);
		MC2S_TakeLevelReward mC2S_TakeLevelReward = new MC2S_TakeLevelReward();
		mC2S_TakeLevelReward.Index = index;
		Globals.Instance.CliSession.Send(228, mC2S_TakeLevelReward);
	}

	private void HandleTakeCardDiamondCmd(CommandParser.Command param)
	{
		MC2S_TakeCardDiamond ojb = new MC2S_TakeCardDiamond();
		Globals.Instance.CliSession.Send(234, ojb);
	}

	private void HandleChangeNameCmd(CommandParser.Command param)
	{
		MC2S_ChangeName mC2S_ChangeName = new MC2S_ChangeName();
		mC2S_ChangeName.Name = param.GetParam(1);
		Globals.Instance.CliSession.Send(238, mC2S_ChangeName);
	}

	private void HandleDialogCmd(CommandParser.Command param)
	{
		int dialogID = 0;
		int.TryParse(param.GetParam(1), out dialogID);
		GameUIManager.mInstance.ShowPlotDialog(dialogID, null, null);
	}

	private void HandleChangeForceFollowCmd(CommandParser.Command param)
	{
		Globals.Instance.ActorMgr.ChangeForceFollow();
	}

	private void HandleShowReconnectCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		Globals.Instance.CliSession.ShowReconnect(num != 0);
	}

	private void HandleGetBossDataCmd(CommandParser.Command param)
	{
		MC2S_GetBossData mC2S_GetBossData = new MC2S_GetBossData();
		mC2S_GetBossData.Status = Globals.Instance.Player.WorldBossSystem.Status;
		mC2S_GetBossData.TimeStamp = Globals.Instance.Player.WorldBossSystem.TimeStamp;
		Globals.Instance.CliSession.Send(614, mC2S_GetBossData);
	}

	private void HandleEnterWSCmd(CommandParser.Command param)
	{
		int slot = 0;
		int.TryParse(param.GetParam(1), out slot);
		MC2S_WorldBossStart mC2S_WorldBossStart = new MC2S_WorldBossStart();
		mC2S_WorldBossStart.Slot = slot;
		Globals.Instance.CliSession.Send(624, mC2S_WorldBossStart);
	}

	private void HandleTakeWRewardCmd(CommandParser.Command param)
	{
		MC2S_TakeWorldBossReward ojb = new MC2S_TakeWorldBossReward();
		Globals.Instance.CliSession.Send(626, ojb);
	}

	private void HandleCloseSessionCmd(CommandParser.Command param)
	{
		Globals.Instance.CliSession.Close();
	}

	private void HandleReconnectCmd(CommandParser.Command param)
	{
		Globals.Instance.CliSession.Reconnect();
	}

	private void HandleTrialStartCmd(CommandParser.Command param)
	{
		MC2S_TrialStart ojb = new MC2S_TrialStart();
		Globals.Instance.CliSession.Send(628, ojb);
	}

	private void HandleTrialResetCmd(CommandParser.Command param)
	{
		MC2S_TrialReset ojb = new MC2S_TrialReset();
		Globals.Instance.CliSession.Send(632, ojb);
	}

	private void HandleTrialFarmStartCmd(CommandParser.Command param)
	{
		MC2S_TrialFarmStart ojb = new MC2S_TrialFarmStart();
		Globals.Instance.CliSession.Send(634, ojb);
	}

	private void HandleTrialFarmStopCmd(CommandParser.Command param)
	{
		MC2S_TrialFarmStop ojb = new MC2S_TrialFarmStop();
		Globals.Instance.CliSession.Send(636, ojb);
	}

	private void HandleEnterSceneCmd(CommandParser.Command param)
	{
		int sceneID = 0;
		int.TryParse(param.GetParam(1), out sceneID);
		GameUIManager.mInstance.LoadScene(sceneID);
	}

	private void SendServerCommand(string cmd)
	{
		MC2S_GMCommand mC2S_GMCommand = new MC2S_GMCommand();
		mC2S_GMCommand.Command = cmd;
		Globals.Instance.CliSession.Send(1498, mC2S_GMCommand);
	}

	private void HandleEnterTutorialCmd(CommandParser.Command param)
	{
		Globals.Instance.TutorialMgr.StartTutorial = true;
	}

	private void HandleCloseTutorialCmd(CommandParser.Command param)
	{
		Globals.Instance.TutorialMgr.StartTutorial = false;
		Globals.Instance.TutorialMgr.ClearGuideMask();
		Globals.Instance.TutorialMgr.ClearTutorial();
	}

	private void HandleRemoveTutorialCmd(CommandParser.Command param)
	{
		Globals.Instance.TutorialMgr.StartTutorial = false;
		Globals.Instance.TutorialMgr.ClearGuideMask();
		Globals.Instance.TutorialMgr.ClearTutorial();
		Globals.Instance.TutorialMgr.CurrentTutorialNum = TutorialManager.ETutorialNum.Tutorial_Null;
		MC2S_SaveGuideSteps mC2S_SaveGuideSteps = new MC2S_SaveGuideSteps();
		mC2S_SaveGuideSteps.index = -1;
		global::Debug.Log(new object[]
		{
			"send to server :" + mC2S_SaveGuideSteps.index
		});
		Globals.Instance.CliSession.SendPacket(244, mC2S_SaveGuideSteps);
	}

	private void HandlePassTutorialCmd(CommandParser.Command param)
	{
		for (int i = 0; i <= Convert.ToInt32(param.GetParam(1)); i++)
		{
			TutorialEntity.SendTutorialNumToServer(i);
		}
	}

	private void HandleDoorCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		Globals.Instance.ActorMgr.SetWalkableMask(4, 0 == num);
	}

	private void HandleAttackSpeedModCmd(CommandParser.Command param)
	{
		int value = 0;
		int.TryParse(param.GetParam(1), out value);
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < 5; i++)
		{
			if (i >= actors.Count || actors[i] == null || actors[i].IsDead)
			{
				break;
			}
			actors[i].BuffAttackSpeedMod(value, true);
		}
	}

	private void HandlePauseCmd(CommandParser.Command param)
	{
		float num = 0f;
		float.TryParse(param.GetParam(1), out num);
		int num2 = 0;
		int.TryParse(param.GetParam(2), out num2);
		if (num2 == 0)
		{
			Globals.Instance.ActorMgr.Pause(num != 0f);
		}
		else
		{
			ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
			if (actor == null)
			{
				global::Debug.Log(new object[]
				{
					"must in game scene!"
				});
				return;
			}
			Globals.Instance.ActorMgr.PauseActor(num != 0f, actor);
		}
	}

	private void HandleQueryAttCmd(CommandParser.Command param)
	{
		float num = 0f;
		float.TryParse(param.GetParam(1), out num);
		global::Debug.Log(new object[]
		{
			"----------Query Attribute----------"
		});
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		if (num == 0f)
		{
			for (int i = 0; i < actors.Count; i++)
			{
				if (actors[i] == null)
				{
				}
			}
		}
		else if (num == 1f)
		{
			for (int j = 0; j < 5; j++)
			{
				if (actors[j] == null)
				{
				}
			}
		}
		else if (num == 2f)
		{
			for (int k = 5; k < actors.Count; k++)
			{
				if (actors[k] == null)
				{
				}
			}
		}
		global::Debug.Log(new object[]
		{
			"----------------------------------"
		});
	}

	private void UpdatePlayer()
	{
		if (Globals.Instance.Player.PlayerUpdateEvent != null)
		{
			Globals.Instance.Player.PlayerUpdateEvent();
		}
	}

	private void HandleAddDiamond(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		num += Globals.Instance.Player.Data.Diamond;
		if (num < 0)
		{
			Globals.Instance.Player.Data.Diamond = 0;
		}
		else
		{
			Globals.Instance.Player.Data.Diamond = num;
		}
		this.UpdatePlayer();
	}

	private void HandleAddMoney(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		num += Globals.Instance.Player.Data.Money;
		if (num < 0)
		{
			Globals.Instance.Player.Data.Money = 0;
		}
		else
		{
			Globals.Instance.Player.Data.Diamond = num;
		}
		this.UpdatePlayer();
	}

	private void HandleAddEnergy(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		num += Globals.Instance.Player.Data.Energy;
		if (num < 0)
		{
			Globals.Instance.Player.Data.Energy = 0;
		}
		else
		{
			Globals.Instance.Player.Data.Energy = num;
		}
		this.UpdatePlayer();
	}

	private void HandleResetSceneCD(CommandParser.Command param)
	{
		int sceneID = 0;
		int.TryParse(param.GetParam(1), out sceneID);
		SceneData sceneData = Globals.Instance.Player.GetSceneData(sceneID);
		if (sceneData == null)
		{
			return;
		}
		sceneData.ResetCount = 0;
		sceneData.CoolDown = Globals.Instance.Player.GetTimeStamp();
		sceneData.Times = 0;
	}

	private void HandleAddHP(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		int num2 = (int)actor.CurHP + num;
		if (num2 < 0)
		{
			num2 = 1;
		}
		if ((long)num2 > actor.MaxHP)
		{
			num2 = (int)actor.MaxHP;
		}
		actor.CurHP = (long)((ulong)num2);
	}

	private void HandlePetDeadCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		for (int i = 1; i < 5; i++)
		{
			ActorController actor = Globals.Instance.ActorMgr.GetActor(i);
			if (actor != null && (num == 0 || i == num))
			{
				actor.DoDamage(actor.MaxHP * 2L, null, false);
			}
		}
	}

	private void HandleTestCmd(CommandParser.Command param)
	{
		SdkU3dCallback.DarenUpdatedEvent(1, null);
	}

	private void HandleCheckPayResultCmd(CommandParser.Command param)
	{
		string param2 = param.GetParam(1);
		MC2S_CheckPayResult mC2S_CheckPayResult = new MC2S_CheckPayResult();
		mC2S_CheckPayResult.OrderID = param2;
		Globals.Instance.CliSession.Send(258, mC2S_CheckPayResult);
	}

	private void HandleExchangeGiftCodeCmd(CommandParser.Command param)
	{
		string param2 = param.GetParam(1);
		MC2S_ExchangeGiftCode mC2S_ExchangeGiftCode = new MC2S_ExchangeGiftCode();
		mC2S_ExchangeGiftCode.Code = param2;
		Globals.Instance.CliSession.Send(724, mC2S_ExchangeGiftCode);
	}

	private void HandleChangeSocketCmd(CommandParser.Command param)
	{
		int slot = 0;
		int.TryParse(param.GetParam(1), out slot);
		int slot2 = 0;
		int.TryParse(param.GetParam(2), out slot2);
		MC2S_ChangeSocket mC2S_ChangeSocket = new MC2S_ChangeSocket();
		mC2S_ChangeSocket.Slot1 = slot;
		mC2S_ChangeSocket.Slot2 = slot2;
		Globals.Instance.CliSession.Send(193, mC2S_ChangeSocket);
	}

	private void HandleSetCombatPetCmd(CommandParser.Command param)
	{
		int slot = 0;
		int.TryParse(param.GetParam(1), out slot);
		ulong petID = 0uL;
		ulong.TryParse(param.GetParam(2), out petID);
		MC2S_SetCombatPet mC2S_SetCombatPet = new MC2S_SetCombatPet();
		mC2S_SetCombatPet.Slot = slot;
		mC2S_SetCombatPet.PetID = petID;
		Globals.Instance.CliSession.Send(195, mC2S_SetCombatPet);
	}

	private void HandleEquipItemCmd(CommandParser.Command param)
	{
		int socketSlot = 0;
		int.TryParse(param.GetParam(1), out socketSlot);
		int equipSlot = 0;
		int.TryParse(param.GetParam(2), out equipSlot);
		ulong itemID = 0uL;
		ulong.TryParse(param.GetParam(3), out itemID);
		MC2S_EquipItem mC2S_EquipItem = new MC2S_EquipItem();
		mC2S_EquipItem.SocketSlot = socketSlot;
		mC2S_EquipItem.EquipSlot = equipSlot;
		mC2S_EquipItem.ItemID = itemID;
		Globals.Instance.CliSession.Send(197, mC2S_EquipItem);
	}

	private void HandleShowAttCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(num);
		if (socket != null)
		{
			global::Debug.LogFormat("MaxHP = {0}, Attack = {1}, PhysicDefense = {2}, MagicDefense = {3}, Hit = {4}, Dodge = {5}, Crit = {6}, CritResis = {7}, DamagePlus = {8}, DamageMinus = {9}, TotalCombatValue = {10}", new object[]
			{
				socket.MaxHP,
				socket.Attack,
				socket.PhysicDefense,
				socket.MagicDefense,
				socket.GetAtt(5),
				socket.GetAtt(6),
				socket.GetAtt(7),
				socket.GetAtt(8),
				socket.GetAtt(9),
				socket.GetAtt(10),
				Globals.Instance.Player.TeamSystem.GetCombatValue()
			});
		}
		this.SendServerCommand(string.Format("-show_att {0}", num));
	}

	private void HandlePetLevelupCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		long itemID = 0L;
		long.TryParse(param.GetParam(2), out itemID);
		int count = 0;
		int.TryParse(param.GetParam(3), out count);
		long num2 = 0L;
		long.TryParse(param.GetParam(4), out num2);
		int num3 = 0;
		int.TryParse(param.GetParam(5), out num3);
		MC2S_PetLevelup mC2S_PetLevelup = new MC2S_PetLevelup();
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(num);
		if (socket == null)
		{
			global::Debug.LogFormat("pet is empty, slot = {0}", new object[]
			{
				num
			});
		}
		PetDataEx pet = socket.GetPet();
		if (pet != null)
		{
			mC2S_PetLevelup.PetID = pet.Data.ID;
			ItemUpdate itemUpdate = new ItemUpdate();
			itemUpdate.ItemID = (ulong)itemID;
			itemUpdate.Count = count;
			Globals.Instance.CliSession.Send(402, mC2S_PetLevelup);
		}
	}

	private void HandlePetFurtherCmd(CommandParser.Command param)
	{
		ulong petID = 0uL;
		ulong.TryParse(param.GetParam(1), out petID);
		MC2S_PetFurther mC2S_PetFurther = new MC2S_PetFurther();
		mC2S_PetFurther.PetID = petID;
		Globals.Instance.CliSession.Send(404, mC2S_PetFurther);
	}

	private void HandleItemEnhanceCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
	}

	private void HandleEquipFefineCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
	}

	private void HandleTrinketEnhanceCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
	}

	private void HandleTrinketFefineCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
	}

	private void HandleEquipCreateCmd(CommandParser.Command param)
	{
		ulong itemID = 0uL;
		ulong.TryParse(param.GetParam(1), out itemID);
		MC2S_EquipCreate mC2S_EquipCreate = new MC2S_EquipCreate();
		mC2S_EquipCreate.ItemID = itemID;
		Globals.Instance.CliSession.Send(502, mC2S_EquipCreate);
	}

	private void HandleTrinketCreateCmd(CommandParser.Command param)
	{
		int infoID = 0;
		int.TryParse(param.GetParam(1), out infoID);
		MC2S_TrinketCreate mC2S_TrinketCreate = new MC2S_TrinketCreate();
		mC2S_TrinketCreate.InfoID = infoID;
		Globals.Instance.CliSession.Send(506, mC2S_TrinketCreate);
	}

	private void HandleEquipBreakUpCmd(CommandParser.Command param)
	{
		ulong item = 0uL;
		ulong.TryParse(param.GetParam(1), out item);
		MC2S_EquipBreakUp mC2S_EquipBreakUp = new MC2S_EquipBreakUp();
		mC2S_EquipBreakUp.EquipID.Add(item);
		Globals.Instance.CliSession.Send(530, mC2S_EquipBreakUp);
	}

	private void HandleTrinketRebornCmd(CommandParser.Command param)
	{
		ulong trinketID = 0uL;
		ulong.TryParse(param.GetParam(1), out trinketID);
		MC2S_TrinketReborn mC2S_TrinketReborn = new MC2S_TrinketReborn();
		mC2S_TrinketReborn.TrinketID = trinketID;
		Globals.Instance.CliSession.Send(532, mC2S_TrinketReborn);
	}

	private void HandlePetSkillCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		int index = 0;
		int.TryParse(param.GetParam(2), out index);
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(num);
		if (socket == null)
		{
			global::Debug.LogFormat("pet is empty, slot = {0}", new object[]
			{
				num
			});
		}
		PetDataEx pet = socket.GetPet();
		MC2S_PetSkill mC2S_PetSkill = new MC2S_PetSkill();
		mC2S_PetSkill.PetID = pet.Data.ID;
		mC2S_PetSkill.Index = index;
		Globals.Instance.CliSession.Send(406, mC2S_PetSkill);
	}

	private void HandlePetRebornCmd(CommandParser.Command param)
	{
		ulong petID = 0uL;
		ulong.TryParse(param.GetParam(1), out petID);
		MC2S_PetReborn mC2S_PetReborn = new MC2S_PetReborn();
		mC2S_PetReborn.PetID = petID;
		Globals.Instance.CliSession.Send(415, mC2S_PetReborn);
	}

	private void HandlePetBreakUpCmd(CommandParser.Command param)
	{
		ulong item = 0uL;
		ulong.TryParse(param.GetParam(1), out item);
		MC2S_PetBreakUp mC2S_PetBreakUp = new MC2S_PetBreakUp();
		mC2S_PetBreakUp.PetID.Add(item);
		Globals.Instance.CliSession.Send(413, mC2S_PetBreakUp);
	}

	private void HandleQueryArenaCmd(CommandParser.Command param)
	{
		MC2S_QueryArenaData ojb = new MC2S_QueryArenaData();
		Globals.Instance.CliSession.Send(801, ojb);
	}

	private void HandleArenaStartCmd(CommandParser.Command param)
	{
		ulong num = 0uL;
		ulong.TryParse(param.GetParam(1), out num);
		int rank = 0;
		int.TryParse(param.GetParam(2), out rank);
		Globals.Instance.Player.PvpSystem.SetArenaTargetID(num);
		MC2S_PvpArenaStart mC2S_PvpArenaStart = new MC2S_PvpArenaStart();
		mC2S_PvpArenaStart.TargetID = num;
		mC2S_PvpArenaStart.Rank = rank;
		Globals.Instance.CliSession.Send(803, mC2S_PvpArenaStart);
	}

	private void HandleQueryPillageCmd(CommandParser.Command param)
	{
		int itemID = 0;
		int.TryParse(param.GetParam(1), out itemID);
		MC2S_QueryPillageTarget mC2S_QueryPillageTarget = new MC2S_QueryPillageTarget();
		mC2S_QueryPillageTarget.ItemID = itemID;
		Globals.Instance.CliSession.Send(814, mC2S_QueryPillageTarget);
	}

	private void HandlePillageStartCmd(CommandParser.Command param)
	{
		ulong num = 0uL;
		ulong.TryParse(param.GetParam(1), out num);
		Globals.Instance.Player.PvpSystem.SetPillageTargetID(num);
		MC2S_PvpPillageStart mC2S_PvpPillageStart = new MC2S_PvpPillageStart();
		mC2S_PvpPillageStart.TargetID = num;
		Globals.Instance.CliSession.Send(816, mC2S_PvpPillageStart);
	}

	private void HandleConLevelupCmd(CommandParser.Command param)
	{
		MC2S_ConstellationLevelup ojb = new MC2S_ConstellationLevelup();
		Globals.Instance.CliSession.Send(203, ojb);
	}

	private void HandleTakeAchievementRewardCmd(CommandParser.Command param)
	{
		int achievementID = 0;
		int.TryParse(param.GetParam(1), out achievementID);
		MC2S_TakeAchievementReward mC2S_TakeAchievementReward = new MC2S_TakeAchievementReward();
		mC2S_TakeAchievementReward.AchievementID = achievementID;
		Globals.Instance.CliSession.Send(246, mC2S_TakeAchievementReward);
	}

	private void HandleQueryRemotePlayerCmd(CommandParser.Command param)
	{
		ulong playerID = 0uL;
		ulong.TryParse(param.GetParam(1), out playerID);
		MC2S_QueryRemotePlayer mC2S_QueryRemotePlayer = new MC2S_QueryRemotePlayer();
		mC2S_QueryRemotePlayer.PlayerID = playerID;
		Globals.Instance.CliSession.Send(286, mC2S_QueryRemotePlayer);
	}

	private void HandleTakeDailyScoreRewardCmd(CommandParser.Command param)
	{
		int index = 0;
		int.TryParse(param.GetParam(1), out index);
		MC2S_TakeDailyScoreReward mC2S_TakeDailyScoreReward = new MC2S_TakeDailyScoreReward();
		mC2S_TakeDailyScoreReward.Index = index;
		Globals.Instance.CliSession.Send(248, mC2S_TakeDailyScoreReward);
	}

	private void HandleShowPetCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		PetInfo info = Globals.Instance.AttDB.PetDict.GetInfo(num);
		if (info == null)
		{
			global::Debug.LogErrorFormat("PetDict.GetInfo error, id = {0}", new object[]
			{
				num
			});
			return;
		}
		PetDataEx petData = new PetDataEx(new PetData
		{
			InfoID = num,
			Level = 1u
		}, info);
		GetPetLayer.Show(petData, null, GetPetLayer.EGPL_ShowNewsType.Null);
	}

	private void HandleTakeSevenDayRewardCmd(CommandParser.Command param)
	{
		int iD = 0;
		int.TryParse(param.GetParam(1), out iD);
		MC2S_TakeSevenDayReward mC2S_TakeSevenDayReward = new MC2S_TakeSevenDayReward();
		mC2S_TakeSevenDayReward.ID = iD;
		Globals.Instance.CliSession.Send(722, mC2S_TakeSevenDayReward);
	}

	private void HandleShareAchievementCmd(CommandParser.Command param)
	{
		int iD = 0;
		int.TryParse(param.GetParam(1), out iD);
		MC2S_ShareAchievement mC2S_ShareAchievement = new MC2S_ShareAchievement();
		mC2S_ShareAchievement.ID = iD;
		Globals.Instance.CliSession.Send(731, mC2S_ShareAchievement);
	}

	private void HandleTakeShareAchievementRewardCmd(CommandParser.Command param)
	{
		int iD = 0;
		int.TryParse(param.GetParam(1), out iD);
		MC2S_TakeShareAchievementReward mC2S_TakeShareAchievementReward = new MC2S_TakeShareAchievementReward();
		mC2S_TakeShareAchievementReward.ID = iD;
		Globals.Instance.CliSession.Send(733, mC2S_TakeShareAchievementReward);
	}

	private void HandleBuyFundCmd(CommandParser.Command param)
	{
		MC2S_BuyFund ojb = new MC2S_BuyFund();
		Globals.Instance.CliSession.Send(735, ojb);
	}

	private void HandleTakeFundLevelRewardCmd(CommandParser.Command param)
	{
		int iD = 0;
		int.TryParse(param.GetParam(1), out iD);
		MC2S_TakeFundLevelReward mC2S_TakeFundLevelReward = new MC2S_TakeFundLevelReward();
		mC2S_TakeFundLevelReward.ID = iD;
		Globals.Instance.CliSession.Send(737, mC2S_TakeFundLevelReward);
	}

	private void HandleTakeWelfareCmd(CommandParser.Command param)
	{
		int iD = 0;
		int.TryParse(param.GetParam(1), out iD);
		MC2S_TakeWelfare mC2S_TakeWelfare = new MC2S_TakeWelfare();
		mC2S_TakeWelfare.ID = iD;
		Globals.Instance.CliSession.Send(739, mC2S_TakeWelfare);
	}

	private void HandleUndeadCmd(CommandParser.Command param)
	{
		int index = 0;
		int.TryParse(param.GetParam(1), out index);
		int num = 0;
		int.TryParse(param.GetParam(2), out num);
		ActorController actor = Globals.Instance.ActorMgr.GetActor(index);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		actor.Undead = (num != 0);
	}

	private void HandleChangeMemoryCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		int num2 = 0;
		int.TryParse(param.GetParam(2), out num2);
		if (num2 == 0)
		{
			PetDataEx pet = Globals.Instance.Player.TeamSystem.GetPet(0);
			if (pet != null)
			{
				pet.Data.Level = (uint)num;
			}
		}
		else
		{
			ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
			if (actor == null)
			{
				global::Debug.Log(new object[]
				{
					"must in game scene!"
				});
				return;
			}
			actor.level.Value = num;
		}
	}

	private void HandleShowIndicateCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		if (num != 0)
		{
			GameUIManager.mInstance.ShowIndicate();
		}
		else
		{
			GameUIManager.mInstance.HideIndicate();
		}
	}

	private void HandleTakeDayHotRewardCmd(CommandParser.Command param)
	{
		int iD = 0;
		int.TryParse(param.GetParam(1), out iD);
		int index = 0;
		int.TryParse(param.GetParam(2), out index);
		MC2S_TakeDayHotReward mC2S_TakeDayHotReward = new MC2S_TakeDayHotReward();
		mC2S_TakeDayHotReward.ID = iD;
		mC2S_TakeDayHotReward.Index = index;
		Globals.Instance.CliSession.Send(755, mC2S_TakeDayHotReward);
	}

	private void HandleEquipAwakeItemCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		int index = 0;
		int.TryParse(param.GetParam(2), out index);
		ulong itemID = 0uL;
		ulong.TryParse(param.GetParam(3), out itemID);
		PetDataEx pet = Globals.Instance.Player.TeamSystem.GetPet(num);
		if (pet == null)
		{
			global::Debug.LogErrorFormat("can't find the pet, slot = {0}", new object[]
			{
				num
			});
			return;
		}
		MC2S_EquipAwakeItem mC2S_EquipAwakeItem = new MC2S_EquipAwakeItem();
		mC2S_EquipAwakeItem.PetID = pet.Data.ID;
		mC2S_EquipAwakeItem.Index = index;
		mC2S_EquipAwakeItem.ItemID = itemID;
		Globals.Instance.CliSession.Send(417, mC2S_EquipAwakeItem);
	}

	private void HandleAwakeLevelupCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		PetDataEx pet = Globals.Instance.Player.TeamSystem.GetPet(num);
		if (pet == null)
		{
			global::Debug.LogErrorFormat("can't find the pet, slot = {0}", new object[]
			{
				num
			});
			return;
		}
		MC2S_AwakeLevelup mC2S_AwakeLevelup = new MC2S_AwakeLevelup();
		mC2S_AwakeLevelup.PetID = pet.Data.ID;
		Globals.Instance.CliSession.Send(419, mC2S_AwakeLevelup);
	}

	private void HandleAwakeItemCreateCmd(CommandParser.Command param)
	{
		int infoID = 0;
		int.TryParse(param.GetParam(1), out infoID);
		MC2S_AwakeItemCreate mC2S_AwakeItemCreate = new MC2S_AwakeItemCreate();
		mC2S_AwakeItemCreate.InfoID = infoID;
		Globals.Instance.CliSession.Send(536, mC2S_AwakeItemCreate);
	}

	private void HandleAwakeItemBreakUpCmd(CommandParser.Command param)
	{
		ulong iD = 0uL;
		ulong.TryParse(param.GetParam(1), out iD);
		int count = 0;
		int.TryParse(param.GetParam(2), out count);
		MC2S_AwakeItemBreakUp mC2S_AwakeItemBreakUp = new MC2S_AwakeItemBreakUp();
		mC2S_AwakeItemBreakUp.ID = iD;
		mC2S_AwakeItemBreakUp.Count = count;
		Globals.Instance.CliSession.Send(538, mC2S_AwakeItemBreakUp);
	}

	private void HandleBuyActivityPayShopItemCmd(CommandParser.Command param)
	{
		int activityID = 0;
		int.TryParse(param.GetParam(1), out activityID);
		int itemID = 0;
		int.TryParse(param.GetParam(2), out itemID);
		int price = 0;
		int.TryParse(param.GetParam(3), out price);
		MC2S_BuyActivityPayShopItem mC2S_BuyActivityPayShopItem = new MC2S_BuyActivityPayShopItem();
		mC2S_BuyActivityPayShopItem.ActivityID = activityID;
		mC2S_BuyActivityPayShopItem.ItemID = itemID;
		mC2S_BuyActivityPayShopItem.Price = price;
		Globals.Instance.CliSession.Send(759, mC2S_BuyActivityPayShopItem);
	}

	private void HandleShowStatsCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		SocketDataEx socket = Globals.Instance.Player.TeamSystem.GetSocket(num);
		if (socket != null)
		{
			global::Debug.LogFormat("Socket Stats: MaxHP = {0}, Attack = {1}, PhysicDefense = {2}, MagicDefense = {3}, Hit = {4}, Dodge = {5}, Crit = {6}, CritResis = {7}, DamagePlus = {8}, DamageMinus = {9}", new object[]
			{
				socket.MaxHP,
				socket.Attack,
				socket.PhysicDefense,
				socket.MagicDefense,
				socket.GetAtt(5),
				socket.GetAtt(6),
				socket.GetAtt(7),
				socket.GetAtt(8),
				socket.GetAtt(9),
				socket.GetAtt(10)
			});
		}
		ActorController actor = Globals.Instance.ActorMgr.GetActor(num);
		if (actor != null)
		{
			global::Debug.LogFormat("Actor Stats: MaxHP = {0}, Attack = {1}, PhysicDefense = {2}, MagicDefense = {3}, Hit = {4}, Dodge = {5}, Crit = {6}, CritResis = {7}, DamagePlus = {8}, DamageMinus = {9}", new object[]
			{
				actor.MaxHP,
				actor.GetAtt(EAttID.EAID_Attack),
				actor.GetAtt(EAttID.EAID_PhysicDefense),
				actor.GetAtt(EAttID.EAID_MagicDefense),
				actor.GetAtt(EAttID.EAID_Hit),
				actor.GetAtt(EAttID.EAID_Dodge),
				actor.GetAtt(EAttID.EAID_Crit),
				actor.GetAtt(EAttID.EAID_CritResis),
				actor.GetAtt(EAttID.EAID_DamagePlus),
				actor.GetAtt(EAttID.EAID_DamageMinus)
			});
		}
	}

	private void HandleSkipCombatCmd(CommandParser.Command param)
	{
		Globals.Instance.ActorMgr.SkipCombat();
	}

	private void HandleGetFamePlayersCmd(CommandParser.Command param)
	{
		MC2S_GetFamePlayers ojb = new MC2S_GetFamePlayers();
		Globals.Instance.CliSession.Send(298, ojb);
	}

	private void HandlePraisePlayerCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		MC2S_PraisePlayer mC2S_PraisePlayer = new MC2S_PraisePlayer();
		mC2S_PraisePlayer.GUID = gUID;
		Globals.Instance.CliSession.Send(300, mC2S_PraisePlayer);
	}

	private void HandleSetFameMessageCmd(CommandParser.Command param)
	{
		string param2 = param.GetParam(1);
		MC2S_SetFameMessage mC2S_SetFameMessage = new MC2S_SetFameMessage();
		mC2S_SetFameMessage.Message = param2;
		Globals.Instance.CliSession.Send(302, mC2S_SetFameMessage);
	}

	private void HandleRecommendFriendCmd(CommandParser.Command param)
	{
		MC2S_RecommendFriend ojb = new MC2S_RecommendFriend();
		Globals.Instance.CliSession.Send(307, ojb);
	}

	private void HandleRequestFriendCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		MC2S_RequestFriend mC2S_RequestFriend = new MC2S_RequestFriend();
		mC2S_RequestFriend.GUID = gUID;
		Globals.Instance.CliSession.Send(309, mC2S_RequestFriend);
	}

	private void HandleReplyFriendCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		int num = 0;
		int.TryParse(param.GetParam(2), out num);
		MC2S_ReplyFriend mC2S_ReplyFriend = new MC2S_ReplyFriend();
		mC2S_ReplyFriend.GUID = gUID;
		mC2S_ReplyFriend.Agree = (num != 0);
		Globals.Instance.CliSession.Send(311, mC2S_ReplyFriend);
	}

	private void HandleRemoveFriendCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		MC2S_RemoveFriend mC2S_RemoveFriend = new MC2S_RemoveFriend();
		mC2S_RemoveFriend.GUID = gUID;
		Globals.Instance.CliSession.Send(313, mC2S_RemoveFriend);
	}

	private void HandleAddBlackListCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		MC2S_AddBlackList mC2S_AddBlackList = new MC2S_AddBlackList();
		mC2S_AddBlackList.GUID = gUID;
		Globals.Instance.CliSession.Send(315, mC2S_AddBlackList);
	}

	private void HandleRemoveBlackListCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		MC2S_RemoveBlackList mC2S_RemoveBlackList = new MC2S_RemoveBlackList();
		mC2S_RemoveBlackList.GUID = gUID;
		Globals.Instance.CliSession.Send(317, mC2S_RemoveBlackList);
	}

	private void HandleGiveFriendEnergyCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		MC2S_GiveFriendEnergy mC2S_GiveFriendEnergy = new MC2S_GiveFriendEnergy();
		mC2S_GiveFriendEnergy.GUID = gUID;
		Globals.Instance.CliSession.Send(319, mC2S_GiveFriendEnergy);
	}

	private void HandleTakeFriendEnergyCmd(CommandParser.Command param)
	{
		ulong gUID = 0uL;
		ulong.TryParse(param.GetParam(1), out gUID);
		MC2S_TakeFriendEnergy mC2S_TakeFriendEnergy = new MC2S_TakeFriendEnergy();
		mC2S_TakeFriendEnergy.GUID = gUID;
		Globals.Instance.CliSession.Send(321, mC2S_TakeFriendEnergy);
	}

	private void HandleSummonTowerCmd(CommandParser.Command param)
	{
		int infoID = 0;
		int.TryParse(param.GetParam(1), out infoID);
		ActorController actor = Globals.Instance.ActorMgr.GetActor(0);
		if (actor == null)
		{
			global::Debug.Log(new object[]
			{
				"must in game scene!"
			});
			return;
		}
		Globals.Instance.ActorMgr.CreateBuilding(infoID, actor.transform.position, actor.transform.rotation, Vector3.one, ActorController.EFactionType.ERed, true, true);
	}

	private void HandleShowBossAttCmd(CommandParser.Command param)
	{
		ActorController bossActor = Globals.Instance.ActorMgr.GetBossActor();
		if (bossActor != null)
		{
			global::Debug.LogFormat("MaxHP = {0}, Attack = {1}", new object[]
			{
				bossActor.MaxHP,
				bossActor.GetAtt(EAttID.EAID_Attack)
			});
		}
	}

	private void HandleTakePayRewardCmd(CommandParser.Command param)
	{
		if (Globals.Instance.Player.ActivitySystem.SPData == null)
		{
			return;
		}
		int productID = 0;
		int.TryParse(param.GetParam(1), out productID);
		MC2S_TakePayReward mC2S_TakePayReward = new MC2S_TakePayReward();
		mC2S_TakePayReward.ActivityID = Globals.Instance.Player.ActivitySystem.SPData.Base.ID;
		mC2S_TakePayReward.ProductID = productID;
		Globals.Instance.CliSession.Send(768, mC2S_TakePayReward);
	}

	private void HandleDestroyAllTowerCmd(CommandParser.Command param)
	{
		if (Globals.Instance.ActorMgr.CurScene != null && Globals.Instance.ActorMgr.CurScene == Globals.Instance.ActorMgr.MemoryGearScene)
		{
			Globals.Instance.ActorMgr.MemoryGearScene.DestroyAllTower();
		}
	}

	private void HandleShowBuffCountCmd(CommandParser.Command param)
	{
		List<ActorController> actors = Globals.Instance.ActorMgr.Actors;
		for (int i = 0; i < 5; i++)
		{
			if (actors[i] != null)
			{
				global::Debug.LogFormat("slot = {0}, buff count = {1}", new object[]
				{
					i,
					actors[i].Buffs.Count
				});
			}
		}
	}

	private void HandleGetItemCountCmd(CommandParser.Command param)
	{
		int num = 0;
		int.TryParse(param.GetParam(1), out num);
		ItemInfo info = Globals.Instance.AttDB.ItemDict.GetInfo(num);
		if (info == null)
		{
			return;
		}
		GameUIManager.mInstance.ShowMessageTip(string.Format("{0} count = {1}", info.Name, Globals.Instance.Player.ItemSystem.GetEquipCount(num)), 0f, 0f);
	}

	private void HandlePetExchangeCmd(CommandParser.Command param)
	{
		ulong petID = 0uL;
		ulong.TryParse(param.GetParam(1), out petID);
		ulong petID2 = 0uL;
		ulong.TryParse(param.GetParam(2), out petID2);
		MC2S_PetExchange mC2S_PetExchange = new MC2S_PetExchange();
		mC2S_PetExchange.PetID1 = petID;
		mC2S_PetExchange.PetID2 = petID2;
		Globals.Instance.CliSession.Send(425, mC2S_PetExchange);
	}

	private void HandleAddEPCmd(CommandParser.Command param)
	{
		int value = 0;
		int.TryParse(param.GetParam(1), out value);
		Globals.Instance.ActorMgr.AddLocalPlayerEP(value);
	}

	private void HandlePetChangeModelCmd(CommandParser.Command param)
	{
		int index = 0;
		int.TryParse(param.GetParam(1), out index);
		int modelState = 0;
		int.TryParse(param.GetParam(2), out modelState);
		ActorController actor = Globals.Instance.ActorMgr.GetActor(index);
		if (actor != null)
		{
			ModelController component = actor.GetComponent<ModelController>();
			if (component != null)
			{
				component.SetModelState(modelState);
				actor.AnimationCtrler.UpdateAnimCtrl();
			}
		}
	}

	private void HandleUploadVoiceCmd(CommandParser.Command param)
	{
		string param2 = param.GetParam(1);
		Globals.Instance.VoiceMgr.onVoiceRecordFinish(param2);
	}

	private void HandleGetVoiceCmd(CommandParser.Command param)
	{
		string param2 = param.GetParam(1);
		string text = string.Format("key={0}&host={1}&usernum={2}", param2, GameSetting.Data.ServerID, GameSetting.Data.Account);
		string url = "http://voice.x.netease.com:8020/ma32/getfile?" + text;
		Globals.Instance.CliSession.HttpGet(url, 1514, true, Encoding.ASCII.GetBytes(text));
	}

	private void HandleTranslateVoiceCmd(CommandParser.Command param)
	{
		string param2 = param.GetParam(1);
		string s = string.Format("key={0}&host={1}&usernum={2}", param2, GameSetting.Data.ServerID, GameSetting.Data.Account);
		string url = "http://voice.x.netease.com:8020/ma32/get_translation?key=" + param2;
		Globals.Instance.CliSession.HttpGet(url, 1515, false, Encoding.ASCII.GetBytes(s));
	}
}
