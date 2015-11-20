using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EContentType")]
	public enum EContentType
	{
		[ProtoEnum(Name = "EContent_Text", Value = 0)]
		EContent_Text,
		[ProtoEnum(Name = "EContent_AffixToText", Value = 1)]
		EContent_AffixToText
	}
}
