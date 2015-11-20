using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EBuffEffectType")]
	public enum EBuffEffectType
	{
		[ProtoEnum(Name = "EBET_Null", Value = 0)]
		EBET_Null,
		[ProtoEnum(Name = "EBET_ModAttValue", Value = 1)]
		EBET_ModAttValue,
		[ProtoEnum(Name = "EBET_ModAttPct", Value = 2)]
		EBET_ModAttPct,
		[ProtoEnum(Name = "EBET_PeriodicDamage", Value = 3)]
		EBET_PeriodicDamage,
		[ProtoEnum(Name = "EBET_PeriodicHeal", Value = 4)]
		EBET_PeriodicHeal,
		[ProtoEnum(Name = "EBET_Root", Value = 5)]
		EBET_Root,
		[ProtoEnum(Name = "EBET_Silence", Value = 6)]
		EBET_Silence,
		[ProtoEnum(Name = "EBET_Stun", Value = 7)]
		EBET_Stun,
		[ProtoEnum(Name = "EBET_DecreaseSpeed", Value = 8)]
		EBET_DecreaseSpeed,
		[ProtoEnum(Name = "EBET_IncreaseSpeed", Value = 9)]
		EBET_IncreaseSpeed,
		[ProtoEnum(Name = "EBET_Immunity", Value = 10)]
		EBET_Immunity,
		[ProtoEnum(Name = "EBET_AbsorbDamage", Value = 11)]
		EBET_AbsorbDamage,
		[ProtoEnum(Name = "EBET_Reflect", Value = 12)]
		EBET_Reflect,
		[ProtoEnum(Name = "EBET_ModDamage", Value = 13)]
		EBET_ModDamage,
		[ProtoEnum(Name = "EBET_ModDamageTaken", Value = 14)]
		EBET_ModDamageTaken,
		[ProtoEnum(Name = "EBET_ModHeal", Value = 15)]
		EBET_ModHeal,
		[ProtoEnum(Name = "EBET_ModHealTaken", Value = 16)]
		EBET_ModHealTaken,
		[ProtoEnum(Name = "EBET_Fear", Value = 17)]
		EBET_Fear,
		[ProtoEnum(Name = "EBET_ModAttackSpeed", Value = 18)]
		EBET_ModAttackSpeed,
		[ProtoEnum(Name = "EBET_HitBack", Value = 19)]
		EBET_HitBack,
		[ProtoEnum(Name = "EBET_HitDown", Value = 20)]
		EBET_HitDown,
		[ProtoEnum(Name = "EBET_ModResis", Value = 21)]
		EBET_ModResis,
		[ProtoEnum(Name = "EBET_Resurrect", Value = 22)]
		EBET_Resurrect,
		[ProtoEnum(Name = "EBET_ChangeFaction", Value = 23)]
		EBET_ChangeFaction,
		[ProtoEnum(Name = "EBET_DamageToHeal", Value = 24)]
		EBET_DamageToHeal,
		[ProtoEnum(Name = "EBET_PeriodicHeal2", Value = 25)]
		EBET_PeriodicHeal2,
		[ProtoEnum(Name = "EBET_ModAutoAttack", Value = 26)]
		EBET_ModAutoAttack,
		[ProtoEnum(Name = "EBET_ModHitEffect", Value = 27)]
		EBET_ModHitEffect,
		[ProtoEnum(Name = "EBET_HPTrigger", Value = 28)]
		EBET_HPTrigger,
		[ProtoEnum(Name = "EBET_TakeDamageToHeal", Value = 29)]
		EBET_TakeDamageToHeal,
		[ProtoEnum(Name = "EBET_ChangeModel", Value = 30)]
		EBET_ChangeModel,
		[ProtoEnum(Name = "EBET_ShareDamage", Value = 31)]
		EBET_ShareDamage
	}
}
