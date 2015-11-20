using ProtoBuf;
using System;

namespace Att
{
	[ProtoContract(Name = "ESubTypeFragment")]
	public enum ESubTypeFragment
	{
		[ProtoEnum(Name = "EFragment_Pet", Value = 0)]
		EFragment_Pet,
		[ProtoEnum(Name = "EFragment_Equip", Value = 1)]
		EFragment_Equip,
		[ProtoEnum(Name = "EFragment_Trinket", Value = 2)]
		EFragment_Trinket,
		[ProtoEnum(Name = "EFragment_Lopet", Value = 3)]
		EFragment_Lopet,
		[ProtoEnum(Name = "EFragment_Max", Value = 4)]
		EFragment_Max
	}
}
