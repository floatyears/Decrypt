using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "EQuestIconType")]
	public enum EQuestIconType
	{
		[ProtoEnum(Name = "EQIcon_Boss", Value = 0)]
		EQIcon_Boss,
		[ProtoEnum(Name = "EQIcon_Pet", Value = 1)]
		EQIcon_Pet,
		[ProtoEnum(Name = "EQIcon_Item", Value = 2)]
		EQIcon_Item
	}
}
