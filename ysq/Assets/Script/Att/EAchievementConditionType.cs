using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EAchievementConditionType")]
	public enum EAchievementConditionType
	{
		[ProtoEnum(Name = "EACT_Null", Value = 0)]
		EACT_Null,
		[ProtoEnum(Name = "EACT_ChallengeScene", Value = 1)]
		EACT_ChallengeScene,
		[ProtoEnum(Name = "EACT_ChallengeEliteScene", Value = 2)]
		EACT_ChallengeEliteScene,
		[ProtoEnum(Name = "EACT_Trial", Value = 3)]
		EACT_Trial,
		[ProtoEnum(Name = "EACT_Pvp", Value = 4)]
		EACT_Pvp,
		[ProtoEnum(Name = "EACT_TrinketCreate", Value = 5)]
		EACT_TrinketCreate,
		[ProtoEnum(Name = "EACT_TrinketEnhance", Value = 6)]
		EACT_TrinketEnhance,
		[ProtoEnum(Name = "EACT_EquipEnhance", Value = 7)]
		EACT_EquipEnhance,
		[ProtoEnum(Name = "EACT_EquipRefine", Value = 8)]
		EACT_EquipRefine,
		[ProtoEnum(Name = "EACT_CostumeParty", Value = 9)]
		EACT_CostumeParty,
		[ProtoEnum(Name = "EACT_SummonPet", Value = 10)]
		EACT_SummonPet,
		[ProtoEnum(Name = "EACT_SummonPet2", Value = 11)]
		EACT_SummonPet2,
		[ProtoEnum(Name = "EACT_WorldBoss", Value = 12)]
		EACT_WorldBoss,
		[ProtoEnum(Name = "EACT_BuyEnergyItem", Value = 13)]
		EACT_BuyEnergyItem,
		[ProtoEnum(Name = "EACT_BuyStaminaItem", Value = 14)]
		EACT_BuyStaminaItem,
		[ProtoEnum(Name = "EACT_KingReward", Value = 15)]
		EACT_KingReward,
		[ProtoEnum(Name = "EACT_Card", Value = 16)]
		EACT_Card,
		[ProtoEnum(Name = "EACT_SuperCard", Value = 17)]
		EACT_SuperCard,
		[ProtoEnum(Name = "EACT_PlayerLevel", Value = 18)]
		EACT_PlayerLevel,
		[ProtoEnum(Name = "EACT_MapStar", Value = 19)]
		EACT_MapStar,
		[ProtoEnum(Name = "EACT_EliteMapStar", Value = 20)]
		EACT_EliteMapStar,
		[ProtoEnum(Name = "EACT_TrialWave", Value = 21)]
		EACT_TrialWave,
		[ProtoEnum(Name = "EACT_CombatValue", Value = 22)]
		EACT_CombatValue,
		[ProtoEnum(Name = "EACT_PetLevel", Value = 23)]
		EACT_PetLevel,
		[ProtoEnum(Name = "EACT_PetFurther", Value = 24)]
		EACT_PetFurther,
		[ProtoEnum(Name = "EACT_PetSkill", Value = 25)]
		EACT_PetSkill,
		[ProtoEnum(Name = "EACT_VipLevel", Value = 26)]
		EACT_VipLevel,
		[ProtoEnum(Name = "EACT_PartyInteraction", Value = 27)]
		EACT_PartyInteraction,
		[ProtoEnum(Name = "EACT_PartyTime", Value = 28)]
		EACT_PartyTime,
		[ProtoEnum(Name = "EACT_Pay", Value = 29)]
		EACT_Pay,
		[ProtoEnum(Name = "EACT_ConsumeDiamond", Value = 30)]
		EACT_ConsumeDiamond,
		[ProtoEnum(Name = "EACT_LoginDay", Value = 31)]
		EACT_LoginDay,
		[ProtoEnum(Name = "EACT_Pillage", Value = 32)]
		EACT_Pillage,
		[ProtoEnum(Name = "EACT_PetEquipEnhance", Value = 33)]
		EACT_PetEquipEnhance,
		[ProtoEnum(Name = "EACT_PetEquipRefine", Value = 34)]
		EACT_PetEquipRefine,
		[ProtoEnum(Name = "EACT_OneEquipEnhance", Value = 35)]
		EACT_OneEquipEnhance,
		[ProtoEnum(Name = "EACT_OneEquipRefine", Value = 36)]
		EACT_OneEquipRefine,
		[ProtoEnum(Name = "EACT_OnePetFurther", Value = 37)]
		EACT_OnePetFurther,
		[ProtoEnum(Name = "EACT_OnePetSkill", Value = 38)]
		EACT_OnePetSkill,
		[ProtoEnum(Name = "EACT_PvpRank", Value = 39)]
		EACT_PvpRank,
		[ProtoEnum(Name = "EACT_PetTrinketRefine", Value = 40)]
		EACT_PetTrinketRefine,
		[ProtoEnum(Name = "EACT_OneTrinketRefine", Value = 41)]
		EACT_OneTrinketRefine,
		[ProtoEnum(Name = "EACT_PetEquipQuality", Value = 42)]
		EACT_PetEquipQuality,
		[ProtoEnum(Name = "EACT_SceneChapter", Value = 43)]
		EACT_SceneChapter,
		[ProtoEnum(Name = "EACT_EliteSceneChapter", Value = 44)]
		EACT_EliteSceneChapter,
		[ProtoEnum(Name = "EACT_WinPvp", Value = 45)]
		EACT_WinPvp,
		[ProtoEnum(Name = "EACT_GainOrangePet", Value = 46)]
		EACT_GainOrangePet,
		[ProtoEnum(Name = "EACT_OneOrderPay", Value = 47)]
		EACT_OneOrderPay,
		[ProtoEnum(Name = "EACT_KingReward5Star", Value = 48)]
		EACT_KingReward5Star,
		[ProtoEnum(Name = "EACT_BuyDiamond", Value = 49)]
		EACT_BuyDiamond,
		[ProtoEnum(Name = "EACT_AwakeMapStar", Value = 50)]
		EACT_AwakeMapStar,
		[ProtoEnum(Name = "EACT_AwakeSceneChapter", Value = 51)]
		EACT_AwakeSceneChapter,
		[ProtoEnum(Name = "EACT_ChallengeAwakeScene", Value = 52)]
		EACT_ChallengeAwakeScene,
		[ProtoEnum(Name = "EACT_NightmareMapStar", Value = 53)]
		EACT_NightmareMapStar,
		[ProtoEnum(Name = "EACT_NightmareSceneChapter", Value = 54)]
		EACT_NightmareSceneChapter,
		[ProtoEnum(Name = "EACT_ChallengeNightmareScene", Value = 55)]
		EACT_ChallengeNightmareScene,
		[ProtoEnum(Name = "EACT_OrePillage", Value = 56)]
		EACT_OrePillage,
		[ProtoEnum(Name = "EACT_EliteTrialWave", Value = 57)]
		EACT_EliteTrialWave,
		[ProtoEnum(Name = "EACT_GiveFriendEnergy", Value = 58)]
		EACT_GiveFriendEnergy,
		[ProtoEnum(Name = "EACT_FriendCount", Value = 59)]
		EACT_FriendCount,
		[ProtoEnum(Name = "EACT_GainGoldEquip", Value = 60)]
		EACT_GainGoldEquip,
		[ProtoEnum(Name = "EACT_GainGoldTrinket", Value = 61)]
		EACT_GainGoldTrinket,
		[ProtoEnum(Name = "EACT_GuildPvp", Value = 62)]
		EACT_GuildPvp,
		[ProtoEnum(Name = "EACT_GainOrangeLopet", Value = 63)]
		EACT_GainOrangeLopet,
		[ProtoEnum(Name = "EACT_LopetFurther", Value = 64)]
		EACT_LopetFurther,
		[ProtoEnum(Name = "EACT_LopetLevel", Value = 65)]
		EACT_LopetLevel,
		[ProtoEnum(Name = "EACT_PlayerLevel2", Value = 66)]
		EACT_PlayerLevel2,
		[ProtoEnum(Name = "EACT_Max", Value = 67)]
		EACT_Max
	}
}
