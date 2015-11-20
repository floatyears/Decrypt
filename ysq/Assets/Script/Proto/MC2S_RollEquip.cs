using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_RollEquip")]
	[Serializable]
	public class MC2S_RollEquip : IExtensible
	{
		private bool _Flag;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Flag", DataFormat = DataFormat.Default), DefaultValue(false)]
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
