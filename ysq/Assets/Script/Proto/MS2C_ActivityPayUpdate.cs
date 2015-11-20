using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivityPayUpdate")]
	[Serializable]
	public class MS2C_ActivityPayUpdate : IExtensible
	{
		private ActivityPayData _Data;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Data", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivityPayData Data
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
