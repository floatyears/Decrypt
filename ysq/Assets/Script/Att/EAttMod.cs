using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EAttMod")]
	public enum EAttMod
	{
		[ProtoEnum(Name = "EAM_Value", Value = 0)]
		EAM_Value,
		[ProtoEnum(Name = "EAM_Pct", Value = 1)]
		EAM_Pct
	}
}
