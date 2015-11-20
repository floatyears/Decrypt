using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "MsgID")]
	public enum MsgID
	{
		[ProtoEnum(Name = "C2S_EnterGame", Value = 101)]
		C2S_EnterGame = 101,
		[ProtoEnum(Name = "S2C_EnterGame", Value = 102)]
		S2C_EnterGame,
		[ProtoEnum(Name = "C2S_CreatePlayer", Value = 103)]
		C2S_CreatePlayer,
		[ProtoEnum(Name = "S2C_CreatePlayer", Value = 104)]
		S2C_CreatePlayer,
		[ProtoEnum(Name = "C2S_GetPlayerData", Value = 105)]
		C2S_GetPlayerData,
		[ProtoEnum(Name = "S2C_GetPlayerData", Value = 106)]
		S2C_GetPlayerData,
		[ProtoEnum(Name = "C2S_HeartBeat", Value = 107)]
		C2S_HeartBeat,
		[ProtoEnum(Name = "S2C_HeartBeat", Value = 108)]
		S2C_HeartBeat,
		[ProtoEnum(Name = "S2C_SerialIDError", Value = 109)]
		S2C_SerialIDError,
		[ProtoEnum(Name = "S2C_KickPlayer", Value = 110)]
		S2C_KickPlayer,
		[ProtoEnum(Name = "S2C_UpdatePlayer", Value = 192)]
		S2C_UpdatePlayer = 192,
		[ProtoEnum(Name = "C2S_ChangeSocket", Value = 193)]
		C2S_ChangeSocket,
		[ProtoEnum(Name = "S2C_ChangeSocket", Value = 194)]
		S2C_ChangeSocket,
		[ProtoEnum(Name = "C2S_SetCombatPet", Value = 195)]
		C2S_SetCombatPet,
		[ProtoEnum(Name = "S2C_SetCombatPet", Value = 196)]
		S2C_SetCombatPet,
		[ProtoEnum(Name = "C2S_EquipItem", Value = 197)]
		C2S_EquipItem,
		[ProtoEnum(Name = "S2C_EquipItem", Value = 198)]
		S2C_EquipItem,
		[ProtoEnum(Name = "C2S_AutoEquipItem", Value = 199)]
		C2S_AutoEquipItem,
		[ProtoEnum(Name = "S2C_AutoEquipItem", Value = 200)]
		S2C_AutoEquipItem,
		[ProtoEnum(Name = "C2S_ChangeFashion", Value = 201)]
		C2S_ChangeFashion,
		[ProtoEnum(Name = "S2C_ChangeFashion", Value = 202)]
		S2C_ChangeFashion,
		[ProtoEnum(Name = "C2S_ConstellationLevelup", Value = 203)]
		C2S_ConstellationLevelup,
		[ProtoEnum(Name = "S2C_ConstellationLevelup", Value = 204)]
		S2C_ConstellationLevelup,
		[ProtoEnum(Name = "S2C_UpdateFDSReward", Value = 205)]
		S2C_UpdateFDSReward,
		[ProtoEnum(Name = "C2S_GetDayEnergy", Value = 206)]
		C2S_GetDayEnergy,
		[ProtoEnum(Name = "S2C_GetDayEnergy", Value = 207)]
		S2C_GetDayEnergy,
		[ProtoEnum(Name = "S2C_UpdateFrozenPlay", Value = 208)]
		S2C_UpdateFrozenPlay,
		[ProtoEnum(Name = "C2S_TakeQuestReward", Value = 209)]
		C2S_TakeQuestReward,
		[ProtoEnum(Name = "S2C_TakeQuestReward", Value = 210)]
		S2C_TakeQuestReward,
		[ProtoEnum(Name = "S2C_MailVersionUpdate", Value = 211)]
		S2C_MailVersionUpdate,
		[ProtoEnum(Name = "C2S_GetMailData", Value = 212)]
		C2S_GetMailData,
		[ProtoEnum(Name = "S2C_GetMailData", Value = 213)]
		S2C_GetMailData,
		[ProtoEnum(Name = "C2S_TakeMailAffix", Value = 214)]
		C2S_TakeMailAffix,
		[ProtoEnum(Name = "S2C_TakeMailAffix", Value = 215)]
		S2C_TakeMailAffix,
		[ProtoEnum(Name = "C2S_Chat", Value = 216)]
		C2S_Chat,
		[ProtoEnum(Name = "S2C_Chat", Value = 217)]
		S2C_Chat,
		[ProtoEnum(Name = "S2C_PlayerChat", Value = 218)]
		S2C_PlayerChat,
		[ProtoEnum(Name = "S2C_SystemEvent", Value = 219)]
		S2C_SystemEvent,
		[ProtoEnum(Name = "C2S_Diamond2Money", Value = 220)]
		C2S_Diamond2Money,
		[ProtoEnum(Name = "S2C_Diamond2Money", Value = 221)]
		S2C_Diamond2Money,
		[ProtoEnum(Name = "C2S_BuyEnergy", Value = 222)]
		C2S_BuyEnergy,
		[ProtoEnum(Name = "S2C_BuyEnergy", Value = 223)]
		S2C_BuyEnergy,
		[ProtoEnum(Name = "C2S_LuckyRoll", Value = 224)]
		C2S_LuckyRoll,
		[ProtoEnum(Name = "S2C_LuckyRoll", Value = 225)]
		S2C_LuckyRoll,
		[ProtoEnum(Name = "C2S_SignIn", Value = 226)]
		C2S_SignIn,
		[ProtoEnum(Name = "S2C_SignIn", Value = 227)]
		S2C_SignIn,
		[ProtoEnum(Name = "C2S_TakeLevelReward", Value = 228)]
		C2S_TakeLevelReward,
		[ProtoEnum(Name = "S2C_TakeLevelReward", Value = 229)]
		S2C_TakeLevelReward,
		[ProtoEnum(Name = "C2S_BuyVipReward", Value = 232)]
		C2S_BuyVipReward = 232,
		[ProtoEnum(Name = "S2C_BuyVipReward", Value = 233)]
		S2C_BuyVipReward,
		[ProtoEnum(Name = "C2S_TakeCardDiamond", Value = 234)]
		C2S_TakeCardDiamond,
		[ProtoEnum(Name = "S2C_TakeCardDiamond", Value = 235)]
		S2C_TakeCardDiamond,
		[ProtoEnum(Name = "C2S_TakeSuperCardDiamond", Value = 236)]
		C2S_TakeSuperCardDiamond,
		[ProtoEnum(Name = "S2C_TakeSuperCardDiamond", Value = 237)]
		S2C_TakeSuperCardDiamond,
		[ProtoEnum(Name = "C2S_ChangeName", Value = 238)]
		C2S_ChangeName,
		[ProtoEnum(Name = "S2C_ChangeName", Value = 239)]
		S2C_ChangeName,
		[ProtoEnum(Name = "C2S_TakeFirstPayReward", Value = 240)]
		C2S_TakeFirstPayReward,
		[ProtoEnum(Name = "S2C_TakeFirstPayReward", Value = 241)]
		S2C_TakeFirstPayReward,
		[ProtoEnum(Name = "C2S_TakeDay7Reward", Value = 242)]
		C2S_TakeDay7Reward,
		[ProtoEnum(Name = "S2C_TakeDay7Reward", Value = 243)]
		S2C_TakeDay7Reward,
		[ProtoEnum(Name = "C2S_SaveGuideSteps", Value = 244)]
		C2S_SaveGuideSteps,
		[ProtoEnum(Name = "S2C_AchievementUpdate", Value = 245)]
		S2C_AchievementUpdate,
		[ProtoEnum(Name = "C2S_TakeAchievementReward", Value = 246)]
		C2S_TakeAchievementReward,
		[ProtoEnum(Name = "S2C_TakeAchievementReward", Value = 247)]
		S2C_TakeAchievementReward,
		[ProtoEnum(Name = "C2S_TakeDailyScoreReward", Value = 248)]
		C2S_TakeDailyScoreReward,
		[ProtoEnum(Name = "S2C_TakeDailyScoreReward", Value = 249)]
		S2C_TakeDailyScoreReward,
		[ProtoEnum(Name = "C2S_Share", Value = 250)]
		C2S_Share,
		[ProtoEnum(Name = "S2C_SystemNotice", Value = 254)]
		S2C_SystemNotice = 254,
		[ProtoEnum(Name = "C2S_CreateOrder", Value = 255)]
		C2S_CreateOrder,
		[ProtoEnum(Name = "S2C_CreateOrder", Value = 256)]
		S2C_CreateOrder,
		[ProtoEnum(Name = "S2C_OrderInfo", Value = 257)]
		S2C_OrderInfo,
		[ProtoEnum(Name = "C2S_CheckPayResult", Value = 258)]
		C2S_CheckPayResult,
		[ProtoEnum(Name = "S2C_CheckPayResult", Value = 259)]
		S2C_CheckPayResult,
		[ProtoEnum(Name = "S2C_UpdatePay", Value = 260)]
		S2C_UpdatePay,
		[ProtoEnum(Name = "S2C_UpdateCostumePartyData", Value = 261)]
		S2C_UpdateCostumePartyData,
		[ProtoEnum(Name = "C2S_GetCostumePartyData", Value = 262)]
		C2S_GetCostumePartyData,
		[ProtoEnum(Name = "S2C_GetCostumePartyData", Value = 263)]
		S2C_GetCostumePartyData,
		[ProtoEnum(Name = "C2S_CreateCostumeParty", Value = 264)]
		C2S_CreateCostumeParty,
		[ProtoEnum(Name = "S2C_CreateCostumeParty", Value = 265)]
		S2C_CreateCostumeParty,
		[ProtoEnum(Name = "C2S_JoinCostumeParty", Value = 266)]
		C2S_JoinCostumeParty,
		[ProtoEnum(Name = "S2C_JoinCostumeParty", Value = 267)]
		S2C_JoinCostumeParty,
		[ProtoEnum(Name = "C2S_LeaveCostumeParty", Value = 268)]
		C2S_LeaveCostumeParty,
		[ProtoEnum(Name = "S2C_LeaveCostumeParty", Value = 269)]
		S2C_LeaveCostumeParty,
		[ProtoEnum(Name = "C2S_RemoveGuest", Value = 270)]
		C2S_RemoveGuest,
		[ProtoEnum(Name = "S2C_RemoveGuest", Value = 271)]
		S2C_RemoveGuest,
		[ProtoEnum(Name = "C2S_StartCarnival", Value = 272)]
		C2S_StartCarnival,
		[ProtoEnum(Name = "S2C_StartCarnival", Value = 273)]
		S2C_StartCarnival,
		[ProtoEnum(Name = "C2S_TakeCostumePartyReward", Value = 274)]
		C2S_TakeCostumePartyReward,
		[ProtoEnum(Name = "S2C_TakeCostumePartyReward", Value = 275)]
		S2C_TakeCostumePartyReward,
		[ProtoEnum(Name = "C2S_Interaction", Value = 276)]
		C2S_Interaction,
		[ProtoEnum(Name = "S2C_Interaction", Value = 277)]
		S2C_Interaction,
		[ProtoEnum(Name = "C2S_TakeInteractionReward", Value = 278)]
		C2S_TakeInteractionReward,
		[ProtoEnum(Name = "S2C_TakeInteractionReward", Value = 279)]
		S2C_TakeInteractionReward,
		[ProtoEnum(Name = "S2C_UpdateCostumePartyGuest", Value = 280)]
		S2C_UpdateCostumePartyGuest,
		[ProtoEnum(Name = "S2C_AddGuest", Value = 281)]
		S2C_AddGuest,
		[ProtoEnum(Name = "S2C_InteractionMessage", Value = 282)]
		S2C_InteractionMessage,
		[ProtoEnum(Name = "S2C_RecycleChat", Value = 283)]
		S2C_RecycleChat,
		[ProtoEnum(Name = "C2S_CombatValueRank", Value = 284)]
		C2S_CombatValueRank,
		[ProtoEnum(Name = "S2C_CombatValueRank", Value = 285)]
		S2C_CombatValueRank,
		[ProtoEnum(Name = "C2S_QueryRemotePlayer", Value = 286)]
		C2S_QueryRemotePlayer,
		[ProtoEnum(Name = "S2C_QueryRemotePlayer", Value = 287)]
		S2C_QueryRemotePlayer,
		[ProtoEnum(Name = "C2S_GradeReward", Value = 288)]
		C2S_GradeReward,
		[ProtoEnum(Name = "S2C_GradeReward", Value = 289)]
		S2C_GradeReward,
		[ProtoEnum(Name = "C2S_PVEStarsRank", Value = 290)]
		C2S_PVEStarsRank,
		[ProtoEnum(Name = "S2C_PVEStarsRank", Value = 291)]
		S2C_PVEStarsRank,
		[ProtoEnum(Name = "C2S_TakeICountReward", Value = 292)]
		C2S_TakeICountReward,
		[ProtoEnum(Name = "S2C_TakeICountReward", Value = 293)]
		S2C_TakeICountReward,
		[ProtoEnum(Name = "C2S_LevelRank", Value = 294)]
		C2S_LevelRank,
		[ProtoEnum(Name = "S2C_LevelRank", Value = 295)]
		S2C_LevelRank,
		[ProtoEnum(Name = "S2C_CommentMsg", Value = 296)]
		S2C_CommentMsg,
		[ProtoEnum(Name = "C2S_CloseComment", Value = 297)]
		C2S_CloseComment,
		[ProtoEnum(Name = "C2S_GetFamePlayers", Value = 298)]
		C2S_GetFamePlayers,
		[ProtoEnum(Name = "S2C_GetFamePlayers", Value = 299)]
		S2C_GetFamePlayers,
		[ProtoEnum(Name = "C2S_PraisePlayer", Value = 300)]
		C2S_PraisePlayer,
		[ProtoEnum(Name = "S2C_PraisePlayer", Value = 301)]
		S2C_PraisePlayer,
		[ProtoEnum(Name = "C2S_SetFameMessage", Value = 302)]
		C2S_SetFameMessage,
		[ProtoEnum(Name = "S2C_SetFameMessage", Value = 303)]
		S2C_SetFameMessage,
		[ProtoEnum(Name = "S2C_AddFriendData", Value = 304)]
		S2C_AddFriendData,
		[ProtoEnum(Name = "S2C_RemoveFriendData", Value = 305)]
		S2C_RemoveFriendData,
		[ProtoEnum(Name = "S2C_UpdateFriendData", Value = 306)]
		S2C_UpdateFriendData,
		[ProtoEnum(Name = "C2S_RecommendFriend", Value = 307)]
		C2S_RecommendFriend,
		[ProtoEnum(Name = "S2C_RecommendFriend", Value = 308)]
		S2C_RecommendFriend,
		[ProtoEnum(Name = "C2S_RequestFriend", Value = 309)]
		C2S_RequestFriend,
		[ProtoEnum(Name = "S2C_RequestFriend", Value = 310)]
		S2C_RequestFriend,
		[ProtoEnum(Name = "C2S_ReplyFriend", Value = 311)]
		C2S_ReplyFriend,
		[ProtoEnum(Name = "S2C_ReplyFriend", Value = 312)]
		S2C_ReplyFriend,
		[ProtoEnum(Name = "C2S_RemoveFriend", Value = 313)]
		C2S_RemoveFriend,
		[ProtoEnum(Name = "S2C_RemoveFriend", Value = 314)]
		S2C_RemoveFriend,
		[ProtoEnum(Name = "C2S_AddBlackList", Value = 315)]
		C2S_AddBlackList,
		[ProtoEnum(Name = "S2C_AddBlackList", Value = 316)]
		S2C_AddBlackList,
		[ProtoEnum(Name = "C2S_RemoveBlackList", Value = 317)]
		C2S_RemoveBlackList,
		[ProtoEnum(Name = "S2C_RemoveBlackList", Value = 318)]
		S2C_RemoveBlackList,
		[ProtoEnum(Name = "C2S_GiveFriendEnergy", Value = 319)]
		C2S_GiveFriendEnergy,
		[ProtoEnum(Name = "S2C_GiveFriendEnergy", Value = 320)]
		S2C_GiveFriendEnergy,
		[ProtoEnum(Name = "C2S_TakeFriendEnergy", Value = 321)]
		C2S_TakeFriendEnergy,
		[ProtoEnum(Name = "S2C_TakeFriendEnergy", Value = 322)]
		S2C_TakeFriendEnergy,
		[ProtoEnum(Name = "S2C_UpdateEnergyFlag", Value = 323)]
		S2C_UpdateEnergyFlag,
		[ProtoEnum(Name = "C2S_GetGuildWarHeros", Value = 324)]
		C2S_GetGuildWarHeros,
		[ProtoEnum(Name = "S2C_GetGuildWarHeros", Value = 325)]
		S2C_GetGuildWarHeros,
		[ProtoEnum(Name = "C2S_PraiseGuildWarHero", Value = 326)]
		C2S_PraiseGuildWarHero,
		[ProtoEnum(Name = "S2C_PraiseGuildWarHero", Value = 327)]
		S2C_PraiseGuildWarHero,
		[ProtoEnum(Name = "C2S_SetGuildWarHeroMessage", Value = 328)]
		C2S_SetGuildWarHeroMessage,
		[ProtoEnum(Name = "S2C_SetGuildWarHeroMessage", Value = 329)]
		S2C_SetGuildWarHeroMessage,
		[ProtoEnum(Name = "C2S_RequestSMSCode", Value = 394)]
		C2S_RequestSMSCode = 394,
		[ProtoEnum(Name = "S2C_RequestSMSCode", Value = 395)]
		S2C_RequestSMSCode,
		[ProtoEnum(Name = "C2S_VerifySMSCode", Value = 396)]
		C2S_VerifySMSCode,
		[ProtoEnum(Name = "S2C_VerifySMSCode", Value = 397)]
		S2C_VerifySMSCode,
		[ProtoEnum(Name = "C2S_IAPCheckPayResult", Value = 398)]
		C2S_IAPCheckPayResult,
		[ProtoEnum(Name = "S2C_IAPCheckPayResult", Value = 399)]
		S2C_IAPCheckPayResult,
		[ProtoEnum(Name = "C2S_PetLevelup", Value = 402)]
		C2S_PetLevelup = 402,
		[ProtoEnum(Name = "S2C_PetLevelup", Value = 403)]
		S2C_PetLevelup,
		[ProtoEnum(Name = "C2S_PetFurther", Value = 404)]
		C2S_PetFurther,
		[ProtoEnum(Name = "S2C_PetFurther", Value = 405)]
		S2C_PetFurther,
		[ProtoEnum(Name = "C2S_PetSkill", Value = 406)]
		C2S_PetSkill,
		[ProtoEnum(Name = "S2C_PetSkill", Value = 407)]
		S2C_PetSkill,
		[ProtoEnum(Name = "S2C_AddPet", Value = 410)]
		S2C_AddPet = 410,
		[ProtoEnum(Name = "S2C_PetUpdate", Value = 411)]
		S2C_PetUpdate,
		[ProtoEnum(Name = "S2C_PetRemove", Value = 412)]
		S2C_PetRemove,
		[ProtoEnum(Name = "C2S_PetBreakUp", Value = 413)]
		C2S_PetBreakUp,
		[ProtoEnum(Name = "S2C_PetBreakUp", Value = 414)]
		S2C_PetBreakUp,
		[ProtoEnum(Name = "C2S_PetReborn", Value = 415)]
		C2S_PetReborn,
		[ProtoEnum(Name = "S2C_PetReborn", Value = 416)]
		S2C_PetReborn,
		[ProtoEnum(Name = "C2S_EquipAwakeItem", Value = 417)]
		C2S_EquipAwakeItem,
		[ProtoEnum(Name = "S2C_EquipAwakeItem", Value = 418)]
		S2C_EquipAwakeItem,
		[ProtoEnum(Name = "C2S_AwakeLevelup", Value = 419)]
		C2S_AwakeLevelup,
		[ProtoEnum(Name = "S2C_AwakeLevelup", Value = 420)]
		S2C_AwakeLevelup,
		[ProtoEnum(Name = "C2S_PetCultivate", Value = 421)]
		C2S_PetCultivate,
		[ProtoEnum(Name = "S2C_PetCultivate", Value = 422)]
		S2C_PetCultivate,
		[ProtoEnum(Name = "C2S_PetCultivateAck", Value = 423)]
		C2S_PetCultivateAck,
		[ProtoEnum(Name = "S2C_PetCultivateAck", Value = 424)]
		S2C_PetCultivateAck,
		[ProtoEnum(Name = "C2S_PetExchange", Value = 425)]
		C2S_PetExchange,
		[ProtoEnum(Name = "S2C_PetExchange", Value = 426)]
		S2C_PetExchange,
		[ProtoEnum(Name = "S2C_AddItem", Value = 500)]
		S2C_AddItem = 500,
		[ProtoEnum(Name = "S2C_UpdateItem", Value = 501)]
		S2C_UpdateItem,
		[ProtoEnum(Name = "C2S_EquipCreate", Value = 502)]
		C2S_EquipCreate,
		[ProtoEnum(Name = "S2C_EquipCreate", Value = 503)]
		S2C_EquipCreate,
		[ProtoEnum(Name = "C2S_SummonPet", Value = 504)]
		C2S_SummonPet,
		[ProtoEnum(Name = "S2C_SummonPet", Value = 505)]
		S2C_SummonPet,
		[ProtoEnum(Name = "C2S_TrinketCreate", Value = 506)]
		C2S_TrinketCreate,
		[ProtoEnum(Name = "S2C_TrinketCreate", Value = 507)]
		S2C_TrinketCreate,
		[ProtoEnum(Name = "C2S_OpenItem", Value = 508)]
		C2S_OpenItem,
		[ProtoEnum(Name = "S2C_OpenItem", Value = 509)]
		S2C_OpenItem,
		[ProtoEnum(Name = "C2S_SellItem", Value = 510)]
		C2S_SellItem,
		[ProtoEnum(Name = "S2C_SellItem", Value = 511)]
		S2C_SellItem,
		[ProtoEnum(Name = "C2S_GetShopData", Value = 512)]
		C2S_GetShopData,
		[ProtoEnum(Name = "S2C_GetShopData", Value = 513)]
		S2C_GetShopData,
		[ProtoEnum(Name = "C2S_ShopBuyItem", Value = 514)]
		C2S_ShopBuyItem,
		[ProtoEnum(Name = "S2C_ShopBuyItem", Value = 515)]
		S2C_ShopBuyItem,
		[ProtoEnum(Name = "C2S_UseItem", Value = 516)]
		C2S_UseItem,
		[ProtoEnum(Name = "S2C_UseItem", Value = 517)]
		S2C_UseItem,
		[ProtoEnum(Name = "S2C_UpdateItemData", Value = 518)]
		S2C_UpdateItemData,
		[ProtoEnum(Name = "C2S_EquipEnhance", Value = 520)]
		C2S_EquipEnhance = 520,
		[ProtoEnum(Name = "S2C_EquipEnhance", Value = 521)]
		S2C_EquipEnhance,
		[ProtoEnum(Name = "C2S_EquipRefine", Value = 522)]
		C2S_EquipRefine,
		[ProtoEnum(Name = "S2C_EquipRefine", Value = 523)]
		S2C_EquipRefine,
		[ProtoEnum(Name = "C2S_TrinketEnhance", Value = 524)]
		C2S_TrinketEnhance,
		[ProtoEnum(Name = "S2C_TrinketEnhance", Value = 525)]
		S2C_TrinketEnhance,
		[ProtoEnum(Name = "C2S_TrinketRefine", Value = 526)]
		C2S_TrinketRefine,
		[ProtoEnum(Name = "S2C_TrinketRefine", Value = 527)]
		S2C_TrinketRefine,
		[ProtoEnum(Name = "S2C_AddFashion", Value = 528)]
		S2C_AddFashion,
		[ProtoEnum(Name = "S2C_RemoveFashion", Value = 529)]
		S2C_RemoveFashion,
		[ProtoEnum(Name = "C2S_EquipBreakUp", Value = 530)]
		C2S_EquipBreakUp,
		[ProtoEnum(Name = "S2C_EquipBreakUp", Value = 531)]
		S2C_EquipBreakUp,
		[ProtoEnum(Name = "C2S_TrinketReborn", Value = 532)]
		C2S_TrinketReborn,
		[ProtoEnum(Name = "S2C_TrinketReborn", Value = 533)]
		S2C_TrinketReborn,
		[ProtoEnum(Name = "C2S_ShopBuyFashion", Value = 534)]
		C2S_ShopBuyFashion,
		[ProtoEnum(Name = "S2C_ShopBuyFashion", Value = 535)]
		S2C_ShopBuyFashion,
		[ProtoEnum(Name = "C2S_AwakeItemCreate", Value = 536)]
		C2S_AwakeItemCreate,
		[ProtoEnum(Name = "S2C_AwakeItemCreate", Value = 537)]
		S2C_AwakeItemCreate,
		[ProtoEnum(Name = "C2S_AwakeItemBreakUp", Value = 538)]
		C2S_AwakeItemBreakUp,
		[ProtoEnum(Name = "S2C_AwakeItemBreakUp", Value = 539)]
		S2C_AwakeItemBreakUp,
		[ProtoEnum(Name = "C2S_OpenSelectBox", Value = 540)]
		C2S_OpenSelectBox,
		[ProtoEnum(Name = "S2C_OpenSelectBox", Value = 541)]
		S2C_OpenSelectBox,
		[ProtoEnum(Name = "C2S_TrinketCompound", Value = 542)]
		C2S_TrinketCompound,
		[ProtoEnum(Name = "S2C_TrinketCompound", Value = 543)]
		S2C_TrinketCompound,
		[ProtoEnum(Name = "C2S_OpenRewardBox", Value = 544)]
		C2S_OpenRewardBox,
		[ProtoEnum(Name = "S2C_OpenRewardBox", Value = 545)]
		S2C_OpenRewardBox,
		[ProtoEnum(Name = "S2C_UpdateFashion", Value = 546)]
		S2C_UpdateFashion,
		[ProtoEnum(Name = "C2S_PveStart", Value = 600)]
		C2S_PveStart = 600,
		[ProtoEnum(Name = "S2C_PveStart", Value = 601)]
		S2C_PveStart,
		[ProtoEnum(Name = "C2S_PveResult", Value = 602)]
		C2S_PveResult,
		[ProtoEnum(Name = "S2C_PveResult", Value = 603)]
		S2C_PveResult,
		[ProtoEnum(Name = "C2S_Farm", Value = 604)]
		C2S_Farm,
		[ProtoEnum(Name = "S2C_Farm", Value = 605)]
		S2C_Farm,
		[ProtoEnum(Name = "C2S_QueryTrialRank", Value = 606)]
		C2S_QueryTrialRank,
		[ProtoEnum(Name = "S2C_QueryTrialRank", Value = 607)]
		S2C_QueryTrialRank,
		[ProtoEnum(Name = "S2C_SceneScore", Value = 608)]
		S2C_SceneScore,
		[ProtoEnum(Name = "C2S_TakeMapReward", Value = 609)]
		C2S_TakeMapReward,
		[ProtoEnum(Name = "S2C_TakeMapReward", Value = 610)]
		S2C_TakeMapReward,
		[ProtoEnum(Name = "S2C_MapReward", Value = 611)]
		S2C_MapReward,
		[ProtoEnum(Name = "C2S_ResetSceneCD", Value = 612)]
		C2S_ResetSceneCD,
		[ProtoEnum(Name = "S2C_ResetSceneCD", Value = 613)]
		S2C_ResetSceneCD,
		[ProtoEnum(Name = "C2S_GetBossData", Value = 614)]
		C2S_GetBossData,
		[ProtoEnum(Name = "S2C_GetBossData", Value = 615)]
		S2C_GetBossData,
		[ProtoEnum(Name = "S2C_BossRespawn", Value = 616)]
		S2C_BossRespawn,
		[ProtoEnum(Name = "S2C_BossDead", Value = 617)]
		S2C_BossDead,
		[ProtoEnum(Name = "C2S_DoBossDamage", Value = 618)]
		C2S_DoBossDamage,
		[ProtoEnum(Name = "S2C_DoBossDamage", Value = 619)]
		S2C_DoBossDamage,
		[ProtoEnum(Name = "S2C_BroadcastDamage", Value = 620)]
		S2C_BroadcastDamage,
		[ProtoEnum(Name = "S2C_BroadcastDamageRank", Value = 621)]
		S2C_BroadcastDamageRank,
		[ProtoEnum(Name = "C2S_WorldBossResurrect", Value = 622)]
		C2S_WorldBossResurrect,
		[ProtoEnum(Name = "S2C_WorldBossResurrect", Value = 623)]
		S2C_WorldBossResurrect,
		[ProtoEnum(Name = "C2S_WorldBossStart", Value = 624)]
		C2S_WorldBossStart,
		[ProtoEnum(Name = "S2C_WorldBossStart", Value = 625)]
		S2C_WorldBossStart,
		[ProtoEnum(Name = "C2S_TakeWorldBossReward", Value = 626)]
		C2S_TakeWorldBossReward,
		[ProtoEnum(Name = "S2C_TakeWorldBossReward", Value = 627)]
		S2C_TakeWorldBossReward,
		[ProtoEnum(Name = "C2S_TrialStart", Value = 628)]
		C2S_TrialStart,
		[ProtoEnum(Name = "S2C_TrialStart", Value = 629)]
		S2C_TrialStart,
		[ProtoEnum(Name = "C2S_TrialWave", Value = 630)]
		C2S_TrialWave,
		[ProtoEnum(Name = "S2C_TrialWave", Value = 631)]
		S2C_TrialWave,
		[ProtoEnum(Name = "C2S_TrialReset", Value = 632)]
		C2S_TrialReset,
		[ProtoEnum(Name = "S2C_TrialReset", Value = 633)]
		S2C_TrialReset,
		[ProtoEnum(Name = "C2S_TrialFarmStart", Value = 634)]
		C2S_TrialFarmStart,
		[ProtoEnum(Name = "S2C_TrialFarmStart", Value = 635)]
		S2C_TrialFarmStart,
		[ProtoEnum(Name = "C2S_TrialFarmStop", Value = 636)]
		C2S_TrialFarmStop,
		[ProtoEnum(Name = "S2C_TrialFarmStop", Value = 637)]
		S2C_TrialFarmStop,
		[ProtoEnum(Name = "C2S_TakeWorldBossDamageReward", Value = 638)]
		C2S_TakeWorldBossDamageReward,
		[ProtoEnum(Name = "S2C_TakeWorldBossDamageReward", Value = 639)]
		S2C_TakeWorldBossDamageReward,
		[ProtoEnum(Name = "C2S_TrialQuit", Value = 640)]
		C2S_TrialQuit,
		[ProtoEnum(Name = "C2S_GetKRData", Value = 641)]
		C2S_GetKRData,
		[ProtoEnum(Name = "S2C_GetKRData", Value = 642)]
		S2C_GetKRData,
		[ProtoEnum(Name = "C2S_OneKeyKR", Value = 643)]
		C2S_OneKeyKR,
		[ProtoEnum(Name = "S2C_OneKeyKR", Value = 644)]
		S2C_OneKeyKR,
		[ProtoEnum(Name = "C2S_GetBossRank", Value = 645)]
		C2S_GetBossRank,
		[ProtoEnum(Name = "S2C_GetBossRank", Value = 646)]
		S2C_GetBossRank,
		[ProtoEnum(Name = "C2S_TakeFDSReward", Value = 647)]
		C2S_TakeFDSReward,
		[ProtoEnum(Name = "S2C_TakeFDSReward", Value = 648)]
		S2C_TakeFDSReward,
		[ProtoEnum(Name = "C2S_CombatLog", Value = 649)]
		C2S_CombatLog,
		[ProtoEnum(Name = "C2S_TakeKillWorldBossReward", Value = 650)]
		C2S_TakeKillWorldBossReward,
		[ProtoEnum(Name = "S2C_TakeKillWorldBossReward", Value = 651)]
		S2C_TakeKillWorldBossReward,
		[ProtoEnum(Name = "C2S_MGFarm", Value = 652)]
		C2S_MGFarm,
		[ProtoEnum(Name = "S2C_MGFarm", Value = 653)]
		S2C_MGFarm,
		[ProtoEnum(Name = "C2S_GetDartData", Value = 700)]
		C2S_GetDartData = 700,
		[ProtoEnum(Name = "S2C_GetDartData", Value = 701)]
		S2C_GetDartData,
		[ProtoEnum(Name = "C2S_StartDart", Value = 702)]
		C2S_StartDart,
		[ProtoEnum(Name = "S2C_StartDart", Value = 703)]
		S2C_StartDart,
		[ProtoEnum(Name = "C2S_GetLuckyDrawData", Value = 704)]
		C2S_GetLuckyDrawData,
		[ProtoEnum(Name = "S2C_GetLuckyDrawData", Value = 705)]
		S2C_GetLuckyDrawData,
		[ProtoEnum(Name = "C2S_GetScratchOffData", Value = 708)]
		C2S_GetScratchOffData = 708,
		[ProtoEnum(Name = "S2C_GetScratchOffData", Value = 709)]
		S2C_GetScratchOffData,
		[ProtoEnum(Name = "C2S_StartScratchOff", Value = 710)]
		C2S_StartScratchOff,
		[ProtoEnum(Name = "S2C_StartScratchOff", Value = 711)]
		S2C_StartScratchOff,
		[ProtoEnum(Name = "C2S_GetFlashSaleData", Value = 712)]
		C2S_GetFlashSaleData,
		[ProtoEnum(Name = "S2C_GetFlashSaleData", Value = 713)]
		S2C_GetFlashSaleData,
		[ProtoEnum(Name = "C2S_StartFlashSale", Value = 714)]
		C2S_StartFlashSale,
		[ProtoEnum(Name = "S2C_StartFlashSale", Value = 715)]
		S2C_StartFlashSale,
		[ProtoEnum(Name = "S2C_ActivityAchievementUpdate", Value = 716)]
		S2C_ActivityAchievementUpdate,
		[ProtoEnum(Name = "S2C_ActivityValueUpdate", Value = 717)]
		S2C_ActivityValueUpdate,
		[ProtoEnum(Name = "S2C_UpdateAAItem", Value = 718)]
		S2C_UpdateAAItem,
		[ProtoEnum(Name = "C2S_TakeAAReward", Value = 719)]
		C2S_TakeAAReward,
		[ProtoEnum(Name = "S2C_TakeAAReward", Value = 720)]
		S2C_TakeAAReward,
		[ProtoEnum(Name = "S2C_UpdateSevenDayReward", Value = 721)]
		S2C_UpdateSevenDayReward,
		[ProtoEnum(Name = "C2S_TakeSevenDayReward", Value = 722)]
		C2S_TakeSevenDayReward,
		[ProtoEnum(Name = "S2C_TakeSevenDayReward", Value = 723)]
		S2C_TakeSevenDayReward,
		[ProtoEnum(Name = "C2S_ExchangeGiftCode", Value = 724)]
		C2S_ExchangeGiftCode,
		[ProtoEnum(Name = "S2C_ExchangeGiftCode", Value = 725)]
		S2C_ExchangeGiftCode,
		[ProtoEnum(Name = "S2C_UpdateLuckyDrawRankList", Value = 726)]
		S2C_UpdateLuckyDrawRankList,
		[ProtoEnum(Name = "C2S_ApplyCSAServerToken", Value = 727)]
		C2S_ApplyCSAServerToken,
		[ProtoEnum(Name = "S2C_ApplyCSAServerToken", Value = 728)]
		S2C_ApplyCSAServerToken,
		[ProtoEnum(Name = "S2C_UpdateShareAchievement", Value = 730)]
		S2C_UpdateShareAchievement = 730,
		[ProtoEnum(Name = "C2S_ShareAchievement", Value = 731)]
		C2S_ShareAchievement,
		[ProtoEnum(Name = "S2C_ShareAchievement", Value = 732)]
		S2C_ShareAchievement,
		[ProtoEnum(Name = "C2S_TakeShareAchievementReward", Value = 733)]
		C2S_TakeShareAchievementReward,
		[ProtoEnum(Name = "S2C_TakeShareAchievementReward", Value = 734)]
		S2C_TakeShareAchievementReward,
		[ProtoEnum(Name = "C2S_BuyFund", Value = 735)]
		C2S_BuyFund,
		[ProtoEnum(Name = "S2C_BuyFund", Value = 736)]
		S2C_BuyFund,
		[ProtoEnum(Name = "C2S_TakeFundLevelReward", Value = 737)]
		C2S_TakeFundLevelReward,
		[ProtoEnum(Name = "S2C_TakeFundLevelReward", Value = 738)]
		S2C_TakeFundLevelReward,
		[ProtoEnum(Name = "C2S_TakeWelfare", Value = 739)]
		C2S_TakeWelfare,
		[ProtoEnum(Name = "S2C_TakeWelfare", Value = 740)]
		S2C_TakeWelfare,
		[ProtoEnum(Name = "C2S_TakeHotTimeReward", Value = 741)]
		C2S_TakeHotTimeReward,
		[ProtoEnum(Name = "S2C_TakeHotTimeReward", Value = 742)]
		S2C_TakeHotTimeReward,
		[ProtoEnum(Name = "C2S_GetActivityDesc", Value = 743)]
		C2S_GetActivityDesc,
		[ProtoEnum(Name = "S2C_GetActivityDesc", Value = 744)]
		S2C_GetActivityDesc,
		[ProtoEnum(Name = "C2S_GetActivityTitle", Value = 745)]
		C2S_GetActivityTitle,
		[ProtoEnum(Name = "S2C_GetActivityTitle", Value = 746)]
		S2C_GetActivityTitle,
		[ProtoEnum(Name = "C2S_HotTimeData", Value = 747)]
		C2S_HotTimeData,
		[ProtoEnum(Name = "S2C_HotTimeData", Value = 748)]
		S2C_HotTimeData,
		[ProtoEnum(Name = "S2C_UpdateBuyFundNum", Value = 749)]
		S2C_UpdateBuyFundNum,
		[ProtoEnum(Name = "S2C_ActivityShopUpdate", Value = 750)]
		S2C_ActivityShopUpdate,
		[ProtoEnum(Name = "C2S_GetActivityShopData", Value = 751)]
		C2S_GetActivityShopData,
		[ProtoEnum(Name = "S2C_GetActivityShopData", Value = 752)]
		S2C_GetActivityShopData,
		[ProtoEnum(Name = "C2S_BuyActivityShopItem", Value = 753)]
		C2S_BuyActivityShopItem,
		[ProtoEnum(Name = "S2C_BuyActivityShopItem", Value = 754)]
		S2C_BuyActivityShopItem,
		[ProtoEnum(Name = "C2S_TakeDayHotReward", Value = 755)]
		C2S_TakeDayHotReward,
		[ProtoEnum(Name = "S2C_TakeDayHotReward", Value = 756)]
		S2C_TakeDayHotReward,
		[ProtoEnum(Name = "S2C_ActivityPayUpdate", Value = 757)]
		S2C_ActivityPayUpdate,
		[ProtoEnum(Name = "S2C_ActivityPayShopUpdate", Value = 758)]
		S2C_ActivityPayShopUpdate,
		[ProtoEnum(Name = "C2S_BuyActivityPayShopItem", Value = 759)]
		C2S_BuyActivityPayShopItem,
		[ProtoEnum(Name = "S2C_BuyActivityPayShopItem", Value = 760)]
		S2C_BuyActivityPayShopItem,
		[ProtoEnum(Name = "S2C_UpdateActivityPayDay", Value = 761)]
		S2C_UpdateActivityPayDay,
		[ProtoEnum(Name = "C2S_ActivityPayTodayNoPopup", Value = 762)]
		C2S_ActivityPayTodayNoPopup,
		[ProtoEnum(Name = "S2C_ActivityRollEquipUpdate", Value = 763)]
		S2C_ActivityRollEquipUpdate,
		[ProtoEnum(Name = "C2S_RollEquip", Value = 764)]
		C2S_RollEquip,
		[ProtoEnum(Name = "S2C_RollEquip", Value = 765)]
		S2C_RollEquip,
		[ProtoEnum(Name = "S2C_ActivitySpecifyPayUpdate", Value = 766)]
		S2C_ActivitySpecifyPayUpdate,
		[ProtoEnum(Name = "S2C_UpdatePayItem", Value = 767)]
		S2C_UpdatePayItem,
		[ProtoEnum(Name = "C2S_TakePayReward", Value = 768)]
		C2S_TakePayReward,
		[ProtoEnum(Name = "S2C_TakePayReward", Value = 769)]
		S2C_TakePayReward,
		[ProtoEnum(Name = "S2C_ActivityNationalDayUpdate", Value = 770)]
		S2C_ActivityNationalDayUpdate,
		[ProtoEnum(Name = "C2S_TakeNationalDayReward", Value = 771)]
		C2S_TakeNationalDayReward,
		[ProtoEnum(Name = "S2C_TakeNationalDayReward", Value = 772)]
		S2C_TakeNationalDayReward,
		[ProtoEnum(Name = "C2S_ActivityGroupBuyingInfo", Value = 773)]
		C2S_ActivityGroupBuyingInfo,
		[ProtoEnum(Name = "S2C_ActivityGroupBuyingInfo", Value = 774)]
		S2C_ActivityGroupBuyingInfo,
		[ProtoEnum(Name = "C2S_ActivityGroupBuyingBuy", Value = 775)]
		C2S_ActivityGroupBuyingBuy,
		[ProtoEnum(Name = "S2C_ActivityGroupBuyingBuy", Value = 776)]
		S2C_ActivityGroupBuyingBuy,
		[ProtoEnum(Name = "C2S_ActivityGroupBuyingScoreReward", Value = 777)]
		C2S_ActivityGroupBuyingScoreReward,
		[ProtoEnum(Name = "S2C_ActivityGroupBuyingScoreReward", Value = 778)]
		S2C_ActivityGroupBuyingScoreReward,
		[ProtoEnum(Name = "S2C_ActivityGroupBuyingOpen", Value = 779)]
		S2C_ActivityGroupBuyingOpen,
		[ProtoEnum(Name = "S2C_ActivityHalloweenUpdate", Value = 780)]
		S2C_ActivityHalloweenUpdate,
		[ProtoEnum(Name = "C2S_ActivityHalloweenInfo", Value = 781)]
		C2S_ActivityHalloweenInfo,
		[ProtoEnum(Name = "S2C_ActivityHalloweenInfo", Value = 782)]
		S2C_ActivityHalloweenInfo,
		[ProtoEnum(Name = "C2S_ActivityHalloweenBuy", Value = 783)]
		C2S_ActivityHalloweenBuy,
		[ProtoEnum(Name = "S2C_ActivityHalloweenBuy", Value = 784)]
		S2C_ActivityHalloweenBuy,
		[ProtoEnum(Name = "C2S_ActivityHalloweenContract", Value = 785)]
		C2S_ActivityHalloweenContract,
		[ProtoEnum(Name = "S2C_ActivityHalloweenContract", Value = 786)]
		S2C_ActivityHalloweenContract,
		[ProtoEnum(Name = "C2S_ActivityHalloweenScoreReward", Value = 787)]
		C2S_ActivityHalloweenScoreReward,
		[ProtoEnum(Name = "S2C_ActivityHalloweenScoreReward", Value = 788)]
		S2C_ActivityHalloweenScoreReward,
		[ProtoEnum(Name = "C2S_QueryArenaData", Value = 801)]
		C2S_QueryArenaData = 801,
		[ProtoEnum(Name = "S2C_QueryArenaData", Value = 802)]
		S2C_QueryArenaData,
		[ProtoEnum(Name = "C2S_PvpArenaStart", Value = 803)]
		C2S_PvpArenaStart,
		[ProtoEnum(Name = "S2C_PvpArenaStart", Value = 804)]
		S2C_PvpArenaStart,
		[ProtoEnum(Name = "C2S_PvpCombatStart", Value = 805)]
		C2S_PvpCombatStart,
		[ProtoEnum(Name = "C2S_PvpArenaResult", Value = 806)]
		C2S_PvpArenaResult,
		[ProtoEnum(Name = "S2C_PvpArenaResult", Value = 807)]
		S2C_PvpArenaResult,
		[ProtoEnum(Name = "C2S_QueryArenaRank", Value = 808)]
		C2S_QueryArenaRank,
		[ProtoEnum(Name = "S2C_QueryArenaRank", Value = 809)]
		S2C_QueryArenaRank,
		[ProtoEnum(Name = "C2S_QueryPvpRecord", Value = 810)]
		C2S_QueryPvpRecord,
		[ProtoEnum(Name = "S2C_QueryPvpRecord", Value = 811)]
		S2C_QueryPvpRecord,
		[ProtoEnum(Name = "C2S_BuyHonorItem", Value = 812)]
		C2S_BuyHonorItem,
		[ProtoEnum(Name = "S2C_BuyHonorItem", Value = 813)]
		S2C_BuyHonorItem,
		[ProtoEnum(Name = "C2S_QueryPillageTarget", Value = 814)]
		C2S_QueryPillageTarget,
		[ProtoEnum(Name = "S2C_QueryPillageTarget", Value = 815)]
		S2C_QueryPillageTarget,
		[ProtoEnum(Name = "C2S_PvpPillageStart", Value = 816)]
		C2S_PvpPillageStart,
		[ProtoEnum(Name = "S2C_PvpPillageStart", Value = 817)]
		S2C_PvpPillageStart,
		[ProtoEnum(Name = "C2S_PvpPillageResult", Value = 818)]
		C2S_PvpPillageResult,
		[ProtoEnum(Name = "S2C_PvpPillageResult", Value = 819)]
		S2C_PvpPillageResult,
		[ProtoEnum(Name = "C2S_QueryPillageRecord", Value = 820)]
		C2S_QueryPillageRecord,
		[ProtoEnum(Name = "S2C_QueryPillageRecord", Value = 821)]
		S2C_QueryPillageRecord,
		[ProtoEnum(Name = "C2S_PvpPillageFarm", Value = 822)]
		C2S_PvpPillageFarm,
		[ProtoEnum(Name = "S2C_PvpPillageFarm", Value = 823)]
		S2C_PvpPillageFarm,
		[ProtoEnum(Name = "C2S_PvpFarm", Value = 824)]
		C2S_PvpFarm,
		[ProtoEnum(Name = "S2C_PvpFarm", Value = 825)]
		S2C_PvpFarm,
		[ProtoEnum(Name = "C2S_PvpOneKeyPillage", Value = 826)]
		C2S_PvpOneKeyPillage,
		[ProtoEnum(Name = "S2C_PvpOneKeyPillage", Value = 827)]
		S2C_PvpOneKeyPillage,
		[ProtoEnum(Name = "S2C_InitGuildData", Value = 900)]
		S2C_InitGuildData = 900,
		[ProtoEnum(Name = "C2S_GetGuildData", Value = 901)]
		C2S_GetGuildData,
		[ProtoEnum(Name = "S2C_GetGuildData", Value = 902)]
		S2C_GetGuildData,
		[ProtoEnum(Name = "C2S_CreateGuild", Value = 903)]
		C2S_CreateGuild,
		[ProtoEnum(Name = "S2C_CreateGuild", Value = 904)]
		S2C_CreateGuild,
		[ProtoEnum(Name = "C2S_LeaveGuild", Value = 905)]
		C2S_LeaveGuild,
		[ProtoEnum(Name = "S2C_LeaveGuild", Value = 906)]
		S2C_LeaveGuild,
		[ProtoEnum(Name = "C2S_RemoveGuildMember", Value = 907)]
		C2S_RemoveGuildMember,
		[ProtoEnum(Name = "S2C_RemoveGuildMember", Value = 908)]
		S2C_RemoveGuildMember,
		[ProtoEnum(Name = "C2S_GetGuildList", Value = 909)]
		C2S_GetGuildList,
		[ProtoEnum(Name = "S2C_GetGuildList", Value = 910)]
		S2C_GetGuildList,
		[ProtoEnum(Name = "C2S_GuildApply", Value = 911)]
		C2S_GuildApply,
		[ProtoEnum(Name = "S2C_GuildApply", Value = 912)]
		S2C_GuildApply,
		[ProtoEnum(Name = "C2S_GetGuildApplication", Value = 913)]
		C2S_GetGuildApplication,
		[ProtoEnum(Name = "S2C_GetGuildApplication", Value = 914)]
		S2C_GetGuildApplication,
		[ProtoEnum(Name = "C2S_ProcessGuildApplication", Value = 915)]
		C2S_ProcessGuildApplication,
		[ProtoEnum(Name = "S2C_ProcessGuildApplication", Value = 916)]
		S2C_ProcessGuildApplication,
		[ProtoEnum(Name = "C2S_GuildAppoint", Value = 917)]
		C2S_GuildAppoint,
		[ProtoEnum(Name = "S2C_GuildAppoint", Value = 918)]
		S2C_GuildAppoint,
		[ProtoEnum(Name = "C2S_SetGuildManifesto", Value = 919)]
		C2S_SetGuildManifesto,
		[ProtoEnum(Name = "S2C_SetGuildManifesto", Value = 920)]
		S2C_SetGuildManifesto,
		[ProtoEnum(Name = "C2S_QueryGuildEvent", Value = 921)]
		C2S_QueryGuildEvent,
		[ProtoEnum(Name = "S2C_QueryGuildEvent", Value = 922)]
		S2C_QueryGuildEvent,
		[ProtoEnum(Name = "C2S_ImpeachGuildMaster", Value = 923)]
		C2S_ImpeachGuildMaster,
		[ProtoEnum(Name = "S2C_ImpeachGuildMaster", Value = 924)]
		S2C_ImpeachGuildMaster,
		[ProtoEnum(Name = "C2S_SupportImpeach", Value = 925)]
		C2S_SupportImpeach,
		[ProtoEnum(Name = "S2C_SupportImpeach", Value = 926)]
		S2C_SupportImpeach,
		[ProtoEnum(Name = "S2C_GuildDataUpdate", Value = 927)]
		S2C_GuildDataUpdate,
		[ProtoEnum(Name = "S2C_GuildMemberUpdate", Value = 928)]
		S2C_GuildMemberUpdate,
		[ProtoEnum(Name = "S2C_AddGuildMember", Value = 929)]
		S2C_AddGuildMember,
		[ProtoEnum(Name = "C2S_GuildSign", Value = 930)]
		C2S_GuildSign,
		[ProtoEnum(Name = "S2C_GuildSign", Value = 931)]
		S2C_GuildSign,
		[ProtoEnum(Name = "C2S_TakeScoreReward", Value = 932)]
		C2S_TakeScoreReward,
		[ProtoEnum(Name = "S2C_TakeScoreReward", Value = 933)]
		S2C_TakeScoreReward,
		[ProtoEnum(Name = "C2S_GetGuildBoss", Value = 934)]
		C2S_GetGuildBoss,
		[ProtoEnum(Name = "S2C_GetGuildBoss", Value = 935)]
		S2C_GetGuildBoss,
		[ProtoEnum(Name = "C2S_NoBossUpdate", Value = 936)]
		C2S_NoBossUpdate,
		[ProtoEnum(Name = "S2C_GuildBossUpdate", Value = 937)]
		S2C_GuildBossUpdate,
		[ProtoEnum(Name = "C2S_DoGuildBossDamage", Value = 938)]
		C2S_DoGuildBossDamage,
		[ProtoEnum(Name = "S2C_DoGuildBossDamage", Value = 939)]
		S2C_DoGuildBossDamage,
		[ProtoEnum(Name = "C2S_BuyGuildBossCount", Value = 940)]
		C2S_BuyGuildBossCount,
		[ProtoEnum(Name = "S2C_BuyGuildBossCount", Value = 941)]
		S2C_BuyGuildBossCount,
		[ProtoEnum(Name = "C2S_TakeGuildBossReward", Value = 942)]
		C2S_TakeGuildBossReward,
		[ProtoEnum(Name = "S2C_TakeGuildBossReward", Value = 943)]
		S2C_TakeGuildBossReward,
		[ProtoEnum(Name = "C2S_GetLootReward", Value = 944)]
		C2S_GetLootReward,
		[ProtoEnum(Name = "S2C_GetLootReward", Value = 945)]
		S2C_GetLootReward,
		[ProtoEnum(Name = "C2S_SetAttackTarget", Value = 946)]
		C2S_SetAttackTarget,
		[ProtoEnum(Name = "S2C_SetAttackTarget", Value = 947)]
		S2C_SetAttackTarget,
		[ProtoEnum(Name = "C2S_TakeGift", Value = 948)]
		C2S_TakeGift,
		[ProtoEnum(Name = "S2C_TakeGift", Value = 949)]
		S2C_TakeGift,
		[ProtoEnum(Name = "C2S_GiveGift", Value = 950)]
		C2S_GiveGift,
		[ProtoEnum(Name = "S2C_GiveGift", Value = 951)]
		S2C_GiveGift,
		[ProtoEnum(Name = "C2S_ChangeGuildName", Value = 952)]
		C2S_ChangeGuildName,
		[ProtoEnum(Name = "S2C_ChangeGuildName", Value = 953)]
		S2C_ChangeGuildName,
		[ProtoEnum(Name = "C2S_SetApplyCondition", Value = 954)]
		C2S_SetApplyCondition,
		[ProtoEnum(Name = "S2C_SetApplyCondition", Value = 955)]
		S2C_SetApplyCondition,
		[ProtoEnum(Name = "C2S_QueryGuildSignRecord", Value = 956)]
		C2S_QueryGuildSignRecord,
		[ProtoEnum(Name = "S2C_QueryGuildSignRecord", Value = 957)]
		S2C_QueryGuildSignRecord,
		[ProtoEnum(Name = "C2S_GuildRankData", Value = 958)]
		C2S_GuildRankData,
		[ProtoEnum(Name = "S2C_GuildRankData", Value = 959)]
		S2C_GuildRankData,
		[ProtoEnum(Name = "C2S_QueryGuildData", Value = 960)]
		C2S_QueryGuildData,
		[ProtoEnum(Name = "S2C_QueryGuildData", Value = 961)]
		S2C_QueryGuildData,
		[ProtoEnum(Name = "C2S_GuildBossStart", Value = 962)]
		C2S_GuildBossStart,
		[ProtoEnum(Name = "S2C_GuildBossStart", Value = 963)]
		S2C_GuildBossStart,
		[ProtoEnum(Name = "S2C_GuildBroadcastDamage", Value = 964)]
		S2C_GuildBroadcastDamage,
		[ProtoEnum(Name = "S2C_GuildBossDead", Value = 966)]
		S2C_GuildBossDead = 966,
		[ProtoEnum(Name = "C2S_TakeGuildBossDamageReward", Value = 967)]
		C2S_TakeGuildBossDamageReward,
		[ProtoEnum(Name = "S2C_TakeGuildBossDamageReward", Value = 968)]
		S2C_TakeGuildBossDamageReward,
		[ProtoEnum(Name = "C2S_QueryGuildBossDamageRank", Value = 969)]
		C2S_QueryGuildBossDamageRank,
		[ProtoEnum(Name = "S2C_QueryGuildBossDamageRank", Value = 970)]
		S2C_QueryGuildBossDamageRank,
		[ProtoEnum(Name = "C2S_GuildWarQueryInfo", Value = 975)]
		C2S_GuildWarQueryInfo = 975,
		[ProtoEnum(Name = "S2C_GuildWarQueryInfo", Value = 976)]
		S2C_GuildWarQueryInfo,
		[ProtoEnum(Name = "C2S_GuildWarRank", Value = 977)]
		C2S_GuildWarRank,
		[ProtoEnum(Name = "S2C_GuildWarRank", Value = 978)]
		S2C_GuildWarRank,
		[ProtoEnum(Name = "C2S_GuildWarEnter", Value = 979)]
		C2S_GuildWarEnter,
		[ProtoEnum(Name = "S2C_GuildWarEnter", Value = 980)]
		S2C_GuildWarEnter,
		[ProtoEnum(Name = "C2S_GuildWarQueryStrongholdInfo", Value = 981)]
		C2S_GuildWarQueryStrongholdInfo,
		[ProtoEnum(Name = "S2C_GuildWarQueryStrongholdInfo", Value = 982)]
		S2C_GuildWarQueryStrongholdInfo,
		[ProtoEnum(Name = "C2S_GuildWarFightBegin", Value = 983)]
		C2S_GuildWarFightBegin,
		[ProtoEnum(Name = "S2C_GuildWarFightBegin", Value = 984)]
		S2C_GuildWarFightBegin,
		[ProtoEnum(Name = "C2S_GuildWarCombatStart", Value = 985)]
		C2S_GuildWarCombatStart,
		[ProtoEnum(Name = "C2S_GuildWarSupport", Value = 986)]
		C2S_GuildWarSupport,
		[ProtoEnum(Name = "C2S_GuildWarFightEnd", Value = 987)]
		C2S_GuildWarFightEnd,
		[ProtoEnum(Name = "S2C_GuildWarFightEnd", Value = 988)]
		S2C_GuildWarFightEnd,
		[ProtoEnum(Name = "C2S_GuildWarHold", Value = 989)]
		C2S_GuildWarHold,
		[ProtoEnum(Name = "S2C_GuildWarHold", Value = 990)]
		S2C_GuildWarHold,
		[ProtoEnum(Name = "C2S_GuildWarTakeReward", Value = 991)]
		C2S_GuildWarTakeReward,
		[ProtoEnum(Name = "S2C_GuildWarTakeReward", Value = 992)]
		S2C_GuildWarTakeReward,
		[ProtoEnum(Name = "C2S_GuildWarUpdate", Value = 993)]
		C2S_GuildWarUpdate,
		[ProtoEnum(Name = "S2C_GuildWarUpdate", Value = 994)]
		S2C_GuildWarUpdate,
		[ProtoEnum(Name = "S2C_GuildWarSupport", Value = 995)]
		S2C_GuildWarSupport,
		[ProtoEnum(Name = "S2C_GuildWarPrompt", Value = 996)]
		S2C_GuildWarPrompt,
		[ProtoEnum(Name = "C2S_GuildWarKillRank", Value = 997)]
		C2S_GuildWarKillRank,
		[ProtoEnum(Name = "S2C_GuildWarKillRank", Value = 998)]
		S2C_GuildWarKillRank,
		[ProtoEnum(Name = "C2S_GuildWarReqRevive", Value = 999)]
		C2S_GuildWarReqRevive,
		[ProtoEnum(Name = "S2C_GuildWarReqRevive", Value = 1000)]
		S2C_GuildWarReqRevive,
		[ProtoEnum(Name = "C2S_GuildWarQueryCombatRecord", Value = 1001)]
		C2S_GuildWarQueryCombatRecord,
		[ProtoEnum(Name = "S2C_GuildWarQueryCombatRecord", Value = 1002)]
		S2C_GuildWarQueryCombatRecord,
		[ProtoEnum(Name = "C2S_GuildWarQuitHold", Value = 1003)]
		C2S_GuildWarQuitHold,
		[ProtoEnum(Name = "S2C_GuildWarQuitHold", Value = 1004)]
		S2C_GuildWarQuitHold,
		[ProtoEnum(Name = "C2S_GuildWarRecoverHP", Value = 1005)]
		C2S_GuildWarRecoverHP,
		[ProtoEnum(Name = "S2C_GuildWarRecoverHP", Value = 1006)]
		S2C_GuildWarRecoverHP,
		[ProtoEnum(Name = "S2C_GuildWarPlayerUpdate", Value = 1008)]
		S2C_GuildWarPlayerUpdate = 1008,
		[ProtoEnum(Name = "S2C_GuildWarStrongholdUpdate", Value = 1010)]
		S2C_GuildWarStrongholdUpdate = 1010,
		[ProtoEnum(Name = "S2C_GuildWarRewardUpdate", Value = 1012)]
		S2C_GuildWarRewardUpdate = 1012,
		[ProtoEnum(Name = "S2C_GuildWarEnd", Value = 1014)]
		S2C_GuildWarEnd = 1014,
		[ProtoEnum(Name = "C2S_GuildWarKickHold", Value = 1015)]
		C2S_GuildWarKickHold,
		[ProtoEnum(Name = "S2C_GuildWarKickHold", Value = 1016)]
		S2C_GuildWarKickHold,
		[ProtoEnum(Name = "C2S_GuildWarGetSupportInfo", Value = 1017)]
		C2S_GuildWarGetSupportInfo,
		[ProtoEnum(Name = "S2C_GuildWarGetSupportInfo", Value = 1018)]
		S2C_GuildWarGetSupportInfo,
		[ProtoEnum(Name = "C2S_QueryOrePillageData", Value = 1020)]
		C2S_QueryOrePillageData = 1020,
		[ProtoEnum(Name = "S2C_QueryOrePillageData", Value = 1021)]
		S2C_QueryOrePillageData,
		[ProtoEnum(Name = "C2S_QueryMyOreData", Value = 1022)]
		C2S_QueryMyOreData,
		[ProtoEnum(Name = "S2C_QueryMyOreData", Value = 1023)]
		S2C_QueryMyOreData,
		[ProtoEnum(Name = "C2S_TakeOreReward", Value = 1024)]
		C2S_TakeOreReward,
		[ProtoEnum(Name = "S2C_TakeOreReward", Value = 1025)]
		S2C_TakeOreReward,
		[ProtoEnum(Name = "C2S_OrePillageStart", Value = 1026)]
		C2S_OrePillageStart,
		[ProtoEnum(Name = "S2C_OrePillageStart", Value = 1027)]
		S2C_OrePillageStart,
		[ProtoEnum(Name = "C2S_OrePillageResult", Value = 1028)]
		C2S_OrePillageResult,
		[ProtoEnum(Name = "S2C_OrePillageResult", Value = 1029)]
		S2C_OrePillageResult,
		[ProtoEnum(Name = "C2S_BuyOreRevengeCount", Value = 1030)]
		C2S_BuyOreRevengeCount,
		[ProtoEnum(Name = "S2C_BuyOreRevengeCount", Value = 1031)]
		S2C_BuyOreRevengeCount,
		[ProtoEnum(Name = "C2S_BuyOrePillageCount", Value = 1032)]
		C2S_BuyOrePillageCount,
		[ProtoEnum(Name = "S2C_BuyOrePillageCount", Value = 1033)]
		S2C_BuyOrePillageCount,
		[ProtoEnum(Name = "S2C_UpdateOrePillageData", Value = 1034)]
		S2C_UpdateOrePillageData,
		[ProtoEnum(Name = "C2S_GetOreRankList", Value = 1035)]
		C2S_GetOreRankList,
		[ProtoEnum(Name = "S2C_GetOreRankList", Value = 1036)]
		S2C_GetOreRankList,
		[ProtoEnum(Name = "C2S_GetGGOreRankList", Value = 1037)]
		C2S_GetGGOreRankList,
		[ProtoEnum(Name = "S2C_GetGGOreRankList", Value = 1038)]
		S2C_GetGGOreRankList,
		[ProtoEnum(Name = "C2S_GetGOreRankList", Value = 1039)]
		C2S_GetGOreRankList,
		[ProtoEnum(Name = "S2C_GetGOreRankList", Value = 1040)]
		S2C_GetGOreRankList,
		[ProtoEnum(Name = "C2S_OrepillageFarm", Value = 1041)]
		C2S_OrepillageFarm,
		[ProtoEnum(Name = "S2C_OrepillageFarm", Value = 1042)]
		S2C_OrepillageFarm,
		[ProtoEnum(Name = "C2S_LopetSetCombat", Value = 1060)]
		C2S_LopetSetCombat = 1060,
		[ProtoEnum(Name = "S2C_LopetSetCombat", Value = 1061)]
		S2C_LopetSetCombat,
		[ProtoEnum(Name = "C2S_LopetLevelup", Value = 1062)]
		C2S_LopetLevelup,
		[ProtoEnum(Name = "S2C_LopetLevelup", Value = 1063)]
		S2C_LopetLevelup,
		[ProtoEnum(Name = "C2S_LopetAwake", Value = 1064)]
		C2S_LopetAwake,
		[ProtoEnum(Name = "S2C_LopetAwake", Value = 1065)]
		S2C_LopetAwake,
		[ProtoEnum(Name = "C2S_LopetBreakUp", Value = 1066)]
		C2S_LopetBreakUp,
		[ProtoEnum(Name = "S2C_LopetBreakUp", Value = 1067)]
		S2C_LopetBreakUp,
		[ProtoEnum(Name = "C2S_LopetReborn", Value = 1068)]
		C2S_LopetReborn,
		[ProtoEnum(Name = "S2C_LopetReborn", Value = 1069)]
		S2C_LopetReborn,
		[ProtoEnum(Name = "S2C_LopetAdd", Value = 1071)]
		S2C_LopetAdd = 1071,
		[ProtoEnum(Name = "S2C_LopetUpdate", Value = 1073)]
		S2C_LopetUpdate = 1073,
		[ProtoEnum(Name = "S2C_LopetRemove", Value = 1075)]
		S2C_LopetRemove = 1075,
		[ProtoEnum(Name = "C2S_LopetSummon", Value = 1076)]
		C2S_LopetSummon,
		[ProtoEnum(Name = "S2C_LopetSummon", Value = 1077)]
		S2C_LopetSummon,
		[ProtoEnum(Name = "S2C_UpdateMagicLoveData", Value = 1100)]
		S2C_UpdateMagicLoveData = 1100,
		[ProtoEnum(Name = "C2S_GetMagicLoveData", Value = 1101)]
		C2S_GetMagicLoveData,
		[ProtoEnum(Name = "S2C_GetMagicLoveData", Value = 1102)]
		S2C_GetMagicLoveData,
		[ProtoEnum(Name = "C2S_MagicMatch", Value = 1103)]
		C2S_MagicMatch,
		[ProtoEnum(Name = "S2C_MagicMatch", Value = 1104)]
		S2C_MagicMatch,
		[ProtoEnum(Name = "C2S_OneKeyMagicMatch", Value = 1105)]
		C2S_OneKeyMagicMatch,
		[ProtoEnum(Name = "S2C_OneKeyMagicMatch", Value = 1106)]
		S2C_OneKeyMagicMatch,
		[ProtoEnum(Name = "C2S_MagicCamouflage", Value = 1107)]
		C2S_MagicCamouflage,
		[ProtoEnum(Name = "S2C_MagicCamouflage", Value = 1108)]
		S2C_MagicCamouflage,
		[ProtoEnum(Name = "C2S_Go", Value = 1109)]
		C2S_Go,
		[ProtoEnum(Name = "S2C_Go", Value = 1110)]
		S2C_Go,
		[ProtoEnum(Name = "C2S_TakeMagicLoveReward", Value = 1111)]
		C2S_TakeMagicLoveReward,
		[ProtoEnum(Name = "S2C_TakeMagicLoveReward", Value = 1112)]
		S2C_TakeMagicLoveReward,
		[ProtoEnum(Name = "C2S_BuyMagicMatchCount", Value = 1113)]
		C2S_BuyMagicMatchCount,
		[ProtoEnum(Name = "S2C_BuyMagicMatchCount", Value = 1114)]
		S2C_BuyMagicMatchCount,
		[ProtoEnum(Name = "C2S_RefreshPet", Value = 1115)]
		C2S_RefreshPet,
		[ProtoEnum(Name = "S2C_RefreshPet", Value = 1116)]
		S2C_RefreshPet,
		[ProtoEnum(Name = "C2S_QueryRoseRank", Value = 1494)]
		C2S_QueryRoseRank = 1494,
		[ProtoEnum(Name = "S2C_QueryRoseRank", Value = 1495)]
		S2C_QueryRoseRank,
		[ProtoEnum(Name = "C2S_QueryTortoiseRank", Value = 1496)]
		C2S_QueryTortoiseRank,
		[ProtoEnum(Name = "S2C_QueryTortoiseRank", Value = 1497)]
		S2C_QueryTortoiseRank,
		[ProtoEnum(Name = "C2S_GMCommand", Value = 1498)]
		C2S_GMCommand,
		[ProtoEnum(Name = "S2C_GMCommand", Value = 1499)]
		S2C_GMCommand,
		[ProtoEnum(Name = "MaxClientID", Value = 1500)]
		MaxClientID,
		[ProtoEnum(Name = "C2C_Connect", Value = 1501)]
		C2C_Connect,
		[ProtoEnum(Name = "C2C_Disconnect", Value = 1502)]
		C2C_Disconnect,
		[ProtoEnum(Name = "C2C_HttpData", Value = 1503)]
		C2C_HttpData,
		[ProtoEnum(Name = "C2C_HttpRegister", Value = 1504)]
		C2C_HttpRegister,
		[ProtoEnum(Name = "C2C_HttpLogin", Value = 1505)]
		C2C_HttpLogin,
		[ProtoEnum(Name = "C2C_HttpServerList", Value = 1506)]
		C2C_HttpServerList,
		[ProtoEnum(Name = "C2C_HttpBillboard", Value = 1507)]
		C2C_HttpBillboard,
		[ProtoEnum(Name = "C2C_HttpElfInit", Value = 1508)]
		C2C_HttpElfInit,
		[ProtoEnum(Name = "C2C_HttpElfContent", Value = 1509)]
		C2C_HttpElfContent,
		[ProtoEnum(Name = "C2C_HttpElfQuery", Value = 1510)]
		C2C_HttpElfQuery,
		[ProtoEnum(Name = "C2C_HttpElfComment", Value = 1511)]
		C2C_HttpElfComment,
		[ProtoEnum(Name = "C2C_HttpElfHot", Value = 1512)]
		C2C_HttpElfHot,
		[ProtoEnum(Name = "C2C_HttpUploadVoice", Value = 1513)]
		C2C_HttpUploadVoice,
		[ProtoEnum(Name = "C2C_HttpGetVoice", Value = 1514)]
		C2C_HttpGetVoice,
		[ProtoEnum(Name = "C2C_HttpTranslate", Value = 1515)]
		C2C_HttpTranslate
	}
}
