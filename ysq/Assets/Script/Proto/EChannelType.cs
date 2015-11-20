using ProtoBuf;
using System;

namespace Proto
{
	[ProtoContract(Name = "EChannelType")]
	public enum EChannelType
	{
		[ProtoEnum(Name = "EChannel_World", Value = 0)]
		EChannel_World,
		[ProtoEnum(Name = "EChannel_Guild", Value = 1)]
		EChannel_Guild,
		[ProtoEnum(Name = "EChannel_Whisper", Value = 2)]
		EChannel_Whisper,
		[ProtoEnum(Name = "EChannel_CostumeParty", Value = 3)]
		EChannel_CostumeParty
	}
}
