using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_PvpPillageStart")]
	[Serializable]
	public class MS2C_PvpPillageStart : IExtensible
	{
		private int _Result;

		private int _Key;

		private RemotePlayerDetail _Data;

		private RemotePlayer _Data2;

		private ServerActorData _Data3;

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

		[ProtoMember(2, IsRequired = false, Name = "Key", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(3, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public RemotePlayerDetail Data
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
		public RemotePlayer Data2
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
