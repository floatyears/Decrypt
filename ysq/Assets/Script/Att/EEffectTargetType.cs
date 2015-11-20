using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EEffectTargetType")]
	public enum EEffectTargetType
	{
		[ProtoEnum(Name = "EETT_Target", Value = 0)]
		EETT_Target,
		[ProtoEnum(Name = "EETT_Caster", Value = 1)]
		EETT_Caster,
		[ProtoEnum(Name = "EETT_AllEnemyAroundCaster", Value = 2)]
		EETT_AllEnemyAroundCaster,
		[ProtoEnum(Name = "EETT_AllFriendAroundCaster", Value = 3)]
		EETT_AllFriendAroundCaster,
		[ProtoEnum(Name = "EETT_AllEnemyInfrontCaster", Value = 4)]
		EETT_AllEnemyInfrontCaster,
		[ProtoEnum(Name = "EETT_AllEnemyInfrontCaster2", Value = 5)]
		EETT_AllEnemyInfrontCaster2,
		[ProtoEnum(Name = "EETT_AllEnemyInArea1", Value = 6)]
		EETT_AllEnemyInArea1,
		[ProtoEnum(Name = "EETT_AllEnemyInArea2", Value = 7)]
		EETT_AllEnemyInArea2,
		[ProtoEnum(Name = "EETT_AllFriendInArea2", Value = 8)]
		EETT_AllFriendInArea2
	}
}
