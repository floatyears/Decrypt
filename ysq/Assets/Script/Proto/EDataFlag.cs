using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EDataFlag")]
	public enum EDataFlag
	{
		[ProtoEnum(Name = "EDFlag_DayEnergy1", Value = 1)]
		EDFlag_DayEnergy1 = 1,
		[ProtoEnum(Name = "EDFlag_DayEnergy2", Value = 2)]
		EDFlag_DayEnergy2,
		[ProtoEnum(Name = "EDFlag_FirstPayReward", Value = 4)]
		EDFlag_FirstPayReward = 4,
		[ProtoEnum(Name = "EDFlag_BindPhoneReward", Value = 8)]
		EDFlag_BindPhoneReward = 8,
		[ProtoEnum(Name = "EDFlag_GuildBossReward", Value = 16)]
		EDFlag_GuildBossReward = 16,
		[ProtoEnum(Name = "EDFlag_FirstScene", Value = 32)]
		EDFlag_FirstScene = 32,
		[ProtoEnum(Name = "EDFlag_FirstArena", Value = 64)]
		EDFlag_FirstArena = 64,
		[ProtoEnum(Name = "EDFlag_EnableDart", Value = 128)]
		EDFlag_EnableDart = 128,
		[ProtoEnum(Name = "EDFlag_EnableLuckyDraw", Value = 256)]
		EDFlag_EnableLuckyDraw = 256,
		[ProtoEnum(Name = "EDFlag_EnableScratchOff", Value = 512)]
		EDFlag_EnableScratchOff = 512,
		[ProtoEnum(Name = "EDFlag_EnableFlashSale", Value = 1024)]
		EDFlag_EnableFlashSale = 1024,
		[ProtoEnum(Name = "EDFlag_DayEnergy3", Value = 2048)]
		EDFlag_DayEnergy3 = 2048,
		[ProtoEnum(Name = "EDFlag_FirstLuckyRoll", Value = 4096)]
		EDFlag_FirstLuckyRoll = 4096,
		[ProtoEnum(Name = "EDFlag_HotTime", Value = 8192)]
		EDFlag_HotTime = 8192,
		[ProtoEnum(Name = "EDFlag_LevelRank", Value = 16384)]
		EDFlag_LevelRank = 16384,
		[ProtoEnum(Name = "EDFlag_VipLevel", Value = 32768)]
		EDFlag_VipLevel = 32768,
		[ProtoEnum(Name = "EDFlag_ActivityPayPopup", Value = 65536)]
		EDFlag_ActivityPayPopup = 65536,
		[ProtoEnum(Name = "EDFlag_GuildRank", Value = 131072)]
		EDFlag_GuildRank = 131072,
		[ProtoEnum(Name = "EDFlag_IReward1", Value = 262144)]
		EDFlag_IReward1 = 262144,
		[ProtoEnum(Name = "EDFlag_PayCheck", Value = 524288)]
		EDFlag_PayCheck = 524288,
		[ProtoEnum(Name = "EDFlag_IReward3", Value = 1048576)]
		EDFlag_IReward3 = 1048576,
		[ProtoEnum(Name = "EDFlag_DayEnergy4", Value = 2097152)]
		EDFlag_DayEnergy4 = 2097152,
		[ProtoEnum(Name = "EDFlag_Payed", Value = 4194304)]
		EDFlag_Payed = 4194304,
		[ProtoEnum(Name = "EDFlag_IReward2", Value = 8388608)]
		EDFlag_IReward2 = 8388608,
		[ProtoEnum(Name = "EDFlag_TakeHotTimeReward", Value = 33554432)]
		EDFlag_TakeHotTimeReward = 33554432,
		[ProtoEnum(Name = "EDFlag_GuildSign1", Value = 67108864)]
		EDFlag_GuildSign1 = 67108864,
		[ProtoEnum(Name = "EDFlag_GuildSign2", Value = 134217728)]
		EDFlag_GuildSign2 = 134217728,
		[ProtoEnum(Name = "EDFlag_GuildSign3", Value = 268435456)]
		EDFlag_GuildSign3 = 268435456
	}
}
