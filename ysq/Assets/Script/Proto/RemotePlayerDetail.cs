using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "RemotePlayerDetail")]
	[Serializable]
	public class RemotePlayerDetail : IExtensible
	{
		private readonly List<PetData> _Pets = new List<PetData>();

		private readonly List<ItemData> _Equips = new List<ItemData>();

		private int _FashionLevel;

		private LopetData _Lopet;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Pets", DataFormat = DataFormat.Default)]
		public List<PetData> Pets
		{
			get
			{
				return this._Pets;
			}
		}

		[ProtoMember(2, Name = "Equips", DataFormat = DataFormat.Default)]
		public List<ItemData> Equips
		{
			get
			{
				return this._Equips;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "FashionLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionLevel
		{
			get
			{
				return this._FashionLevel;
			}
			set
			{
				this._FashionLevel = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Lopet", DataFormat = DataFormat.Default), DefaultValue(null)]
		public LopetData Lopet
		{
			get
			{
				return this._Lopet;
			}
			set
			{
				this._Lopet = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
