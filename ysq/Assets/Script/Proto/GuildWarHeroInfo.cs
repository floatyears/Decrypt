using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarHeroInfo")]
	[Serializable]
	public class GuildWarHeroInfo : IExtensible
	{
		private ulong _GUID;

		private string _Name = string.Empty;

		private int _Level;

		private int _VipLevel;

		private int _ConLevel;

		private int _FashionID;

		private int _Gender;

		private int _CombatValue;

		private int _PraisedCount;

		private string _Message = string.Empty;

		private int _GuildRank;

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

		[ProtoMember(10, IsRequired = false, Name = "CombatValue", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "PraisedCount", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int PraisedCount
		{
			get
			{
				return this._PraisedCount;
			}
			set
			{
				this._PraisedCount = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "Message", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "GuildRank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int GuildRank
		{
			get
			{
				return this._GuildRank;
			}
			set
			{
				this._GuildRank = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
