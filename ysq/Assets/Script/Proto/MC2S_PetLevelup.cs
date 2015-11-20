using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_PetLevelup")]
	[Serializable]
	public class MC2S_PetLevelup : IExtensible
	{
		private ulong _PetID;

		private readonly List<ulong> _Pets = new List<ulong>();

		private bool _Flag;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PetID
		{
			get
			{
				return this._PetID;
			}
			set
			{
				this._PetID = value;
			}
		}

		[ProtoMember(2, Name = "Pets", DataFormat = DataFormat.TwosComplement)]
		public List<ulong> Pets
		{
			get
			{
				return this._Pets;
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
