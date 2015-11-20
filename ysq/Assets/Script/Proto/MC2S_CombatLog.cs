using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_CombatLog")]
	[Serializable]
	public class MC2S_CombatLog : IExtensible
	{
		private int _Type;

		private CombatLog _Log;

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

		[ProtoMember(2, IsRequired = false, Name = "Log", DataFormat = DataFormat.Default), DefaultValue(null)]
		public CombatLog Log
		{
			get
			{
				return this._Log;
			}
			set
			{
				this._Log = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
