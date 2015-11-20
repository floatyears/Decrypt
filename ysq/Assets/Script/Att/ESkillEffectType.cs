using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ESkillEffectType")]
	public enum ESkillEffectType
	{
		[ProtoEnum(Name = "ESET_Null", Value = 0)]
		ESET_Null,
		[ProtoEnum(Name = "ESET_TriggerSkill", Value = 1)]
		ESET_TriggerSkill,
		[ProtoEnum(Name = "ESET_TriggerBuff", Value = 2)]
		ESET_TriggerBuff,
		[ProtoEnum(Name = "ESET_PhysicDamage", Value = 4)]
		ESET_PhysicDamage = 4,
		[ProtoEnum(Name = "ESET_MagicDamage", Value = 5)]
		ESET_MagicDamage,
		[ProtoEnum(Name = "ESET_Heal", Value = 6)]
		ESET_Heal,
		[ProtoEnum(Name = "ESET_Summon", Value = 7)]
		ESET_Summon,
		[ProtoEnum(Name = "ESET_SummonMaelstrom", Value = 8)]
		ESET_SummonMaelstrom,
		[ProtoEnum(Name = "ESET_SummonMonster", Value = 9)]
		ESET_SummonMonster,
		[ProtoEnum(Name = "ESET_MutilDamage", Value = 10)]
		ESET_MutilDamage,
		[ProtoEnum(Name = "ESET_ScaleDamage", Value = 11)]
		ESET_ScaleDamage,
		[ProtoEnum(Name = "ESET_LopetDamage", Value = 12)]
		ESET_LopetDamage
	}
}
