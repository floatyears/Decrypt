using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EGameConst")]
	public enum EGameConst
	{
		[ProtoEnum(Name = "EGC_MaxCombatPetNum", Value = 3)]
		EGC_MaxCombatPetNum = 3,
		[ProtoEnum(Name = "EGC_MaxGroupNum", Value = 4)]
		EGC_MaxGroupNum,
		[ProtoEnum(Name = "EGC_MaxAssistNum", Value = 6)]
		EGC_MaxAssistNum = 6,
		[ProtoEnum(Name = "EGC_MaxEquipSlotNum", Value = 6)]
		EGC_MaxEquipSlotNum = 6,
		[ProtoEnum(Name = "EGC_MaxSkillSize", Value = 6)]
		EGC_MaxSkillSize = 6,
		[ProtoEnum(Name = "EGC_MaxPetSkillSize", Value = 4)]
		EGC_MaxPetSkillSize = 4,
		[ProtoEnum(Name = "EGC_MaxTrinketEnhanceItemNum", Value = 5)]
		EGC_MaxTrinketEnhanceItemNum,
		[ProtoEnum(Name = "EGC_MaxTrinketRefineItemNum", Value = 2)]
		EGC_MaxTrinketRefineItemNum = 2,
		[ProtoEnum(Name = "EGC_MinEquipEnhanceLevel", Value = 1)]
		EGC_MinEquipEnhanceLevel = 1,
		[ProtoEnum(Name = "EGC_MinEquipRefineLevel", Value = 0)]
		EGC_MinEquipRefineLevel = 0,
		[ProtoEnum(Name = "EGC_MinTrinketEnhanceLevel", Value = 1)]
		EGC_MinTrinketEnhanceLevel,
		[ProtoEnum(Name = "EGC_MinTrinketRefineLevel", Value = 0)]
		EGC_MinTrinketRefineLevel = 0,
		[ProtoEnum(Name = "EGC_EquipSetEquipNum", Value = 4)]
		EGC_EquipSetEquipNum = 4,
		[ProtoEnum(Name = "EGC_EquipSetAttNum", Value = 3)]
		EGC_EquipSetAttNum = 3,
		[ProtoEnum(Name = "EGC_MaxPetRelationNum", Value = 3)]
		EGC_MaxPetRelationNum = 3,
		[ProtoEnum(Name = "EGC_MaxVipLevel", Value = 15)]
		EGC_MaxVipLevel = 15,
		[ProtoEnum(Name = "EGC_MaxD3MCount", Value = 300)]
		EGC_MaxD3MCount = 300,
		[ProtoEnum(Name = "EGC_MaxBatD3MCount", Value = 5)]
		EGC_MaxBatD3MCount = 5,
		[ProtoEnum(Name = "EGC_MaxWorldBossDamageRankNum", Value = 31)]
		EGC_MaxWorldBossDamageRankNum = 31,
		[ProtoEnum(Name = "EGC_MaxTrialRankNum", Value = 50)]
		EGC_MaxTrialRankNum = 50,
		[ProtoEnum(Name = "EGC_MaxTutorialStepCount", Value = 10)]
		EGC_MaxTutorialStepCount = 10,
		[ProtoEnum(Name = "EGC_MaxCostumepartyPlayerCount", Value = 6)]
		EGC_MaxCostumepartyPlayerCount = 6,
		[ProtoEnum(Name = "EGC_MaxCostumepartyCarnivalCount", Value = 5)]
		EGC_MaxCostumepartyCarnivalCount = 5,
		[ProtoEnum(Name = "EGC_MaxLuckyDrawBillBoardCount", Value = 10)]
		EGC_MaxLuckyDrawBillBoardCount = 10,
		[ProtoEnum(Name = "EGC_LuckyDrawBillBoardRewardCount", Value = 10)]
		EGC_LuckyDrawBillBoardRewardCount = 10,
		[ProtoEnum(Name = "EGC_FlashSaleChestNum", Value = 3)]
		EGC_FlashSaleChestNum = 3,
		[ProtoEnum(Name = "EGC_ScratchOffItemNum", Value = 3)]
		EGC_ScratchOffItemNum = 3,
		[ProtoEnum(Name = "EGC_MaxCombatValueRankNum", Value = 50)]
		EGC_MaxCombatValueRankNum = 50,
		[ProtoEnum(Name = "EGC_MaxRecycleBreakItemNum", Value = 5)]
		EGC_MaxRecycleBreakItemNum = 5,
		[ProtoEnum(Name = "EGC_MaxKingRewardQuestNum", Value = 3)]
		EGC_MaxKingRewardQuestNum = 3,
		[ProtoEnum(Name = "EGC_PlayerPetID", Value = 100)]
		EGC_PlayerPetID = 100,
		[ProtoEnum(Name = "EGC_MaxNameLen", Value = 20)]
		EGC_MaxNameLen = 20,
		[ProtoEnum(Name = "EGC_MonthCardTime", Value = 2505600)]
		EGC_MonthCardTime = 2505600,
		[ProtoEnum(Name = "EGC_MonthCardRenewTime", Value = 259200)]
		EGC_MonthCardRenewTime = 259200,
		[ProtoEnum(Name = "EGC_MaxGuildLevel", Value = 10)]
		EGC_MaxGuildLevel = 10,
		[ProtoEnum(Name = "EGC_MaxGuildNameLength", Value = 8)]
		EGC_MaxGuildNameLength = 8,
		[ProtoEnum(Name = "EGC_MaxGuildManifestoLength", Value = 60)]
		EGC_MaxGuildManifestoLength = 60,
		[ProtoEnum(Name = "EGC_MaxSendGuildListNum", Value = 30)]
		EGC_MaxSendGuildListNum = 30,
		[ProtoEnum(Name = "EGC_MaxGuildOfficerNum", Value = 2)]
		EGC_MaxGuildOfficerNum = 2,
		[ProtoEnum(Name = "EGC_MaxGuildAcademy", Value = 9)]
		EGC_MaxGuildAcademy = 9,
		[ProtoEnum(Name = "EGC_MaxPvpRecordCount", Value = 20)]
		EGC_MaxPvpRecordCount = 20,
		[ProtoEnum(Name = "EGC_PvpCombatTime", Value = 180)]
		EGC_PvpCombatTime = 180,
		[ProtoEnum(Name = "EGC_PetSkill3FurtherLevel", Value = 3)]
		EGC_PetSkill3FurtherLevel = 3,
		[ProtoEnum(Name = "EGC_PetSkill4FurtherLevel", Value = 4)]
		EGC_PetSkill4FurtherLevel,
		[ProtoEnum(Name = "EGC_PlayerPurpleLevel", Value = 10)]
		EGC_PlayerPurpleLevel = 10,
		[ProtoEnum(Name = "EGC_PlayerOrangeLevel", Value = 30)]
		EGC_PlayerOrangeLevel = 30,
		[ProtoEnum(Name = "EGC_MaxAwakeLevel", Value = 50)]
		EGC_MaxAwakeLevel = 50,
		[ProtoEnum(Name = "EGC_MaxAwakeItemNum", Value = 4)]
		EGC_MaxAwakeItemNum = 4,
		[ProtoEnum(Name = "EGC_MaxFameCount", Value = 3)]
		EGC_MaxFameCount = 3,
		[ProtoEnum(Name = "EGC_MaxFameMsgLength", Value = 30)]
		EGC_MaxFameMsgLength = 30,
		[ProtoEnum(Name = "EGC_MaxTrinketCompoundItemCount", Value = 3)]
		EGC_MaxTrinketCompoundItemCount = 3
	}
}
