using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MS2C_ActivitySpecifyPayUpdate")]
	[Serializable]
	public class MS2C_ActivitySpecifyPayUpdate : IExtensible
	{
		private ActivitySpecifyPayData _SPData;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "SPData", DataFormat = DataFormat.Default), DefaultValue(null)]
		public ActivitySpecifyPayData SPData
		{
			get
			{
				return this._SPData;
			}
			set
			{
				this._SPData = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
