using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_UpdateFriendData")]
	[Serializable]
	public class MS2C_UpdateFriendData : IExtensible
	{
		private FriendData _Data;

		private int _UpdateFlag;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public FriendData Data
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

		[ProtoMember(2, IsRequired = false, Name = "UpdateFlag", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int UpdateFlag
		{
			get
			{
				return this._UpdateFlag;
			}
			set
			{
				this._UpdateFlag = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
