using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "ERedFlag")]
	public enum ERedFlag
	{
		[ProtoEnum(Name = "ERedFlag_Guild", Value = 1)]
		ERedFlag_Guild = 1,
		[ProtoEnum(Name = "ERedFlag_GuildBoss", Value = 2)]
		ERedFlag_GuildBoss,
		[ProtoEnum(Name = "ERedFlag_Dart", Value = 4)]
		ERedFlag_Dart = 4,
		[ProtoEnum(Name = "ERedFlag_LuckyDraw", Value = 8)]
		ERedFlag_LuckyDraw = 8,
		[ProtoEnum(Name = "ERedFlag_FlashSale", Value = 16)]
		ERedFlag_FlashSale = 16,
		[ProtoEnum(Name = "ERedFlag_ScratchOff", Value = 32)]
		ERedFlag_ScratchOff = 32,
		[ProtoEnum(Name = "ERedFlag_WorldBoss", Value = 64)]
		ERedFlag_WorldBoss = 64,
		[ProtoEnum(Name = "ERedFlag_HotTime", Value = 128)]
		ERedFlag_HotTime = 128,
		[ProtoEnum(Name = "ERedFlag_ArenaRecord", Value = 256)]
		ERedFlag_ArenaRecord = 256,
		[ProtoEnum(Name = "ERedFlag_PillageRecord", Value = 512)]
		ERedFlag_PillageRecord = 512,
		[ProtoEnum(Name = "ERedFlag_ShopCommon2", Value = 1024)]
		ERedFlag_ShopCommon2 = 1024,
		[ProtoEnum(Name = "ERedFlag_ShopAwaken", Value = 2048)]
		ERedFlag_ShopAwaken = 2048,
		[ProtoEnum(Name = "ERedFlag_KR", Value = 4096)]
		ERedFlag_KR = 4096,
		[ProtoEnum(Name = "ERedFlag_GuildWar", Value = 8192)]
		ERedFlag_GuildWar = 8192,
		[ProtoEnum(Name = "ERedFlag_OrePillageRecord", Value = 16384)]
		ERedFlag_OrePillageRecord = 16384,
		[ProtoEnum(Name = "ERedFlag_OrePillage", Value = 32768)]
		ERedFlag_OrePillage = 32768,
		[ProtoEnum(Name = "ERedFlag_GuildApply", Value = 65536)]
		ERedFlag_GuildApply = 65536,
		[ProtoEnum(Name = "ERedFlag_ShopLopet", Value = 131072)]
		ERedFlag_ShopLopet = 131072,
		[ProtoEnum(Name = "ERedFlag_MagicLove", Value = 262144)]
		ERedFlag_MagicLove = 262144
	}
}
