using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildBossStart")]
	[Serializable]
	public class MS2C_GuildBossStart : IExtensible
	{
		private int _Result;

		private int _ID;

		private int _InfoID;

		private float _HealthPct;

		private int _SceneID;

		private int _Key;

		private ServerActorData _Data;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Result
		{
			get
			{
				return this._Result;
			}
			set
			{
				this._Result = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int ID
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

		[ProtoMember(3, IsRequired = false, Name = "InfoID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int InfoID
		{
			get
			{
				return this._InfoID;
			}
			set
			{
				this._InfoID = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "HealthPct", DataFormat = DataFormat.FixedSize), DefaultValue(0f)]
		public float HealthPct
		{
			get
			{
				return this._HealthPct;
			}
			set
			{
				this._HealthPct = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "SceneID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int SceneID
		{
			get
			{
				return this._SceneID;
			}
			set
			{
				this._SceneID = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Key", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Key
		{
			get
			{
				return this._Key;
			}
			set
			{
				this._Key = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ServerActorData Data
		{
			get
			{
				return this._Data;
			}
			set
			{
				this._Data = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
