using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_EnterGame")]
	[Serializable]
	public class MS2C_EnterGame : IExtensible
	{
		private int _Result;

		private byte[] _CryptoKey;

		private string _UID = string.Empty;

		private string _AccessToken = string.Empty;

		private int _Privilege;

		private int _WorldID;

		private bool _GMAutoPatch;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "Result", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(2, IsRequired = false, Name = "CryptoKey", DataFormat = DataFormat.Default), DefaultValue(null)]
		public byte[] CryptoKey
		{
			get
			{
				return this._CryptoKey;
			}
			set
			{
				this._CryptoKey = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "UID", DataFormat = DataFormat.Default), DefaultValue("")]
		public string UID
		{
			get
			{
				return this._UID;
			}
			set
			{
				this._UID = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "AccessToken", DataFormat = DataFormat.Default), DefaultValue("")]
		public string AccessToken
		{
			get
			{
				return this._AccessToken;
			}
			set
			{
				this._AccessToken = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "Privilege", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int Privilege
		{
			get
			{
				return this._Privilege;
			}
			set
			{
				this._Privilege = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "WorldID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int WorldID
		{
			get
			{
				return this._WorldID;
			}
			set
			{
				this._WorldID = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "GMAutoPatch", DataFormat = DataFormat.Default), DefaultValue(false)]
		public bool GMAutoPatch
		{
			get
			{
				return this._GMAutoPatch;
			}
			set
			{
				this._GMAutoPatch = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
