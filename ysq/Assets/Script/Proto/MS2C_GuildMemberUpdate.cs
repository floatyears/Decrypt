using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildMemberUpdate")]
	[Serializable]
	public class MS2C_GuildMemberUpdate : IExtensible
	{
		private ulong _ID;

		private int _Level;

		private int _TotalReputation;

		private int _CurReputation;

		private int _Rank;

		private int _Flag;

		private int _LastOnlineTime;

		private string _Name = string.Empty;

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

		[ProtoMember(2, IsRequired = false, Name = "Level", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "TotalReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(4, IsRequired = false, Name = "CurReputation", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(5, IsRequired = false, Name = "Rank", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(6, IsRequired = false, Name = "Flag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(7, IsRequired = false, Name = "LastOnlineTime", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(8, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
