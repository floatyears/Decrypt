using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EItemSource")]
	public enum EItemSource
	{
		[ProtoEnum(Name = "EISource_HonorShop", Value = 1)]
		EISource_HonorShop = 1,
		[ProtoEnum(Name = "EISource_GuildShop", Value = 2)]
		EISource_GuildShop,
		[ProtoEnum(Name = "EISource_LuckyRoll", Value = 4)]
		EISource_LuckyRoll = 4,
		[ProtoEnum(Name = "EISource_KingReward", Value = 8)]
		EISource_KingReward = 8,
		[ProtoEnum(Name = "EISource_PetShop", Value = 16)]
		EISource_PetShop = 16,
		[ProtoEnum(Name = "EISource_Common", Value = 32)]
		EISource_Common = 32,
		[ProtoEnum(Name = "EISource_KRShop", Value = 64)]
		EISource_KRShop = 64,
		[ProtoEnum(Name = "EISource_TrialShop", Value = 128)]
		EISource_TrialShop = 128,
		[ProtoEnum(Name = "EISource_Trial", Value = 256)]
		EISource_Trial = 256,
		[ProtoEnum(Name = "EISource_WorldBoss", Value = 512)]
		EISource_WorldBoss = 512,
		[ProtoEnum(Name = "EISource_CostumeParty", Value = 1024)]
		EISource_CostumeParty = 1024,
		[ProtoEnum(Name = "EISource_SoulReliquary", Value = 2048)]
		EISource_SoulReliquary = 2048,
		[ProtoEnum(Name = "EISource_SceneLoot", Value = 4096)]
		EISource_SceneLoot = 4096,
		[ProtoEnum(Name = "EISource_Pillage", Value = 8192)]
		EISource_Pillage = 8192,
		[ProtoEnum(Name = "EISource_AwakeShop", Value = 16384)]
		EISource_AwakeShop = 16384,
		[ProtoEnum(Name = "EISource_AllSceneLoot", Value = 32768)]
		EISource_AllSceneLoot = 32768,
		[ProtoEnum(Name = "EISource_AllAwakeSceneLoot", Value = 65536)]
		EISource_AllAwakeSceneLoot = 65536,
		[ProtoEnum(Name = "EISource_LopetShop", Value = 131072)]
		EISource_LopetShop = 131072,
		[ProtoEnum(Name = "EISource_GuildWarMVP", Value = 262144)]
		EISource_GuildWarMVP = 262144
	}
}
