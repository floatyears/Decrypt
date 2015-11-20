using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_GuildWarFightBegin")]
	[Serializable]
	public class MS2C_GuildWarFightBegin : IExtensible
	{
		private EGuildResult _Result;

		private int _Version;

		private RemotePlayer _Data;

		private RemotePlayerDetail _Data2;

		private ServerActorData _Data3;

		private int _Key;

		private int _MyHpPct;

		private int _EnemyHpPct;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Result", DataFormat = DataFormat.TwosComplement), DefaultValue(EGuildResult.EGR_Success)]
		public EGuildResult Result
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

		[ProtoMember(2, IsRequired = false, Name = "Version", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Version
		{
			get
			{
				return this._Version;
			}
			set
			{
				this._Version = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public RemotePlayer Data
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

		[ProtoMember(4, IsRequired = false, Name = "Data2", DataFormat = DataFormat.Default), DefaultValue(null)]
		public RemotePlayerDetail Data2
		{
			get
			{
				return this._Data2;
			}
			set
			{
				this._Data2 = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Data3", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ServerActorData Data3
		{
			get
			{
				return this._Data3;
			}
			set
			{
				this._Data3 = value;
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

		[ProtoMember(7, IsRequired = false, Name = "MyHpPct", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int MyHpPct
		{
			get
			{
				return this._MyHpPct;
			}
			set
			{
				this._MyHpPct = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "EnemyHpPct", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int EnemyHpPct
		{
			get
			{
				return this._EnemyHpPct;
			}
			set
			{
				this._EnemyHpPct = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
