using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EAttID")]
	public enum EAttID
	{
		[ProtoEnum(Name = "EAID_Null", Value = 0)]
		EAID_Null,
		[ProtoEnum(Name = "EAID_MaxHP", Value = 1)]
		EAID_MaxHP,
		[ProtoEnum(Name = "EAID_Attack", Value = 2)]
		EAID_Attack,
		[ProtoEnum(Name = "EAID_PhysicDefense", Value = 3)]
		EAID_PhysicDefense,
		[ProtoEnum(Name = "EAID_MagicDefense", Value = 4)]
		EAID_MagicDefense,
		[ProtoEnum(Name = "EAID_Hit", Value = 5)]
		EAID_Hit,
		[ProtoEnum(Name = "EAID_Dodge", Value = 6)]
		EAID_Dodge,
		[ProtoEnum(Name = "EAID_Crit", Value = 7)]
		EAID_Crit,
		[ProtoEnum(Name = "EAID_CritResis", Value = 8)]
		EAID_CritResis,
		[ProtoEnum(Name = "EAID_DamagePlus", Value = 9)]
		EAID_DamagePlus,
		[ProtoEnum(Name = "EAID_DamageMinus", Value = 10)]
		EAID_DamageMinus,
		[ProtoEnum(Name = "EAID_Separator", Value = 11)]
		EAID_Separator,
		[ProtoEnum(Name = "EAID_Defense", Value = 20)]
		EAID_Defense = 20,
		[ProtoEnum(Name = "EAID_ResisBegin", Value = 300)]
		EAID_ResisBegin = 300,
		[ProtoEnum(Name = "EAID_StunResis", Value = 301)]
		EAID_StunResis,
		[ProtoEnum(Name = "EAID_RootResis", Value = 302)]
		EAID_RootResis,
		[ProtoEnum(Name = "EAID_FearResis", Value = 303)]
		EAID_FearResis,
		[ProtoEnum(Name = "EAID_HitBackResis", Value = 304)]
		EAID_HitBackResis,
		[ProtoEnum(Name = "EAID_HitDownResis", Value = 305)]
		EAID_HitDownResis,
		[ProtoEnum(Name = "EAID_SilenceResis", Value = 306)]
		EAID_SilenceResis
	}
}
