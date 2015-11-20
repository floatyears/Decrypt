using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EShopItemOpenType")]
	public enum EShopItemOpenType
	{
		[ProtoEnum(Name = "ESIOT_Guild", Value = 1)]
		ESIOT_Guild = 1,
		[ProtoEnum(Name = "ESIOT_TrialWave", Value = 2)]
		ESIOT_TrialWave,
		[ProtoEnum(Name = "ESIOT_ArenaHighestRank", Value = 3)]
		ESIOT_ArenaHighestRank,
		[ProtoEnum(Name = "ESIOT_Vip", Value = 4)]
		ESIOT_Vip
	}
}
