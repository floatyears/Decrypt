using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_QueryGuildData")]
	[Serializable]
	public class MS2C_QueryGuildData : IExtensible
	{
		private BriefGuildData _Data;

		private string _MasterName = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public BriefGuildData Data
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

		[ProtoMember(2, IsRequired = false, Name = "MasterName", DataFormat = DataFormat.Default), DefaultValue("")]
		public string MasterName
		{
			get
			{
				return this._MasterName;
			}
			set
			{
				this._MasterName = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
