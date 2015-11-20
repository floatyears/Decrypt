using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_RequestSMSCode")]
	[Serializable]
	public class MC2S_RequestSMSCode : IExtensible
	{
		private string _PhoneNumber = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "PhoneNumber", DataFormat = DataFormat.Default), DefaultValue("")]
		public string PhoneNumber
		{
			get
			{
				return this._PhoneNumber;
			}
			set
			{
				this._PhoneNumber = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
