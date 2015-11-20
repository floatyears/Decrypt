using ProtoBuf;
using System;
using System.ComponentModel;

namespace Proto
{
	[ProtoContract(Name = "MC2S_SetFameMessage")]
	[Serializable]
	public class MC2S_SetFameMessage : IExtensible
	{
		private string _Message = string.Empty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Message", DataFormat = DataFormat.Default), DefaultValue("")]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}
	}
}
