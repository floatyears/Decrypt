using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EItemQualityType")]
	public enum EItemQualityType
	{
		[ProtoEnum(Name = "EIQT_Green", Value = 0)]
		EIQT_Green,
		[ProtoEnum(Name = "EIQT_Blue", Value = 1)]
		EIQT_Blue,
		[ProtoEnum(Name = "EIQT_Purple", Value = 2)]
		EIQT_Purple,
		[ProtoEnum(Name = "EIQT_Orange", Value = 3)]
		EIQT_Orange,
		[ProtoEnum(Name = "EIQT_Gold", Value = 4)]
		EIQT_Gold,
		[ProtoEnum(Name = "EIQT_Max", Value = 5)]
		EIQT_Max
	}
}
