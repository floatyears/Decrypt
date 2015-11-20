using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_OrePillageStart")]
	[Serializable]
	public class MC2S_OrePillageStart : IExtensible
	{
		private ulong _TargetID;

		private int _Amount;

		private bool _Flag;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "TargetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong TargetID
		{
			get
			{
				return this._TargetID;
			}
			set
			{
				this._TargetID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Amount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Amount
		{
			get
			{
				return this._Amount;
			}
			set
			{
				this._Amount = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Flag", DataFormat = DataFormat.Default), DefaultValue(false)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
