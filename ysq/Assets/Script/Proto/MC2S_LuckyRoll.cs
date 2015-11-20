using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_LuckyRoll")]
	[Serializable]
	public class MC2S_LuckyRoll : IExtensible
	{
		private int _Type;

		private bool _Flag;

		private bool _Free;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Type
		{
			get
			{
				return this._Type;
			}
			set
			{
				this._Type = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Flag", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Flag
		{
			get
			{
				return this._Flag;
			}
			set
			{
				this._Flag = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Free", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Free
		{
			get
			{
				return this._Free;
			}
			set
			{
				this._Free = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
