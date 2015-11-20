using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "GuildMember")]
	[Serializable]
	public class GuildMember : IExtensible
	{
		private ulong _ID;

		private string _Name = string.Empty;

		private int _Level;

		private int _TotalReputation;

		private int _CurReputation;

		private int _LastOnlineTime;

		private int _Rank;

		private int _Flag;

		private int _FashionID;

		private int _ConstellationLevel;

		private int _JoinGuildTime;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0f)]
		public ulong ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				this._ID = value;
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

		[ProtoMember(4, IsRequired = false, Name = "TotalReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int TotalReputation
		{
			get
			{
				return this._TotalReputation;
			}
			set
			{
				this._TotalReputation = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "CurReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CurReputation
		{
			get
			{
				return this._CurReputation;
			}
			set
			{
				this._CurReputation = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "LastOnlineTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int LastOnlineTime
		{
			get
			{
				return this._LastOnlineTime;
			}
			set
			{
				this._LastOnlineTime = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Rank
		{
			get
			{
				return this._Rank;
			}
			set
			{
				this._Rank = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(9, IsRequired = false, Name = "FashionID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(10, IsRequired = false, Name = "ConstellationLevel", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(11, IsRequired = false, Name = "JoinGuildTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int JoinGuildTime
		{
			get
			{
				return this._JoinGuildTime;
			}
			set
			{
				this._JoinGuildTime = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
