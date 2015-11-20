using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ELegendEffectType")]
	public enum ELegendEffectType
	{
		[ProtoEnum(Name = "ELET_Null", Value = 0)]
		ELET_Null,
		[ProtoEnum(Name = "ELET_ModAttValue", Value = 1)]
		ELET_ModAttValue,
		[ProtoEnum(Name = "ELET_ModAttPct", Value = 2)]
		ELET_ModAttPct,
		[ProtoEnum(Name = "ELET_DoubleDamage", Value = 3)]
		ELET_DoubleDamage,
		[ProtoEnum(Name = "ELET_ReflexDamage", Value = 4)]
		ELET_ReflexDamage,
		[ProtoEnum(Name = "ELET_ReduceDamage", Value = 5)]
		ELET_ReduceDamage,
		[ProtoEnum(Name = "ELET_DamageToHeal", Value = 6)]
		ELET_DamageToHeal,
		[ProtoEnum(Name = "ELET_IgnoreDefense", Value = 7)]
		ELET_IgnoreDefense,
		[ProtoEnum(Name = "ELET_AttackToHeal", Value = 8)]
		ELET_AttackToHeal
	}
}
