using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PvpPillageResult")]
	[Serializable]
	public class MC2S_PvpPillageResult : IExtensible
	{
		private int _ResultKey;

		private CombatLog _Log;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ResultKey", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ResultKey
		{
			get
			{
				return this._ResultKey;
			}
			set
			{
				this._ResultKey = value;
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
