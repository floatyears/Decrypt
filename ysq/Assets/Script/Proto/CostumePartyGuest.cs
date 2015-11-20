using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "CostumePartyGuest")]
	[Serializable]
	public class CostumePartyGuest : IExtensible
	{
		private ulong _PlayerID;

		private string _Name = string.Empty;

		private int _FashionID;

		private int _Timestamp;

		private int _VipLevel;

		private int _Gender;

		private int _PetID;

		private int _PetTimestamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong PlayerID
		{
			get
			{
				return this._PlayerID;
			}
			set
			{
				this._PlayerID = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FashionID
		{
			get
			{
				return this._FashionID;
			}
			set
			{
				this._FashionID = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Timestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Timestamp
		{
			get
			{
				return this._Timestamp;
			}
			set
			{
				this._Timestamp = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int VipLevel
		{
			get
			{
				return this._VipLevel;
			}
			set
			{
				this._VipLevel = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Gender", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Gender
		{
			get
			{
				return this._Gender;
			}
			set
			{
				this._Gender = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "PetID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PetID
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

		[ProtoMember(9, IsRequired = false, Name = "PetTimestamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PetTimestamp
		{
			get
			{
				return this._PetTimestamp;
			}
			set
			{
				this._PetTimestamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
