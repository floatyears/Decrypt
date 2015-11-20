using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildWarBattleRecord")]
	[Serializable]
	public class GuildWarBattleRecord : IExtensible
	{
		private bool _Attack;

		private bool _Win;

		private int _TimeStamp;

		private int _Level;

		private string _Name = string.Empty;

		private ulong _PlayerID;

		private int _FashionID;

		private int _VipLevel;

		private int _ConstellationLevel;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Attack", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Attack
		{
			get
			{
				return this._Attack;
			}
			set
			{
				this._Attack = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Win", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool Win
		{
			get
			{
				return this._Win;
			}
			set
			{
				this._Win = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "TimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TimeStamp
		{
			get
			{
				return this._TimeStamp;
			}
			set
			{
				this._TimeStamp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(6, IsRequired = false, Name = "PlayerID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
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

		[ProtoMember(7, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "VipLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(9, IsRequired = false, Name = "ConstellationLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ConstellationLevel
		{
			get
			{
				return this._ConstellationLevel;
			}
			set
			{
				this._ConstellationLevel = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
