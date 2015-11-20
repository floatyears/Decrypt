using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ECastType")]
	public enum ECastType
	{
		[ProtoEnum(Name = "ECast_Active", Value = 0)]
		ECast_Active,
		[ProtoEnum(Name = "ECast_Trigger2", Value = 1)]
		ECast_Trigger2,
		[ProtoEnum(Name = "ECast_Passive", Value = 2)]
		ECast_Passive,
		[ProtoEnum(Name = "ECast_Trigger1", Value = 3)]
		ECast_Trigger1
	}
}
