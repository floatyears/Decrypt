using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "FriendData")]
	[Serializable]
	public class FriendData : IExtensible
	{
		private ulong _GUID;

		private string _Name = string.Empty;

		private int _Level;

		private int _VipLevel;

		private int _ConLevel;

		private int _FashionID;

		private int _Gender;

		private string _GuildName = string.Empty;

		private int _CombatValue;

		private int _Offline;

		private int _Flag;

		private int _FriendType;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "GUID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong GUID
		{
			get
			{
				return this._GUID;
			}
			set
			{
				this._GUID = value;
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

		[ProtoMember(3, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Level
		{
			get
			{
				return this._Level;
			}
			set
			{
				this._Level = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "ConLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ConLevel
		{
			get
			{
				return this._ConLevel;
			}
			set
			{
				this._ConLevel = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "GuildName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string GuildName
		{
			get
			{
				return this._GuildName;
			}
			set
			{
				this._GuildName = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CombatValue
		{
			get
			{
				return this._CombatValue;
			}
			set
			{
				this._CombatValue = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "Offline", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Offline
		{
			get
			{
				return this._Offline;
			}
			set
			{
				this._Offline = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Flag
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

		[ProtoMember(12, IsRequired = false, Name = "FriendType", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int FriendType
		{
			get
			{
				return this._FriendType;
			}
			set
			{
				this._FriendType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
