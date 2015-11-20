using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "BaseActivityData")]
	[Serializable]
	public class BaseActivityData : IExtensible
	{
		private int _ID;

		private int _CloseTimeStamp;

		private int _RewardTimeStamp;

		private string _Name = string.Empty;

		private string _Desc = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
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

		[ProtoMember(2, IsRequired = false, Name = "CloseTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int CloseTimeStamp
		{
			get
			{
				return this._CloseTimeStamp;
			}
			set
			{
				this._CloseTimeStamp = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "RewardTimeStamp", DataFormat = DataFormat.TwosComplement), DefaultValue(0)]
		public int RewardTimeStamp
		{
			get
			{
				return this._RewardTimeStamp;
			}
			set
			{
				this._RewardTimeStamp = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default), DefaultValue("")]
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

		[ProtoMember(5, IsRequired = false, Name = "Desc", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Desc
		{
			get
			{
				return this._Desc;
			}
			set
			{
				this._Desc = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
