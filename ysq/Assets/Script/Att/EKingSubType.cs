using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EKingSubType")]
	public enum EKingSubType
	{
		[ProtoEnum(Name = "EKingSub_1", Value = 0)]
		EKingSub_1,
		[ProtoEnum(Name = "EKingSub_2", Value = 1)]
		EKingSub_2,
		[ProtoEnum(Name = "EKingSub_3", Value = 2)]
		EKingSub_3,
		[ProtoEnum(Name = "EKingSub_4", Value = 3)]
		EKingSub_4
	}
}
