using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EInteractionType")]
	public enum EInteractionType
	{
		[ProtoEnum(Name = "EInteraction_Rose", Value = 1)]
		EInteraction_Rose = 1,
		[ProtoEnum(Name = "EInteraction_Dance", Value = 2)]
		EInteraction_Dance,
		[ProtoEnum(Name = "EInteraction_Wand", Value = 3)]
		EInteraction_Wand,
		[ProtoEnum(Name = "EInteraction_Turtle", Value = 4)]
		EInteraction_Turtle
	}
}
